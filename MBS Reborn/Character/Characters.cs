using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using MBS_Reborn.Debugger;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;

namespace MBS_Reborn.Character
{
    public class Characters
    {

        public string Name { get; set; } = "empty";
        public int BaseMana { get; set; } = 0;
        public int ScalingMana { get; set; } = 0;
        public int ManaCost { get; set; } = 0;

        public double BaseHealth { get; set; }
        public double HealthRegen { get; set; }
        public double ScalingHealth { get; set; }

        public double BonusHealth { get; set; }
        public double BonusMana { get; set; }
        public double ManaRegen { get; set; }

        public double BaseAttackDamage { get; set; }
        public double BaseAttackRange { get; set; }
        public double BonusAttackRange { get; set; }
        public double BonusAttackDamage { get; set; }

        public double BaseDamage { get; set; }
        public double BonusDamage { get; set; }
        public double ScalingDamage { get; set; }
        public double ScalingAttackDamage { get; set; }

        public double AttackSpeed { get; set; }
        public double AttackSpeedScaling { get; set; }
        public double AttackSpeedMulti { get; set; }

        public double DamageCooldown { get; set; }
        public double DamageRange { get; set; }

        public double PhysicalResistance { get; set; }
        public double PhysicalResistanceScaling { get; set; }

        public double SpecialResistance { get; set; }
        public double SpecialResistanceScaling { get; set; }

        public double MovementSpeed { get; set; }

        public double AbilityPower { get; set; }
        public double CooldownReduction { get; set; }

        public double CriticalStrikeChance { get; set; }
        public double CriticalStrikeDamage { get; set; }

        public double LifeSteal { get; set; }
        public double SpellVamp { get; set; }

        public double SelfShield { get; set; }
        public double AllyShield { get; set; }
        public double AllyHeal { get; set; }
        public double AllyDamageBoost { get; set; }

        public double Evasion { get; set; }
        public double Unevadable { get; set; }

        public double Mobility { get; set; }
        public double ClearSpeed { get; set; }
        public double Teamfight { get; set; }
        public double Dueling { get; set; }

        public double HealingReduction { get; set; }
        public double SpecialPenetration { get; set; }
        public double PhysicalPenetration { get; set; }

        public double Tenacity { get; set; }
        public double Objective { get; set; }
        public double WaveClear { get; set; }
        public int Vex { get; set; }
        public int Interest { get; set; }
        public double CC { get; set; }
        public double ace { get; set; }
        public bool canTop { get; set; } = false;
        public bool canJg { get; set; } = false;
        public bool canMid { get; set; } = false;
        public bool canAdc { get; set; } = false;
        public bool canSup { get; set; } = false;
        public int wins { get; set; }
        public string Ability1DamageType { get; set; } = "Adaptive";
        public string Ability2DamageType { get; set; } = "Adaptive";
        public string Ability3DamageType { get; set; } = "Special";

        public List<double> Ability1BaseDamage { get; set; } = new List<double> { };
        public List<double> Ability2BaseDamage { get; set; } = new List<double> { };
        public List<double> Ability3BaseDamage { get; set; } = new List<double> { };

        public List<double> Ability1ADScaling { get; set; } = new List<double> { };
        public List<double> Ability2ADScaling { get; set; } = new List<double> { };
        public List<double> Ability3ADScaling { get; set; } = new List<double> { };

        public List<double> Ability1APScaling { get; set; } = new List<double> { };
        public List<double> Ability2APScaling { get; set; } = new List<double> { };
        public List<double> Ability3APScaling { get; set; } = new List<double> { };

        public List<double> Ability1DamageCost { get; set; } = new List<double> { };
        public List<double> Ability2DamageCost { get; set; } = new List<double> { };
        public List<double> Ability3DamageCost { get; set; } = new List<double> { };

        public List<double> Ability1DamageRange { get; set; } = new List<double> { };
        public List<double> Ability2DamageRange { get; set; } = new List<double> { };
        public List<double> Ability3DamageRange { get; set; } = new List<double> { };

