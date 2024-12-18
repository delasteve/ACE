using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories;

public static partial class LootGenerationFactory
{
    // legacy chance tables

    // used by multiple item types

    private static int RollWieldDifficulty(int tier, TreasureWeaponType weaponType = TreasureWeaponType.MeleeWeapon)
    {
        var wield = 0;

        switch (weaponType)
        {
            case TreasureWeaponType.MeleeWeapon:

                switch (tier)
                {
                    case 1:
                        wield = 50;
                        break;
                    case 2:
                        wield = 125;
                        break;
                    case 3:
                        wield = 175;
                        break;
                    case 4:
                        wield = 230;
                        break;
                    case 5:
                        wield = 260;
                        break;
                    case 6:
                        wield = 290;
                        break;
                    case 7:
                        wield = 320;
                        break;
                    case 8:
                        wield = 350;
                        break;
                }
                break;

            case TreasureWeaponType.MissileWeapon:

                switch (tier)
                {
                    case 1:
                        wield = 50;
                        break;
                    case 2:
                        wield = 125;
                        break;
                    case 3:
                        wield = 175;
                        break;
                    case 4:
                        wield = 230;
                        break;
                    case 5:
                        wield = 260;
                        break;
                    case 6:
                        wield = 290;
                        break;
                    case 7:
                        wield = 320;
                        break;
                    case 8:
                        wield = 350;
                        break;
                }
                break;

            case TreasureWeaponType.Caster:

                switch (tier)
                {
                    case 1:
                        wield = 50;
                        break;
                    case 2:
                        wield = 125;
                        break;
                    case 3:
                        wield = 175;
                        break;
                    case 4:
                        wield = 230;
                        break;
                    case 5:
                        wield = 260;
                        break;
                    case 6:
                        wield = 290;
                        break;
                    case 7:
                        wield = 320;
                        break;
                    case 8:
                        wield = 350;
                        break;
                }
                break;
        }
        return wield;
    }

    private static double GetMaxDamageMod(int tier, int maxDamageMod)
    {
        double damageMod = 0;

        switch (maxDamageMod)
        {
            case 15:
                //tier 1
                switch (tier)
                {
                    case 1:
                        damageMod = 0;
                        break;
                    case 2:
                        var chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 80)
                        {
                            damageMod = .01;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .02;
                        }
                        else
                        {
                            damageMod = .03;
                        }

                        break;
                    case 3:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .03;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .04;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .05;
                        }
                        else
                        {
                            damageMod = .06;
                        }

                        break;
                    case 4:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .06;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .07;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .08;
                        }
                        else
                        {
                            damageMod = .09;
                        }

