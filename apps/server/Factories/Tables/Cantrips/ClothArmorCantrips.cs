using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
using Serilog;

namespace ACE.Server.Factories.Tables.Cantrips;

public static class ClothArmorCantrips
{
    private static readonly ILogger _log = Log.ForContext(typeof(ClothArmorCantrips));

    private static readonly List<SpellId> spells = new List<SpellId>()
    {
        // creature cantrips

        SpellId.CANTRIPSTRENGTH1,
        SpellId.CANTRIPENDURANCE1,
        SpellId.CANTRIPCOORDINATION1,
        SpellId.CANTRIPQUICKNESS1,
        SpellId.CANTRIPFOCUS1,
        SpellId.CANTRIPWILLPOWER1,
        SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1, // Martial Weapons
        SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1, // Staff
        SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1, // Dagger
        SpellId.CANTRIPUNARMEDAPTITUDE1,
        SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1, // Bows
        SpellId.CANTRIPTHROWNAPTITUDE1,
        SpellId.CANTRIPTWOHANDEDAPTITUDE1,
        SpellId.CantripDualWieldAptitude1,
        SpellId.CantripShieldAptitude1,
        SpellId.CANTRIPINVULNERABILITY1,
        //SpellId.CANTRIPIMPREGNABILITY1,
        SpellId.CANTRIPMAGICRESISTANCE1,
        SpellId.CANTRIPLIFEMAGICAPTITUDE1,
        SpellId.CANTRIPWARMAGICAPTITUDE1,
        SpellId.CANTRIPMANACONVERSIONPROWESS1,
        //SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1,
        //SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,
        //SpellId.CantripVoidMagicAptitude1,
        //SpellId.CantripSummoningProwess1,

        SpellId.CANTRIPLOCKPICKPROWESS1, // Thievery
        SpellId.CANTRIPMONSTERATTUNEMENT1,
        SpellId.CANTRIPARCANEPROWESS1,
        SpellId.CANTRIPDECEPTIONPROWESS1,
        SpellId.CANTRIPHEALINGPROWESS1,
        SpellId.CANTRIPJUMPINGPROWESS1, // missing from original cantrips, but was in spells
        // should a separate lower armor cantrips table be added for this?
        SpellId.CANTRIPSPRINT1, // missing from original cantrips, but was in spells

        //SpellId.CANTRIPARMOR1,
        //SpellId.CANTRIPACIDWARD1,
        //SpellId.CANTRIPBLUDGEONINGWARD1,
        //SpellId.CANTRIPFLAMEWARD1,
        //SpellId.CANTRIPFROSTWARD1,
        //SpellId.CANTRIPPIERCINGWARD1,
        //SpellId.CANTRIPSLASHINGWARD1,
        //SpellId.CANTRIPSTORMWARD1,
    };

    private static readonly int NumLevels = 4;

    // original api
    public static readonly SpellId[][] Table = new SpellId[spells.Count][];

    static ClothArmorCantrips()
    {
        // takes ~0.3ms
        BuildSpells();
    }

    private static void BuildSpells()
    {
        for (var i = 0; i < spells.Count; i++)
        {
            Table[i] = new SpellId[NumLevels];
        }

        for (var i = 0; i < spells.Count; i++)
        {
            var spell = spells[i];

            var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

            if (spellLevels == null)
            {
                _log.Error($"ClothArmorCantrips - couldn't find {spell}");
                continue;
            }

            if (spellLevels.Count != NumLevels)
            {
                _log.Error($"ClothArmorCantrips - expected {NumLevels} levels for {spell}, found {spellLevels.Count}");
                continue;
            }

            for (var j = 0; j < NumLevels; j++)
            {
                Table[i][j] = spellLevels[j];
            }
        }
    }

    private static ChanceTable<SpellId> clothArmorCantrips = new ChanceTable<SpellId>(ChanceTableType.Weight)
    {
        (SpellId.CANTRIPSTRENGTH1, 1.0f),
        (SpellId.CANTRIPENDURANCE1, 1.0f),
        (SpellId.CANTRIPCOORDINATION1, 1.0f),
        (SpellId.CANTRIPQUICKNESS1, 1.0f),
        (SpellId.CANTRIPFOCUS1, 1.0f),
        (SpellId.CANTRIPWILLPOWER1, 1.0f),
        (SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1, 1.0f),
        (SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1, 1.0f),
        (SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1, 1.0f),
        (SpellId.CANTRIPUNARMEDAPTITUDE1, 1.0f),
        (SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1, 1.0f),
        (SpellId.CANTRIPTWOHANDEDAPTITUDE1, 1.0f),
        (SpellId.CANTRIPINVULNERABILITY1, 1.0f),
        (SpellId.CANTRIPMAGICRESISTANCE1, 1.0f),
        (SpellId.CANTRIPLIFEMAGICAPTITUDE1, 1.0f),
        (SpellId.CANTRIPWARMAGICAPTITUDE1, 1.0f),

        (SpellId.CANTRIPARMOR1, 1.0f),
        (SpellId.CANTRIPACIDWARD1, 1.0f),
        (SpellId.CANTRIPBLUDGEONINGWARD1, 1.0f),
        (SpellId.CANTRIPFLAMEWARD1, 1.0f),
        (SpellId.CANTRIPFROSTWARD1, 1.0f),
        (SpellId.CANTRIPPIERCINGWARD1, 1.0f),
        (SpellId.CANTRIPSLASHINGWARD1, 1.0f),
        (SpellId.CANTRIPSTORMWARD1, 1.0f),

        (SpellId.CANTRIPARCANEPROWESS1, 1.0f),
        (SpellId.CANTRIPJUMPINGPROWESS1, 1.0f),
        (SpellId.CANTRIPJUMPINGPROWESS1, 1.0f),
        (SpellId.CANTRIPLOCKPICKPROWESS1, 1.0f),
        (SpellId.CANTRIPMANACONVERSIONPROWESS1, 1.0f ),
        (SpellId.CANTRIPMONSTERATTUNEMENT1, 1.0f),
        (SpellId.CANTRIPDECEPTIONPROWESS1, 1.0f),
        (SpellId.CANTRIPSPRINT1, 1.0f),

        (SpellId.CantripDualWieldAptitude1, 1.0f),
        (SpellId.CantripShieldAptitude1, 1.0f),
    };

    public static SpellId Roll()
    {
        return clothArmorCantrips.Roll();
    }
}