        public double Ability1DamageAoe { get; set; } = 100;
        public double Ability2DamageAoe { get; set; } = 100;
        public double Ability3DamageAoe { get; set; } = 100;

        public List<double> Ability1DamageHitChance { get; set; } = new List<double> { };
        public List<double> Ability2DamageHitChance { get; set; } = new List<double> { };
        public List<double> Ability3DamageHitChance { get; set; } = new List<double> { };

        public List<double> Ability1DashRange { get; set; } = new List<double> { };
        public List<double> Ability2DashRange { get; set; } = new List<double> { };
        public List<double> Ability3DashRange { get; set; } = new List<double> { };

        public List<double> UltDashRange { get; set; } = new List<double> { };

        public List<double> Ability1Cooldown { get; set; } = new List<double> { };
        public List<double> Ability2Cooldown { get; set; } = new List<double> { };
        public List<double> Ability3Cooldown { get; set; } = new List<double> { };

        public List<double> UltCooldown { get; set; } = new List<double> { };

        public string UltDamageType { get; set; } = "Adaptive";
        public List<double> UltBaseDamage { get; set; } = new List<double> { };
        public List<double> UltADScaling { get; set; } = new List<double> { };
        public List<double> UltAPScaling { get; set; } = new List<double> { };

        public List<double> UltDamageCost { get; set; } = new List<double> { };
        public List<double> UltDamageRange { get; set; }
        public double UltDamageAoe { get; set; } = 100;
        public List<double> UltDamageHitChance { get; set; } = new List<double> { };
        public Ability Ability1 { get; set; }
public Ability Ability2 { get; set; }
public Ability Ability3 { get; set; }
public Ability Ultimate { get; set; }

