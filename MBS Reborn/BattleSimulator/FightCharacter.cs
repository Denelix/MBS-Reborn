using System;
using System.Collections.Generic;

namespace MBS_Reborn.Character
{
    public class FightCharacter : Characters
    {
        // Additional Match-Specific Properties
        public List<string> Runes { get; set; } = new List<string>();
        public int Level { get; set; } = 1;
        public double Gold { get; set; } = 0.0;
        public double Exp { get; set; } = 0.0;
        public double LowBaseHealth { get; set; } = 0.0;
        public double BaseMovementSpeed { get; set; } = 0.0;
        public double bonusAttackSpeed { get; set; } = 0.0;
        public double totalAttackDamage { get; set; } = 0.0;
        public List<string> Items { get; set; } = new List<string> { "", "", "", "", "", "" }; // Initialize with 6 empty items
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public double Health { get; set; } = 0;
        public double Mana { get; set; } = 0;
        public double Shield { get; set; } = 0;
        public short ability1Points { get; set; } = 1;
        public short ability2Points { get; set; } = 1;
        public short ability3Points { get; set; } = 1;
        public short ability4Points { get; set; } = 1;
        public double ability1Cooldown { get; set; } = 1;
        public double ability2Cooldown { get; set; } = 1;
        public double ability3Cooldown { get; set; } = 1;
        public double ability4Cooldown { get; set; } = 1;
        public double AutoAttackCD { get; set; } = 0.0;
        public double Position { get; set; } = 0.0;
        public bool CanWalk { get; set; } = true;
        public bool CanAuto { get; set; } = true;
        public bool CanCast { get; set; } = true;
        public bool isDead { get; set; } = true;
        public List<Effect> Effects { get; set; } = new List<Effect>();

        // Constructor to copy base Characters properties
        public FightCharacter() : base()
        {
            // Default constructor
        }
        public FightCharacter(Characters character) : base()
        {
            Name = character.Name;
            BaseMana = character.BaseMana;
            ScalingMana = character.ScalingMana;
            ManaCost = character.ManaCost;
            BaseHealth = character.BaseHealth;
            HealthRegen = character.HealthRegen;
            ScalingHealth = character.ScalingHealth;
            BonusHealth = character.BonusHealth;
            BonusMana = character.BonusMana;
            ManaRegen = character.ManaRegen;
            BaseAttackDamage = character.BaseAttackDamage;
            BaseAttackRange = character.BaseAttackRange;
            BonusAttackRange = character.BonusAttackRange;
            BonusAttackDamage = character.BonusAttackDamage;
            BaseDamage = character.BaseDamage;
            BonusDamage = character.BonusDamage;
            ScalingDamage = character.ScalingDamage;
            ScalingAttackDamage = character.ScalingAttackDamage;
            AttackSpeed = character.AttackSpeed;
            AttackSpeedScaling = character.AttackSpeedScaling;
            AttackSpeedMulti = character.AttackSpeedMulti;
            DamageCooldown = character.DamageCooldown;
            DamageRange = character.DamageRange;
            PhysicalResistance = character.PhysicalResistance;
            PhysicalResistanceScaling = character.PhysicalResistanceScaling;
            SpecialResistance = character.SpecialResistance;
            SpecialResistanceScaling = character.SpecialResistanceScaling;
            MovementSpeed = character.MovementSpeed;
            AbilityPower = character.AbilityPower;
            CooldownReduction = character.CooldownReduction;
            CriticalStrikeChance = character.CriticalStrikeChance;
            CriticalStrikeDamage = character.CriticalStrikeDamage;
            LifeSteal = character.LifeSteal;
            SpellVamp = character.SpellVamp;
            SelfShield = character.SelfShield;
            AllyShield = character.AllyShield;
            AllyHeal = character.AllyHeal;
            AllyDamageBoost = character.AllyDamageBoost;
            Evasion = character.Evasion;
            Unevadable = character.Unevadable;
            Mobility = character.Mobility;
            ClearSpeed = character.ClearSpeed;
            Teamfight = character.Teamfight;
            Dueling = character.Dueling;
            HealingReduction = character.HealingReduction;
            SpecialPenetration = character.SpecialPenetration;
            PhysicalPenetration = character.PhysicalPenetration;
            Tenacity = character.Tenacity;
            Objective = character.Objective;
            WaveClear = character.WaveClear;
            Vex = character.Vex;
            Interest = character.Interest;
            CC = character.CC;
            ace = character.ace;
            canTop = character.canTop;
            canJg = character.canJg;
            canMid = character.canMid;
            canAdc = character.canAdc;
            canSup = character.canSup;
            wins = character.wins;
            Ability1 = character.Ability1; // Assuming shallow copy is fine
            Ability2 = character.Ability2;
            Ability3 = character.Ability3;
            Ultimate = character.Ultimate;
        }
        public void ApplyScaling()
        {
            // Example for Health scaling
            this.BaseHealth = this.LowBaseHealth + (this.ScalingHealth * this.Level)+this.BonusHealth;
            this.Mana = this.BaseMana + (this.ScalingMana * this.Level)+this.BonusMana;
            this.totalAttackDamage = this.ScalingAttackDamage * this.Level + this.BaseAttackDamage + this.BonusAttackDamage;
            this.AttackSpeed = this.AttackSpeedScaling * this.Level + this.bonusAttackSpeed;

        }
        public bool hasEffect(string Name)
        {
            foreach (Effect effect in this.Effects)
            {
                if (effect.EffectName == Name)
                    return true;
            }
            return false;
        }
    }
}