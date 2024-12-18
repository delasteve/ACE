using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ItemType = ACE.Entity.Enum.ItemType;
using MotionCommand = ACE.Entity.Enum.MotionCommand;
using SpellId = ACE.Entity.Enum.SpellId;

namespace ACE.Server.WorldObjects;

public class SpellTransference : Stackable
{
    /// <summary>
    /// A new biota be created taking all of its values from weenie.
    /// </summary>
    public SpellTransference(Weenie weenie, ObjectGuid guid)
        : base(weenie, guid)
    {
        SetEphemeralValues();
    }

    /// <summary>
    /// Restore a WorldObject from the database.
    /// </summary>
    public SpellTransference(Biota biota)
        : base(biota)
    {
        SetEphemeralValues();
    }

    private void SetEphemeralValues() { }

    public static void BroadcastSpellTransfer(
        Player player,
        string spellName,
        WorldObject target,
        double chance = 1.0f,
        bool success = true
    )
    {
        // send local broadcast
        if (success)
        {
            player.EnqueueBroadcast(
                new GameMessageSystemChat(
                    $"{player.Name} successfully transfers {spellName} to the {target.NameWithMaterial}.",
                    ChatMessageType.Craft
                ),
                8f,
                ChatMessageType.Craft
            );
        }
        else
        {
            player.EnqueueBroadcast(
                new GameMessageSystemChat(
                    $"{player.Name} fails to transfer {spellName} to the {target.NameWithMaterial}. The target is destroyed.",
                    ChatMessageType.Craft
                ),
                8f,
                ChatMessageType.Craft
            );
        }
    }

    public static void BroadcastSpellExtraction(
        Player player,
        string spellName,
        WorldObject target,
        double chance,
        bool success
    )
    {
        // send local broadcast
        if (success)
        {
            player.EnqueueBroadcast(
                new GameMessageSystemChat(
                    $"{player.Name} successfully extracts {spellName} from the {target.NameWithMaterial}. The target is destroyed.",
                    ChatMessageType.Craft
                ),
                8f,
                ChatMessageType.Craft
            );
        }
        else
        {
            player.EnqueueBroadcast(
                new GameMessageSystemChat(
                    $"{player.Name} fails to extract {spellName} from the {target.NameWithMaterial}. The target is destroyed.",
                    ChatMessageType.Craft
                ),
                8f,
                ChatMessageType.Craft
            );
        }
    }

    public override void HandleActionUseOnTarget(Player player, WorldObject target)
    {
        UseObjectOnTarget(player, this, target);
    }