        // Abilities as objects (optional)
        public class Ability
        {
            public List<double>Cooldown { get; set; } = new List<double>();
            public string DamageType { get; set; } = "None";
            public List<int> BaseDamage { get; set; } = new List<int>();
            public List<int> ADScaling { get; set; } = new List<int>();
            public List<int> APScaling { get; set; } = new List<int>();
            public List<int> DamageCost { get; set; } = new List<int>();
            public List<int> DamageRange { get; set; } = new List<int>();
            public int DamageAoe { get; set; } = 0;
            public List<int> DamageHitChance { get; set; } = new List<int>();
            public List<int> DashRange { get; set; } = new List<int>();
            public float SelfHeal { get; set; } = 0;
            public float SelfShield { get; set; } = 0;
            public float AllyShield { get; set; } = 0;
            public float AllyHeal { get; set; } = 0;
            public bool AllyAoe { get; set; } = false;
            public short Point { get; set; } = -1;
            public override string ToString()
            {
                return $@"
                    Cooldown: [{string.Join(", ", Cooldown)}]
                    Damage Type: {DamageType}
                    Base Damage: [{string.Join(", ", BaseDamage)}]
                    AD Scaling: [{string.Join(", ", ADScaling)}]
                    AP Scaling: [{string.Join(", ", APScaling)}]
                    Damage Cost: [{string.Join(", ", DamageCost)}]
                    Damage Range: [{string.Join(", ", DamageRange)}]
                    Damage AoE: {DamageAoe}
                    Damage Hit Chance: [{string.Join(", ", DamageHitChance)}]
                    Dash Range: [{string.Join(", ", DashRange)}]
                    Self Heal: {SelfHeal}
                    Self Shield: {SelfShield}
                    Ally Shield: {AllyShield}
                    Ally Heal: {AllyHeal}
                    Ally AoE: {AllyAoe}
                    Points: {Point}";
            }

        }
        public static List<Characters> SetAbilities(List<Characters> characters)
        {
            foreach (var character in characters)
            {
                // Set Ability 1
                character.Ability1 = new Ability
                {
                    Cooldown = character.Ability1Cooldown,  // Using character.<property>
                    DamageType = character.Ability1DamageType,
                    BaseDamage = character.Ability1BaseDamage.Select(d => (int)d).ToList(),
                    ADScaling = character.Ability1ADScaling.Select(d => (int)d).ToList(),
                    APScaling = character.Ability1APScaling.Select(d => (int)d).ToList(),
                    DamageCost = character.Ability1DamageCost.Select(d => (int)d).ToList(),
                    DamageRange = character.Ability1DamageRange.Select(d => (int)d).ToList(),
                    DamageAoe = (int)character.Ability1DamageAoe,
                    DamageHitChance = character.Ability1DamageHitChance.Select(d => (int)d).ToList(),
                    DashRange = character.Ability1DashRange.Select(d => (int)d).ToList()
                };
                Console.WriteLine(character.Ability1);

                // Set Ability 2
                character.Ability2 = new Ability
                {
                    Cooldown = character.Ability2Cooldown,  // Using character.<property>
                    DamageType = character.Ability2DamageType,
                    BaseDamage = character.Ability2BaseDamage.Select(d => (int)d).ToList(),
                    ADScaling = character.Ability2ADScaling.Select(d => (int)d).ToList(),
                    APScaling = character.Ability2APScaling.Select(d => (int)d).ToList(),
                    DamageCost = character.Ability2DamageCost.Select(d => (int)d).ToList(),
                    DamageRange = character.Ability2DamageRange.Select(d => (int)d).ToList(),
                    DamageAoe = (int)character.Ability2DamageAoe,
                    DamageHitChance = character.Ability2DamageHitChance.Select(d => (int)d).ToList(),
                    DashRange = character.Ability2DashRange.Select(d => (int)d).ToList()
                };
                Console.WriteLine(character.Ability2);

                // Set Ability 3
                character.Ability3 = new Ability
                {
                    Cooldown = character.Ability3Cooldown,  // Using character.<property>
                    DamageType = character.Ability3DamageType,
                    BaseDamage = character.Ability3BaseDamage.Select(d => (int)d).ToList(),
                    ADScaling = character.Ability3ADScaling.Select(d => (int)d).ToList(),
                    APScaling = character.Ability3APScaling.Select(d => (int)d).ToList(),
                    DamageCost = character.Ability3DamageCost.Select(d => (int)d).ToList(),
                    DamageRange = character.Ability3DamageRange.Select(d => (int)d).ToList(),
                    DamageAoe = (int)character.Ability3DamageAoe,
                    DamageHitChance = character.Ability3DamageHitChance.Select(d => (int)d).ToList(),
                    DashRange = character.Ability3DashRange.Select(d => (int)d).ToList()
                };
                Console.WriteLine(character.Ability3);

                // Set Ultimate Ability
                character.Ultimate = new Ability
                {
                    Cooldown = character.UltCooldown,  // Using character.<property>
                    DamageType = character.UltDamageType,
                    BaseDamage = character.UltBaseDamage.Select(d => (int)d).ToList(),
                    ADScaling = character.UltADScaling.Select(d => (int)d).ToList(),
                    APScaling = character.UltAPScaling.Select(d => (int)d).ToList(),
                    DamageCost = character.UltDamageCost.Select(d => (int)d).ToList(),
                    DamageRange = character.UltDamageRange.Select(d => (int)d).ToList(),
                    DamageAoe = (int)character.UltDamageAoe,
                    DamageHitChance = character.UltDamageHitChance.Select(d => (int)d).ToList(),
                    DashRange = character.UltDashRange.Select(d => (int)d).ToList()
                };
                Console.WriteLine(character.Ultimate);
            }
            return characters;
        }
        public double InitPickRate(Stats stats)
        {
            // Only Base
            double sustain = BaseHealth * HealthRegen + BaseMana * ManaRegen;
            sustain -= ManaCost * (16 - DamageCooldown);
            if (BaseMana == 0)
            {
                sustain = (BaseHealth * HealthRegen) * 2;
            }

            // Late game stuff
            sustain += AllyShield * 3;
            sustain += SelfShield * 3;
            sustain += AllyHeal * 3;
            sustain *= 1 + SpellVamp * ((BaseDamage + BonusDamage) / 200);
            sustain *= 1 + (LifeSteal + AttackSpeed) * (BaseAttackDamage + BonusAttackDamage / 2);
            sustain *= 1 + Tenacity * ((BaseHealth + BonusHealth) / 200);

            // Damage can include late game stuff
            double damage = 100 * BaseAttackDamage * AttackSpeed;
            damage += BaseDamage * (16 - DamageCooldown);
            damage += AllyDamageBoost * 4;
            damage += (CriticalStrikeChance * CriticalStrikeDamage) * (BaseAttackDamage + BonusAttackDamage * 4); // crit chance only
            damage += (BaseAttackDamage + BonusAttackDamage) * ((BaseAttackRange + BonusAttackRange + DamageRange) / 125);
            damage += AbilityPower;
            damage += BonusDamage;
            damage *= (1 + ((SpecialPenetration + PhysicalPenetration) * 0.6));

            // Safety and Mobility late game stuff
            double mobilityScore = (Mobility + Evasion) * 5;
            mobilityScore += damage * (DamageRange - 125) * 0.025;
            mobilityScore += (50 - ClearSpeed) * 48;
            mobilityScore += CC * 250;
            mobilityScore += ace * 250;
            mobilityScore += Teamfight * 250;

            // Stat reaction
            double totalStats = damage + sustain + mobilityScore;

            totalStats *= 1 + ((DamageRange - 124) * 0.025);
            totalStats *= (MovementSpeed - 324) * 1.1;

            totalStats += (300000 * (-1 + (1 + stats.wins / 1 + stats.loses)));
            totalStats += (300000 * (-1 + (1 + stats.bans / 1 + stats.matches)));

            // Winrate = more picks
            totalStats *= (1 + stats.wins / 1 + stats.loses);
            totalStats *= (1 + (Interest * 0.002)); // Interest is misspelled as 'intrest' in the user code

            // This is for characters that have interesting design and/or lore and has nothing to do with the kit.
            if (totalStats < 100000)
            {
                totalStats = 100000;
            }

            return totalStats;
        }