                        break;
                    case 5:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .09;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .10;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .11;
                        }
                        else
                        {
                            damageMod = .12;
                        }

                        break;
                    case 6:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .09;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .10;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .11;
                        }
                        else
                        {
                            damageMod = .12;
                        }

                        break;
                    case 7:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .11;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .12;
                        }
                        else
                        {
                            damageMod = .13;
                        }

                        break;
                    case 8:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .13;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .14;
                        }
                        else
                        {
                            damageMod = .15;
                        }

                        break;
                }
                break;
            case 18:
                //tier 1
                switch (tier)
                {
                    case 1:
                        damageMod = 0;
                        break;
                    case 2:
                        var chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 80)
                        {
                            damageMod = .01;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .02;
                        }
                        else
                        {
                            damageMod = .03;
                        }

                        break;
                    case 3:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .03;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .04;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .05;
                        }
                        else
                        {
                            damageMod = .06;
                        }

                        break;
                    case 4:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .06;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .07;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .08;
                        }
                        else
                        {
                            damageMod = .09;
                        }

                        break;
                    case 5:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .09;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .10;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .11;
                        }
                        else
                        {
                            damageMod = .12;
                        }

                        break;
                    case 6:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .12;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .13;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .14;
                        }
                        else
                        {
                            damageMod = .15;
                        }

                        break;
                    case 7:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .15;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .16;
                        }
                        else
                        {
                            damageMod = .17;
                        }

                        break;
                    case 8:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .16;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .17;
                        }
                        else
                        {
                            damageMod = .18;
                        }

                        break;
                }
                break;
            case 20:
                //tier 1
                switch (tier)
                {
                    case 1:
                        damageMod = 0;
                        break;
                    case 2:
                        var chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 80)
                        {
                            damageMod = .01;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .02;
                        }
                        else
                        {
                            damageMod = .03;
                        }

                        break;
                    case 3:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .02;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .03;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .04;
                        }
                        else
                        {
                            damageMod = .05;
                        }

                        break;
                    case 4:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .05;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .06;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .07;
                        }
                        else
                        {
                            damageMod = .08;
                        }

                        break;
                    case 5:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .07;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .08;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .09;
                        }
                        else
                        {
                            damageMod = .10;
                        }

                        break;
                    case 6:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .11;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .12;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .13;
                        }
                        else
                        {
                            damageMod = .14;
                        }

                        break;
                    case 7:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .13;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .14;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .15;
                        }
                        else
                        {
                            damageMod = .16;
                        }

                        break;
                    case 8:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .17;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .18;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .19;
                        }
                        else
                        {
                            damageMod = .20;
                        }

                        break;
                }
                break;
            case 22:
                //tier 1
                switch (tier)
                {
                    case 1:
                        damageMod = 0;
                        break;
                    case 2:
                        var chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 80)
                        {
                            damageMod = .01;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .02;
                        }
                        else
                        {
                            damageMod = .03;
                        }

                        break;
                    case 3:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .02;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .03;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .04;
                        }
                        else
                        {
                            damageMod = .05;
                        }

                        break;
                    case 4:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .05;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .06;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .07;
                        }
                        else
                        {
                            damageMod = .08;
                        }

                        break;
                    case 5:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .07;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .08;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .09;
                        }
                        else
                        {
                            damageMod = .10;
                        }

                        break;
                    case 6:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .11;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .12;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .13;
                        }
                        else
                        {
                            damageMod = .14;
                        }

                        break;
                    case 7:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .14;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .15;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .16;
                        }
                        else
                        {
                            damageMod = .17;
                        }

                        break;
                    case 8:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .18;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .19;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .20;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .21;
                        }
                        else
                        {
                            damageMod = .22;
                        }

                        break;
                }
                break;
            case 25:
                //tier 1
                switch (tier)
                {
                    case 1:
                        damageMod = 0;
                        break;
                    case 2:
                        var chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 80)
                        {
                            damageMod = .01;
                        }
                        else if (chance < 70)
                        {
                            damageMod = .02;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .03;
                        }
                        else
                        {
                            damageMod = .04;
                        }

                        break;
                    case 3:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .04;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .05;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .06;
                        }
                        else
                        {
                            damageMod = .07;
                        }

                        break;
                    case 4:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .07;
                        }
                        else if (chance < 70)
                        {
                            damageMod = .08;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .09;
                        }
                        else if (chance < 96)
                        {
                            damageMod = .10;
                        }
                        else
                        {
                            damageMod = .11;
                        }

                        break;
                    case 5:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .11;
                        }
                        else if (chance < 70)
                        {
                            damageMod = .12;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .13;
                        }
                        else if (chance < 96)
                        {
                            damageMod = .14;
                        }
                        else
                        {
                            damageMod = .15;
                        }

                        break;
                    case 6:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .15;
                        }
                        else if (chance < 70)
                        {
                            damageMod = .16;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .17;
                        }
                        else
                        {
                            damageMod = .18;
                        }

                        break;
                    case 7:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .18;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .19;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .20;
                        }
                        else
                        {
                            damageMod = .21;
                        }

                        break;
                    case 8:
                        chance = ThreadSafeRandom.Next(0, 99);
                        if (chance < 50)
                        {
                            damageMod = .21;
                        }
                        else if (chance < 80)
                        {
                            damageMod = .22;
                        }
                        else if (chance < 90)
                        {
                            damageMod = .23;
                        }
                        else if (chance < 95)
                        {
                            damageMod = .24;
                        }
                        else
                        {
                            damageMod = .25;
                        }

                        break;
                }
                break;
            default:
                break;
        }
        return damageMod + 1;
    }

    /// <summary>
    /// Rolls for a WeaponDefense for a weapon
    /// </summary>
    private static double RollWeaponDefense(int wield, TreasureDeath profile)
    {
        double meleeMod = 0;

        var chance = ThreadSafeRandom.Next(1, 100);
        switch (wield)
        {
            case 0:
                switch (profile.Tier) // Only Tiers 1-6
                {
                    case 0:
                    case 1:
                        meleeMod = 0;
                        break;
                    case 2:
                        if (chance <= 20)
                        {
                            meleeMod = 0.01;
                        }
                        else if (chance <= 45)
                        {
                            meleeMod = 0.02;
                        }
                        else if (chance <= 65)
                        {
                            meleeMod = 0.03;
                        }
                        else if (chance <= 85)
                        {
                            meleeMod = 0.04;
                        }
                        else
                        {
                            meleeMod = 0.05;
                        }

                        break;
                    case 3:
                        if (chance <= 20)
                        {
                            meleeMod = 0.03;
                        }
                        else if (chance <= 45)
                        {
                            meleeMod = 0.03;
                        }
                        else if (chance <= 65)
                        {
                            meleeMod = 0.04;
                        }
                        else if (chance <= 85)
                        {
                            meleeMod = 0.05;
                        }
                        else if (chance <= 95)
                        {
                            meleeMod = 0.06;
                        }
                        else
                        {
                            meleeMod = 0.07;
                        }

                        break;
                    case 4:
                        if (chance <= 20)
                        {
                            meleeMod = 0.04;
                        }
                        else if (chance <= 45)
                        {
                            meleeMod = 0.05;
                        }
                        else if (chance <= 65)
                        {
                            meleeMod = 0.06;
                        }
                        else if (chance <= 85)
                        {
                            meleeMod = 0.07;
                        }
                        else if (chance <= 95)
                        {
                            meleeMod = 0.08;
                        }
                        else
                        {
                            meleeMod = 0.09;
                        }

                        break;
                    case 5:
                        if (chance <= 20)
                        {
                            meleeMod = 0.06;
                        }
                        else if (chance <= 45)
                        {
                            meleeMod = 0.07;
                        }
                        else if (chance <= 65)
                        {
                            meleeMod = 0.08;
                        }
                        else if (chance <= 85)
                        {
                            meleeMod = 0.09;
                        }
                        else if (chance <= 95)
                        {
                            meleeMod = 0.10;
                        }
                        else if (chance <= 98)
                        {
                            meleeMod = 0.11;
                        }
                        else
                        {
                            meleeMod = 0.12;
                        }

                        break;
                    case 6:
                        if (chance <= 20)
                        {
                            meleeMod = 0.08;
                        }
                        else if (chance <= 20)
                        {
                            meleeMod = 0.09;
                        }
                        else if (chance <= 40)
                        {
                            meleeMod = 0.10;
                        }
                        else if (chance <= 60)
                        {
                            meleeMod = 0.11;
                        }
                        else if (chance <= 75)
                        {
                            meleeMod = 0.12;
                        }
                        else if (chance <= 89)
                        {
                            meleeMod = 0.13;
                        }
                        else if (chance <= 98)
                        {
                            meleeMod = 0.14;
                        }
                        else
                        {
                            meleeMod = 0.15;
                        }

                        break;
                    default:
                        meleeMod = 0.05;
                        break;
                }
                break;
            case 250: // Missile
                if (chance <= 20)
                {
                    meleeMod = 0.01;
                }
                else if (chance <= 45)
                {
                    meleeMod = 0.02;
                }
                else if (chance <= 65)
                {
                    meleeMod = 0.03;
                }
                else if (chance <= 85)
                {
                    meleeMod = 0.04;
                }
                else if (chance <= 95)
                {
                    meleeMod = 0.05;
                }
                else
                {
                    meleeMod = 0.06;
                }

                break;
            case 270: // Missile
                if (chance <= 10)
                {
                    meleeMod = 0.04;
                }
                else if (chance <= 20)
                {
                    meleeMod = 0.05;
                }
                else if (chance <= 30)
                {
                    meleeMod = 0.06;
                }
                else if (chance <= 45)
                {
                    meleeMod = 0.07;
                }
                else if (chance <= 55)
                {
                    meleeMod = 0.08;
                }
                else if (chance <= 70)
                {
                    meleeMod = 0.09;
                }
                else if (chance <= 85)
                {
                    meleeMod = 0.10;
                }
                else if (chance <= 95)
                {
                    meleeMod = 0.11;
                }
                else
                {
                    meleeMod = 0.12;
                }

                break;
            case 290: // Missile & Casters
            case 310: // Casters
                if (chance <= 10)
                {
                    meleeMod = 0.08;
                }
                else if (chance <= 20)
                {
                    meleeMod = 0.09;
                }
                else if (chance <= 40)
                {
                    meleeMod = 0.10;
                }
                else if (chance <= 60)
                {
                    meleeMod = 0.11;
                }
                else if (chance <= 80)
                {
                    meleeMod = 0.12;
                }
                else if (chance <= 95)
                {
                    meleeMod = 0.13;
                }
                else
                {
                    meleeMod = 0.14;
                }

                break;
            case 315: // Missile
            case 330: // Casters
            case 335: // Missile
                if (chance <= 10)
                {
                    meleeMod = 0.09;
                }
                else if (chance <= 20)
                {
                    meleeMod = 0.10;
                }
                else if (chance <= 40)
                {
                    meleeMod = 0.11;
                }
                else if (chance <= 60)
                {
                    meleeMod = 0.12;
                }
                else if (chance <= 80)
                {
                    meleeMod = 0.13;
                }
                else if (chance <= 95)
                {
                    meleeMod = 0.14;
                }
                else
                {
                    meleeMod = 0.15;
                }

                break;
            case 150: // No wield Casters
            case 355: // Casters
            case 360: // Missile
                if (chance <= 15)
                {
                    meleeMod = 0.12;
                }
                else if (chance <= 30)
                {
                    meleeMod = 0.13;
                }
                else if (chance <= 45)
                {
                    meleeMod = 0.14;
                }
                else if (chance <= 65)
                {
                    meleeMod = 0.15;
                }
                else if (chance <= 80)
                {
                    meleeMod = 0.16;
                }
                else if (chance <= 95)
                {
                    meleeMod = 0.17;
                }
                else
                {
                    meleeMod = 0.18;
                }

                break;
            case 180: // No wield Casters
            case 375: // Missile/Caster
            case 385: // Missile/Caster
                if (chance <= 10)
                {
                    meleeMod = 0.13;
                }
                else if (chance <= 25)
                {
                    meleeMod = 0.14;
                }
                else if (chance <= 45)
                {
                    meleeMod = 0.15;
                }
                else if (chance <= 65)
                {
                    meleeMod = 0.16;
                }
                else if (chance <= 80)
                {
                    meleeMod = 0.17;
                }
                else if (chance <= 90)
                {
                    meleeMod = 0.18;
                }
                else if (chance <= 98)
                {
                    meleeMod = 0.19;
                }
                else
                {
                    meleeMod = 0.20;
                }

                break;
            default:
                meleeMod = 0.05;
                break;
        }
        meleeMod += 1.0;
        return meleeMod;
    }

    /// <summary>
    /// Returns Values for Magic & Missile Defense Bonus. Updated HarliQ 11/17/19
    /// </summary>
    private static double RollWeapon_MissileMagicDefense(int tier)
    {
        double magicMissileDefenseMod = 0;
        // For seeing if weapon even gets a chance at a modifier
        var modifierChance = ThreadSafeRandom.Next(1, 2);
        if (modifierChance > 1)
        {
            switch (tier)
            {
                case 1:
                case 2:
                    magicMissileDefenseMod = 0;
                    break;
                case 3:
                    var chance = ThreadSafeRandom.Next(1, 100);
                    if (chance > 95)
                    {
                        magicMissileDefenseMod = .005;
                    }

                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(1, 100);
                    if (chance > 95)
                    {
                        magicMissileDefenseMod = .01;
                    }
                    else if (chance > 80)
                    {
                        magicMissileDefenseMod = .005;
                    }
                    else
                    {
                        magicMissileDefenseMod = 0;
                    }

                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 950)
                    {
                        magicMissileDefenseMod = .01;
                    }
                    else if (chance > 800)
                    {
                        magicMissileDefenseMod = .005;
                    }
                    else
                    {
                        magicMissileDefenseMod = 0;
                    }

                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 975)
                    {
                        magicMissileDefenseMod = .020;
                    }
                    else if (chance > 900)
                    {
                        magicMissileDefenseMod = .015;
                    }
                    else if (chance > 800)
                    {
                        magicMissileDefenseMod = .010;
                    }
                    else if (chance > 700)
                    {
                        magicMissileDefenseMod = .005;
                    }
                    else
                    {
                        magicMissileDefenseMod = 0;
                    }

                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 990)
                    {
                        magicMissileDefenseMod = .030;
                    }
                    else if (chance > 985)
                    {
                        magicMissileDefenseMod = .025;
                    }
                    else if (chance > 950)
                    {
                        magicMissileDefenseMod = .020;
                    }
                    else if (chance > 900)
                    {
                        magicMissileDefenseMod = .015;
                    }
                    else if (chance > 850)
                    {
                        magicMissileDefenseMod = .01;
                    }
                    else if (chance > 800)
                    {
                        magicMissileDefenseMod = .005;
                    }
                    else
                    {
                        magicMissileDefenseMod = 0;
                    }

                    break;
                default: // tier 8
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 998)
                    {
                        magicMissileDefenseMod = .04;
                    }
                    else if (chance > 994)
                    {
                        magicMissileDefenseMod = .035;
                    }
                    else if (chance > 990)
                    {
                        magicMissileDefenseMod = .03;
                    }
                    else if (chance > 985)
                    {
                        magicMissileDefenseMod = .025;
                    }
                    else if (chance > 950)
                    {
                        magicMissileDefenseMod = .02;
                    }
                    else if (chance > 900)
                    {
                        magicMissileDefenseMod = .015;
                    }
                    else if (chance > 850)
                    {
                        magicMissileDefenseMod = .01;
                    }
                    else if (chance > 800)
                    {
                        magicMissileDefenseMod = .005;
                    }
                    else
                    {
                        magicMissileDefenseMod = 0;
                    }

                    break;
            }
        }
        var modifier = 1.0 + magicMissileDefenseMod;

        return modifier;
    }
}
