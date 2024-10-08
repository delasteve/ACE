using System;

namespace ACE.Server.WorldObjects;

public class SkillFormula
{
    // everything else: melee weapons (including finesse), thrown weapons, atlatls
    public const float DefaultMod = 0.011f;

    // bows and crossbows
    public const float BowMod = 0.011f;

    // magic
    public const float SpellMod = 1000.0f;

    // defenses
    public const float ArmorMod = 100.0f;
    public const float ShieldMod = 2000.0f;
    public const float WardMod = 100.0f;

    public static float GetAttributeMod(int currentAttribute, ACE.Entity.Enum.Skill skill = ACE.Entity.Enum.Skill.None)
    {
        var factor = skill == ACE.Entity.Enum.Skill.Bow ? BowMod : DefaultMod;

        return Math.Max(1.0f + (currentAttribute - 55) * factor, 1.0f);
    }

    /// <summary>
    /// Converts SpellMod from an additive linear value
    /// to a scaled damage multiplier
    /// </summary>
    public static float CalcSpellMod(int magicSkill)
    {
        return 1 / (SpellMod / (magicSkill + SpellMod));
    }

    /// <summary>
    /// Converts AL from an additive linear value
    /// to a scaled damage multiplier
    /// </summary>
    public static float CalcArmorMod(float armorLevel)
    {
        if (armorLevel > 0)
        {
            return ArmorMod / (armorLevel + ArmorMod);
        }
        else if (armorLevel < 0)
        {
            return 1.0f - armorLevel / ArmorMod;
        }
        else
        {
            return 1.0f;
        }
    }

    /// <summary>
    /// Converts SL from an additive linear value
    /// to a scaled damage multiplier
    /// </summary>
    public static float CalcShieldMod(float shieldLevel)
    {
        if (shieldLevel > 0)
        {
            return ShieldMod / (shieldLevel + ShieldMod);
        }
        else if (shieldLevel < 0)
        {
            return 1.0f - shieldLevel / ShieldMod;
        }
        else
        {
            return 1.0f;
        }
    }

    /// <summary>
    /// Converts Ward Level from an additive linear value
    /// to a scaled damage multiplier
    /// </summary>
    public static float CalcWardMod(float wardLevel)
    {
        if (wardLevel > 0)
        {
            return WardMod / (wardLevel + WardMod);
        }
        else if (wardLevel < 0)
        {
            return 1.0f - wardLevel / WardMod;
        }
        else
        {
            return 1.0f;
        }
    }
}