        public double InitBanRate(Stats stats)
        {
            double sustain = 0.0;

            // Late game stuff
            sustain += AllyShield * 3;
            sustain += SelfShield * 3;
            sustain += AllyHeal * 3;
            sustain *= 1 + SpellVamp * ((BaseDamage + BonusDamage) / 150);
            sustain *= 1 + (LifeSteal + AttackSpeed) * (BaseAttackDamage + BonusAttackDamage / 1.75);
            sustain *= 1 + Tenacity * ((BaseHealth + BonusHealth) / 300);
            sustain *= HealthRegen / 4;

            // Damage can include late game stuff
            double damage = 100 * BaseAttackDamage * AttackSpeed;
            damage += BaseDamage * (14 - DamageCooldown);
            damage += AllyDamageBoost * 3;
            damage += (CriticalStrikeChance * CriticalStrikeDamage) * (BaseAttackDamage + BonusAttackDamage * 4); // crit chance only
            damage += (BaseAttackDamage + BonusAttackDamage) * ((BaseAttackRange + BonusAttackRange + DamageRange) / 200);
            damage += (AbilityPower + DamageRange) * 10;
            damage += BonusDamage;
            damage *= (1 + ((SpecialPenetration + PhysicalPenetration) * 0.77));
            damage += (HealingReduction * 70000) + 1;

            // Safety and Mobility late game stuff
            double mobilityScore = (Mobility + Evasion + Unevadable + MovementSpeed * 4 + CooldownReduction) * 9;
            mobilityScore += damage * (DamageRange - 125) * 0.0325;
            mobilityScore += (50 - ClearSpeed) * 64;
            mobilityScore += CC * 325;
            mobilityScore += ace * 275;
            mobilityScore += Teamfight * 300;

            // Stat reaction
            double totalStats = damage + sustain + mobilityScore;

            totalStats *= 1 + ((DamageRange - 124) * 0.025);
            totalStats *= 1 + (MovementSpeed - 324) * 1.5;

            totalStats += (650000 * (-1 + (1 + stats.picks / 1 + stats.matches)));
            totalStats *= (1 + stats.picks / 1 + stats.matches); // Higher pick rate = more bans
            totalStats *= (1 + stats.wins / 1 + stats.loses);
            totalStats *= (1 + (Vex * 0.004));

            // This is for characters that have interesting design and/or lore and has nothing to do with the kit.
            if (totalStats < 100000)
            {
                totalStats = 100000;
            }

            return totalStats;
        }