    public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target, bool confirmed = false)
    {
        if (source.SpellExtracted == null)
        {
            var pearlStackSize = source.StackSize ?? 1;
            var targetWorkmanship = Math.Clamp((target.ItemWorkmanship ?? 1) - 1, 1, 10);
            var amountToAdd = targetWorkmanship * targetWorkmanship;
            var consumed = amountToAdd > 1 ? $"and consuming {amountToAdd} pearls" : "";

            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (target.WeenieType == source.WeenieType)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (!RecipeManager.VerifyUse(player, source, target, true))
            {
                if (!confirmed)
                {
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                }
                else
                {
                    player.SendTransientError(
                        "Either you or one of the items involved does not pass the requirements for this craft interaction."
                    );
                }

                return;
            }

            if (target.Workmanship == null || target.Tier == null)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (target.Retained == true)
            {
                player.Session.Network.EnqueueSend(
                    new GameMessageSystemChat(
                        $"The {target.NameWithMaterial} is retained and cannot be dismantled for a contained spell.",
                        ChatMessageType.Craft
                    )
                );
                player.SendUseDoneEvent();
                return;
            }

            if (pearlStackSize < amountToAdd)
            {
                player.Session.Network.EnqueueSend(
                    new GameMessageSystemChat(
                        $"You require {amountToAdd} Pearls of Spell Transference to extract a spell from {target.NameWithMaterial}.",
                        ChatMessageType.Craft
                    )
                );
                player.SendUseDoneEvent();
                return;
            }

            var spellCount = 0;
            var allSpells = target.Biota.GetKnownSpellsIds(target.BiotaDatabaseLock);

            if (target.ProcSpell != null && target.ProcSpell != 0)
            {
                allSpells.Add((int)target.ProcSpell);
            }

            var spells = new List<int>();

            foreach (var spellId in allSpells)
            {
                var spell = new Spell(spellId);
                spells.Add(spellId);
            }

            spellCount = spells.Count;
            if (spellCount == 0)
            {
                player.Session.Network.EnqueueSend(
                    new GameMessageSystemChat(
                        $"The {target.NameWithMaterial} does not have any valid spells to extract.",
                        ChatMessageType.Craft
                    )
                );
                player.SendUseDoneEvent();
                return;
            }
            // if not confirmed yet, we select a spell from the item and assign the ID to the pearl's SpellToExtract property.
            // confirmation runs this method a second time after hitting yes
            // with multi-spell items, repeated "No" confirmations will cycle through the item's spells
            if (!confirmed)
            {
                if (target.RemainingConfirmations == null)
                {
                    target.RemainingConfirmations = spellCount;
                }

                var spellToExtractRoll = (target.RemainingConfirmations ?? 1) - 1;
                var spellToExtractId = spells[spellToExtractRoll];

                source.SpellToExtract = (uint?)spellToExtractId;

                target.RemainingConfirmations--;
                if (target.RemainingConfirmations == 0)
                {
                    target.RemainingConfirmations = null;
                }
            }

            if (source.SpellToExtract == null)
            {
                _log.Error("UseObjectOnTarget() - {Source}.SpellToExtract is null", source);
                return;
            }

            var chosenSpell = new Spell((uint)source.SpellToExtract);
            var chance = 100;

            if (!confirmed)
            {
                if (
                    !player.ConfirmationManager.EnqueueSend(
                        new Confirmation_CraftInteration(player.Guid, source.Guid, target.Guid),
                        $"Extract {chosenSpell.Name} from {target.NameWithMaterial}, destroying it {consumed} in the process?\n\nIf this item contains more than one spell, selecting 'No' will cycle through the remaining spells.\n\n"
                    )
                )
                {
                    player.SendUseDoneEvent(WeenieError.ConfirmationInProgress);
                }

                if (PropertyManager.GetBool("craft_exact_msg").Item)
                {
                    var exactMsg = $"You have a 100% chance of extracting a spell from {target.NameWithMaterial}.";

                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(exactMsg, ChatMessageType.Craft));
                }

                return;
            }

            var actionChain = new ActionChain();

            var animTime = 0.0f;

            player.IsBusy = true;

            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                animTime += stanceTime;
            }

            animTime += player.EnqueueMotion(actionChain, MotionCommand.ClapHands);

            actionChain.AddAction(
                player,
                () =>
                {
                    if (!RecipeManager.VerifyUse(player, source, target, true))
                    {
                        player.SendTransientError(
                            "Either you or one of the items involved does not pass the requirements for this craft interaction."
                        );
                        return;
                    }
                    var pearl = WorldObjectFactory.CreateNewWorldObject(1054001);
                    var success = true;
                    var spellName = "";
                    if (success)
                    {
                        var spell = new Spell((int)source.SpellToExtract);
                        spellName = spell.Name;

                        pearl.SpellExtracted = source.SpellToExtract;

                        var itemType = "";
                        if (target.ItemType == ItemType.Jewelry)
                        {
                            itemType = "a piece of jewelry";
                        }
                        if (target.ItemType == ItemType.Armor)
                        {
                            itemType = "a piece of armor";
                        }
                        if (target.ItemType == ItemType.MissileWeapon || target.ItemType == ItemType.MeleeWeapon)
                        {
                            itemType = "a missile or melee weapon";
                        }
                        if (target.ItemType == ItemType.Caster)
                        {
                            itemType = "a magic caster";
                        }
                        if (target.ItemType == ItemType.Clothing)
                        {
                            itemType = "a piece of clothing";
                        }

                        player.TryConsumeFromInventoryWithNetworking(source, amountToAdd);

                        pearl.Tier = target.Tier;
                        var wieldReq = LootGenerationFactory.GetWieldDifficultyPerTier(pearl.Tier ?? 1);
                        pearl.LongDesc =
                            $"This pearl contains the spell {spell.Name}.\n\nIt may only be applied to {itemType} with a Wield Requirement of {wieldReq} or greater.\n\nAdding this spell will increase Spellcraft and Arcane Lore of the target item, and will bind it to your character.\n\nIf the spell is an on-hit weapon proc, it will add a Life or War Magic skill wield requirement as well.";
                        pearl.TinkerLog = $"{target.ItemType}";
                        pearl.UiEffects = ACE.Entity.Enum.UiEffects.BoostMana;
                        player.EnqueueBroadcast(new GameMessageUpdateObject(source));
                        player.PlayParticleEffect(PlayScript.EnchantUpBlue, player.Guid);

                        player.TryConsumeFromInventoryWithNetworking(target, amountToAdd);

                        player.TryCreateInInventoryWithNetworking(pearl);
                    }

                    BroadcastSpellExtraction(player, spellName, target, chance, success);
                }
            );

            player.EnqueueMotion(actionChain, MotionCommand.Ready);

            actionChain.AddAction(
                player,
                () =>
                {
                    player.SendUseDoneEvent();
                    player.IsBusy = false;
                }
            );

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
        }
        // handle Transference
        else
        {
            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (!RecipeManager.VerifyUse(player, source, target, true))
            {
                if (!confirmed)
                {
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                }
                else
                {
                    player.SendTransientError(
                        "Either you or one of the items involved does not pass the requirements for this craft interaction."
                    );
                }

                return;
            }

            if (target.Workmanship == null || target.Tier == null)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (target.Tier < source.Tier)
            {
                player.Session.Network.EnqueueSend(
                    new GameMessageSystemChat(
                        $"The {target.NameWithMaterial} is not powerful enough to contain the spell.",
                        ChatMessageType.Craft
                    )
                );
                player.SendUseDoneEvent();
                return;
            }

            var validType = false;

            if (Enum.TryParse<ItemType>(source.TinkerLog, out var itemType))
            {
                if (itemType == ItemType.MissileWeapon || itemType == ItemType.MeleeWeapon)
                {
                    if (target.ItemType != ItemType.MissileWeapon && target.ItemType != ItemType.MeleeWeapon)
                    {
                        player.Session.Network.EnqueueSend(
                            new GameMessageSystemChat(
                                $"The {source.Name} cannot be used on an item of that type.",
                                ChatMessageType.Craft
                            )
                        );
                        player.SendUseDoneEvent();
                        return;
                    }
                    else
                    {
                        validType = true;
                    }
                }

                if (target.ItemType == itemType)
                {
                    validType = true;
                }
            }

            if (!validType)
            {
                player.Session.Network.EnqueueSend(
                    new GameMessageSystemChat(
                        $"The {source.Name} cannot be used on an item of that type.",
                        ChatMessageType.Craft
                    )
                );
                player.SendUseDoneEvent();
                return;
            }

            if (validType)
            {
                var spellToAddId = (uint)source.SpellExtracted;
                var spellToAddlevel1Id = SpellLevelProgression.GetLevel1SpellId((SpellId)spellToAddId);
                var spellToAdd = new Spell(spellToAddId);

                var isProc = false;
                if (
                    spellToAddlevel1Id != SpellId.Undef
                    && (weaponProcs.FirstOrDefault(x => x.result == spellToAddlevel1Id) != default((SpellId, float)))
                )
                {
                    isProc = true;

                    if (target.ItemType != ItemType.MeleeWeapon && target.ItemType != ItemType.MissileWeapon)
                    {
                        player.Session.Network.EnqueueSend(
                            new GameMessageSystemChat(
                                $"The {target.NameWithMaterial} cannot contain {spellToAdd.Name}.",
                                ChatMessageType.Craft
                            )
                        );
                        player.SendUseDoneEvent();
                        return;
                    }
                }

                // Get spells from target item to check conflicts/overlaps

                var spellsOnItem = target.Biota.GetKnownSpellsIds(target.BiotaDatabaseLock);

                if (target.ProcSpell != null && target.ProcSpell != 0)
                {
                    spellsOnItem.Add((int)target.ProcSpell);
                }

                Spell spellToReplace = null;
                foreach (var spellOnItemId in spellsOnItem)
                {
                    var spellOnItem = new Spell(spellOnItemId);

                    if (spellOnItemId == spellToAddId)
                    {
                        player.Session.Network.EnqueueSend(
                            new GameMessageSystemChat(
                                $"The {target.NameWithMaterial} already contains {spellToAdd.Name}.",
                                ChatMessageType.Craft
                            )
                        );
                        player.SendUseDoneEvent();
                        return;
                    }
                    else if (spellOnItem.Category == spellToAdd.Category)
                    {
                        if (spellOnItem.Power > spellToAdd.Power)
                        {
                            player.Session.Network.EnqueueSend(
                                new GameMessageSystemChat(
                                    $"The {target.NameWithMaterial} already contains {spellOnItem.Name}, which is stronger than {spellToAdd.Name}.",
                                    ChatMessageType.Craft
                                )
                            );
                            player.SendUseDoneEvent();
                            return;
                        }
                        else if (spellOnItem.Power == spellToAdd.Power)
                        {
                            player.Session.Network.EnqueueSend(
                                new GameMessageSystemChat(
                                    $"The {target.NameWithMaterial} already contains {spellOnItem.Name}, which is equivalent to {spellToAdd.Name}.",
                                    ChatMessageType.Craft
                                )
                            );
                            player.SendUseDoneEvent();
                            return;
                        }
                        else
                        {
                            spellToReplace = spellOnItem;
                        }
                    }
                }

                if (!confirmed)
                {
                    var extraMessage = "";
                    if (isProc && target.ProcSpell != null)
                    {
                        var currentProc = new Spell(target.ProcSpell ?? 0);
                        extraMessage = $"\nThis will replace {currentProc.Name}!\n";
                    }
                    else if (spellToReplace != null)
                    {
                        extraMessage = $"\nThis will replace {spellToReplace.Name}!\n";
                    }

                    var potentialArcaneLoreReq = CalculateArcaneLore(target, (int)spellToAdd.Id);
                    var arcaneLoreString = potentialArcaneLoreReq > 0
                        ? $"The {target.Name}'s arcane lore activation requirement will become {potentialArcaneLoreReq}.\n\n" : "";

                    if (
                        !player.ConfirmationManager.EnqueueSend(
                            new Confirmation_CraftInteration(player.Guid, source.Guid, target.Guid),
                            $"Transfer {spellToAdd.Name} to {target.NameWithMaterial}?\n\n" +
                            $"{arcaneLoreString}" +
                            $"The pearl will be consumed and the {target.Name} will become bound to your character.\n\n{extraMessage}"
                        )
                    )
                    {
                        player.SendUseDoneEvent(WeenieError.ConfirmationInProgress);
                    }
                    else
                    {
                        player.SendUseDoneEvent();
                    }

                    return;
                }

                var actionChain = new ActionChain();

                var animTime = 0.0f;

                player.IsBusy = true;

                if (player.CombatMode != CombatMode.NonCombat)
                {
                    var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                    actionChain.AddDelaySeconds(stanceTime);

                    animTime += stanceTime;
                }

                animTime += player.EnqueueMotion(actionChain, MotionCommand.ClapHands);

                actionChain.AddAction(
                    player,
                    () =>
                    {
                        if (!RecipeManager.VerifyUse(player, source, target, true))
                        {
                            player.SendTransientError(
                                "Either you or one of the items involved does not pass the requirements for this craft interaction."
                            );
                            return;
                        }

                        if (isProc)
                        {
                            target.ProcSpellRate = 0.15f;
                            target.ProcSpell = (uint)spellToAddId;
                            target.ProcSpellSelfTargeted = spellToAdd.IsSelfTargeted;
                            target.WieldRequirements2 = WieldRequirement.Training;
                            target.WieldDifficulty2 = 2;

                            if (spellToAdd.School == MagicSchool.LifeMagic)
                            {
                                target.WieldSkillType2 = 33;
                            }

                            if (spellToAdd.School == MagicSchool.WarMagic)
                            {
                                target.WieldSkillType2 = 34;
                            }
                        }
                        else
                        {
                            if (spellToReplace != null)
                            {
                                target.Biota.TryRemoveKnownSpell((int)spellToReplace.Id, target.BiotaDatabaseLock);
                            }
                            else
                            {
                                target.Biota.GetOrAddKnownSpell((int)spellToAddId, target.BiotaDatabaseLock, out _);
                            }
                        }

                        var newMaxBaseMana = LootGenerationFactory.GetCombinedSpellManaCost(target);
                        var newManaRate = LootGenerationFactory.CalculateManaRate(target);
                        var newMaxMana = (int)spellToAdd.BaseMana * 15;

                        if (newMaxMana > (target.ItemMaxMana ?? 0))
                        {
                            target.ItemMaxMana = newMaxMana;
                            target.ItemCurMana = Math.Clamp(target.ItemCurMana ?? 0, 0, target.ItemMaxMana ?? 0);

                            target.ManaRate = newManaRate;
                            target.LongDesc = LootGenerationFactory.GetLongDesc(target);
                        }

                        target.ItemDifficulty = CalculateArcaneLore(target);
                        target.ItemSpellcraft = LootGenerationFactory.RollSpellcraft(target);

                        if (!target.UiEffects.HasValue)
                        {
                            target.UiEffects = ACE.Entity.Enum.UiEffects.Magical;
                        }

                        player.EnqueueBroadcast(new GameMessageUpdateObject(target));
                        player.PlayParticleEffect(PlayScript.EnchantUpBlue, player.Guid);

                        player.TryConsumeFromInventoryWithNetworking(source);
                        BroadcastSpellTransfer(player, spellToAdd.Name, target);
                    }
                );

                player.EnqueueMotion(actionChain, MotionCommand.Ready);

                actionChain.AddAction(
                    player,
                    () =>
                    {
                        player.IsBusy = false;
                    }
                );

                actionChain.EnqueueChain();

                player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
            }
        }
    }

    private static int CalculateArcaneLore(WorldObject target, int? potentialSpell = null)
    {
        var numSpells = 0;
        var increasedDifficulty = 0.0f;

        var targetSpellBook = target.Biota.PropertiesSpellBook;

        if (targetSpellBook == null)
        {
            return 0;
        }

        var spellBook = new Dictionary<int,float>(targetSpellBook);

        if (potentialSpell != null)
        {
            spellBook.Add(potentialSpell.Value, 1.0f);
        }

        int MINOR = 0,
            MAJOR = 1,
            EPIC = 2,
            LEGENDARY = 3;

        foreach (SpellId spellId in spellBook.Keys)
        {
            numSpells++;

            var cantripLevels = SpellLevelProgression.GetSpellLevels(spellId);

            var cantripLevel = cantripLevels.IndexOf(spellId);

            if (cantripLevel == MINOR)
            {
                increasedDifficulty += 5;
            }
            else if (cantripLevel == MAJOR)
            {
                increasedDifficulty += 10;
            }
            else if (cantripLevel == EPIC)
            {
                increasedDifficulty += 15;
            }
            else if (cantripLevel == LEGENDARY)
            {
                increasedDifficulty += 20;
            }
        }

        var tier = (target.Tier ?? 1) - 1;

        if (target.ProcSpell != null)
        {
            numSpells++;
            increasedDifficulty += Math.Max(5 * tier, 5);
        }

        var finalDifficulty = 0;
        var armorSlots = target.ArmorSlots ?? 1;
        var spellsPerSlot = (float)numSpells / armorSlots;

        if (spellsPerSlot > 1 || target.ProcSpell != null)
        {
            var baseDifficulty = ActivationDifficultyPerTier(tier);

            finalDifficulty = baseDifficulty + (int)(increasedDifficulty / armorSlots);
        }

        return finalDifficulty;
    }

    private static int ActivationDifficultyPerTier(int tier)
    {
        switch (tier)
        {
            case 1:
                return 75;
            case 2:
                return 175;
            case 3:
                return 225;
            case 4:
                return 275;
            case 5:
                return 325;
            case 6:
                return 375;
            case 7:
                return 425;
            default:
                return 50;
        }
    }

    public static ChanceTable<SpellId> weaponProcs = new ChanceTable<SpellId>(ChanceTableType.Weight)
    {
        (SpellId.StaminaToManaSelf1, 1.0f),
        (SpellId.ManaToStaminaSelf1, 1.0f),
        (SpellId.ManaToHealthSelf1, 1.0f),
        (SpellId.DrainMana1, 1.0f),
        (SpellId.DrainStamina1, 1.0f),
        (SpellId.DrainHealth1, 1.0f),
        (SpellId.ManaBoostSelf1, 1.0f),
        (SpellId.RevitalizeSelf1, 1.0f),
        (SpellId.HealSelf1, 1.0f),
        (SpellId.HarmOther1, 1.0f),
        (SpellId.ExhaustionOther1, 1.0f),
        (SpellId.ManaDrainOther1, 1.0f),
        (SpellId.WhirlingBlade1, 1.0f),
        (SpellId.ForceBolt1, 1.0f),
        (SpellId.ShockWave1, 1.0f),
        (SpellId.AcidStream1, 1.0f),
        (SpellId.FlameBolt1, 1.0f),
        (SpellId.FrostBolt1, 1.0f),
        (SpellId.LightningBolt1, 1.0f),
    };
}