        // Role-specific Scoring Methods
        public static Tuple<Characters, double> Top(Characters c)
        {
            int manaless = c.BaseMana == 0 ? 1 : 0;

            double score =
                ((c.BaseDamage * c.ScalingDamage * c.ScalingDamage) - (325 / c.BaseAttackRange) * 300) * c.DamageCooldown + // Damage
                ((c.BaseHealth + c.ScalingHealth * 17) * 10 * (c.HealthRegen * 1.4)) + // Health
                ((c.BaseMana + c.ScalingMana * 17) * 12 * (c.ManaRegen * 1.4)) + // Mana
                (manaless * 10000000) +
                (c.WaveClear * 1406000) +
                ((100 - c.ClearSpeed) * 1000000) +
                (c.Tenacity * 25000000) +
                (c.LifeSteal * 32000000) +
                (c.SpellVamp * 26000000) +
                ((c.MovementSpeed - 324) * 950000) +
                (c.PhysicalResistance * (c.PhysicalResistanceScaling * 17) * 100000) +
                (c.SpecialResistance * (c.SpecialResistanceScaling * 17) * 70000) +
                (c.Mobility * 120000) +
                (c.SelfShield * 120000) +
                (c.Evasion * 120000) +
                (c.Dueling * 140000) +
                (c.CC * 1500000) +
                +1;

            return Tuple.Create(c, score);
        }

        public static Tuple<Characters, double> Jg(Characters c)
        {
            int manaless = c.BaseMana == 0 ? 1 : 0;

            double score =
                (c.BaseDamage * c.ScalingDamage * c.ScalingDamage) * c.DamageCooldown + // Damage
                ((c.BaseHealth + c.ScalingHealth * 14) * 6 * (c.HealthRegen * 0.4)) + // Health
                ((c.BaseMana + c.ScalingMana * 14) * 6 * (c.ManaRegen * 0.4)) + // Mana
                (manaless * 60000000) +
                (c.WaveClear * 112000) +
                ((100 - c.ClearSpeed) * 5400000) +
                (c.LifeSteal * 135000000) +
                (c.SpellVamp * 135000000) +
                ((c.MovementSpeed - 324) * 1025000) +
                (c.PhysicalResistance * (c.PhysicalResistanceScaling * 17) * 90000) +
                (c.SpecialResistance * (c.SpecialResistanceScaling * 17) * 30000) +
                (c.Mobility * 160000) +
                (c.Objective * 2000000) +
                (c.Unevadable * 275000) +
                (c.Evasion * 300000) +
                (c.SelfShield * 160000) +
                (c.Dueling * 120000) +
                (c.CC * 1800000) +
                +1;

            return Tuple.Create(c, score);
        }

        public static Tuple<Characters, double> Mid(Characters c)
        {
            int manaless = c.BaseMana == 0 ? 1 : 0;

            double score =
                ((c.BaseDamage * c.ScalingDamage * c.ScalingAttackDamage) * (c.DamageRange / 275)) * c.DamageCooldown + // Damage
                ((c.BaseHealth + c.ScalingHealth * 17) * 10 * (c.HealthRegen * 1.4)) + // Health
                ((c.BaseMana + c.ScalingMana * 17) * 10 * (c.ManaRegen * 1.2)) + // Mana
                (manaless * 100000000) +
                (c.WaveClear * 2360000) +
                (c.Tenacity * 180000000) +
                (c.LifeSteal * 110000000) +
                (c.SpellVamp * 180000000) +
                ((c.MovementSpeed - 324) * 1000000) +
                (c.PhysicalResistance * (c.PhysicalResistanceScaling * 17) * 31000) +
                (c.SpecialResistance * (c.SpecialResistanceScaling * 17) * 62000) +
                (c.Mobility * 160000) +
                (c.Objective * 2600000) +
                (c.SelfShield * 120000) +
                (c.Unevadable * 220000) +
                (c.Evasion * 140000) +
                (c.Dueling * 90000) +
                (c.CC * 12000) +
                +1;

            return new Tuple<Characters, double>(c, score);
        }

        public static Tuple<Characters, double> ADC(Characters c)
        {
            int manaless = c.BaseMana == 0 ? 1 : 0;

            double score =
                ((c.BaseDamage * (c.ScalingDamage / 9) * (c.ScalingAttackDamage * 2)) * (c.BaseAttackDamage * c.ScalingAttackDamage) * (c.DamageRange / 200) * (c.BaseAttackRange / 125)) * c.DamageCooldown + // Damage
                ((c.BaseHealth + c.ScalingHealth * 17) * 17 * (c.HealthRegen * 0.8)) + // Health
                ((c.BaseMana + c.ScalingMana * 17) * 17 * (c.ManaRegen * 1.1)) + // Mana
                (manaless * 7000000) +
                (c.WaveClear * 100000) +
                (c.LifeSteal * 1700000) +
                (c.SpellVamp * 1000000) +
                (c.PhysicalResistance * (c.PhysicalResistanceScaling * 17) * 70000) +
                (c.SpecialResistance * (c.SpecialResistanceScaling * 17) * 15000) +
                (c.Mobility * 100000) +
                (c.Objective * 8000000) +
                (c.SelfShield * 160000) +
                (c.Unevadable * 160000) +
                (c.Evasion * 240000) +
                (c.Teamfight * 500000) +
                (c.AttackSpeed * 120000000 * (1 + c.AttackSpeedScaling)) +
                (c.DamageRange * 12000) +
                +1;

            if (c.BaseAttackRange < 475)
            {
                score *= 0.8;
            }
            if (c.DamageRange < 475)
            {
                score *= 0.8;
            }

            return new Tuple<Characters, double>(c, score);
        }

        public static Tuple<Characters, double> Sup(Characters c)
        {
            int manaless = c.BaseMana == 0 ? 1 : 0;

            double score =
                ((c.BaseDamage * c.ScalingDamage * c.ScalingDamage / 1.6) * c.DamageCooldown + // Damage
                ((c.BaseHealth + c.ScalingHealth * 12) * 10 * (c.HealthRegen * 1.1)) + // Health
                ((c.BaseMana + c.ScalingMana * 12) * 12 * (c.ManaRegen * 2)) + // Mana
                (manaless * 7000000) +
                (c.WaveClear * 400000) +
                (c.Tenacity * 15000000) +
                (c.SpellVamp * 105000000) +
                (c.PhysicalResistance * (c.PhysicalResistanceScaling * 17) * 70000) +
                (c.SpecialResistance * (c.SpecialResistanceScaling * 17) * 15000) +
                ((c.MovementSpeed - 324) * 950000) +
                (c.Mobility * 100000) +
                (c.Objective * 1400000) +
                (c.SelfShield * 120000) +
                (c.AllyShield * 260000) +
                (c.AllyHeal * 600000) +
                (c.Unevadable * 157000) +
                (c.Evasion * 1070000) +
                (c.Teamfight * 120000) +
                (c.CC * 7200000) +
                +1);

            if (c.DamageRange < 360 && c.CC < 20)
            {
                score *= 0.75;
            }

            return Tuple.Create(c, score);
        }
        public class UltDamageRangeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(List<double>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
                {
                    return new List<double> { (double)reader.Value };
                }

                return serializer.Deserialize<List<double>>(reader);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        // ToString Override
        public override string ToString()
        {
            return $"Character: {Name}";
        }

        // Deserialization Method
        public static List<Characters> FromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var characters = JsonConvert.DeserializeObject<List<Characters>>(json);

            characters = SetAbilities(characters);

            return characters;
        }

    }
}
