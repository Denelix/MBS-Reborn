using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using MBS_Reborn.Debugger;

namespace MBS_Reborn.Character
{
    public class Characters
    {
        public string name { get; set; } = "empty";
        public int baseMana { get; set; } = 0;
        public int scalingMana { get; set; }
        public int manaCost { get; set; }
        //If tose are  is zero please add early game strength. ^^
        public double baseHealth { get; set; }
        public double bonusHealth { get; set; }
        public double scalingHealth { get; set; }
		public double healthRegen { get; set; }
        public double bonusMana { get; set; }
        public double manaRegen { get; set; }
        public string damageType { get; set; }
        public double baseAttackDamage { get; set; }
        public double baseAttackRange { get; set; }
        public double bonusAttackRange { get; set; }
        public double bonusAttackDamage { get; set; }
        public double baseDamage { get; set; }
        public double bonusDamage { get; set; } //DONT FORGET ABOUT THIS WHEN STUFF!!!
        public double scalingDamage { get; set; }
        public double scalingAttackDamage { get; set; }
        public double attackSpeed { get; set; }
        public double attackSpeedScaling { get; set; }
        public double attackSpeedMulti { get; set; }
        public double damageCooldown { get; set; }
        public double damageRange { get; set; }
        public double physicalResistance { get; set; }
        public double physicalResistanceScaling { get; set; }
        public double specialResistance { get; set; }
        public double specialResistanceScaling { get; set; }
        public double movementSpeed { get; set; }
        public double abilityPower { get; set; }
        public double cooldownReduction { get; set; }
        public double criticalStrikeChance { get; set; }
        public double criticalStrikeDamage { get; set; }
        public double lifeSteal { get; set; }
        public double spellVamp { get; set; }
        public double selfShield { get; set; }
        public double allyShield { get; set; }
        public double allyHeal { get; set; }
        public double allyDamageBoost { get; set; }
        public double evasion { get; set; }
        public double unevadable { get; set; }
        public double mobility { get; set; } //1 per dash or movementspeed buff in kit. * mobility ammount (units a second~ instant would be 2000) synergizes with cooldown
        public double clearSpeed { get; set; }
        public double teamfight { get; set; }
        public double dueling { get; set; }
        public double healingReduction { get; set; }
        public double specialPenetration { get; set; }
        public double physicalPenetration { get; set; }
        public double tenacity { get; set; }
        public double objective { get; set; }
        public double waveClear { get; set; }
        public double cc { get; set; }
        public int vex { get; set; }
        public int intrest { get; set; }
        public double ace { get; set; }
        public bool canTop { get; set; } = false;
        public bool canJg { get; set; } = false;
        public bool canMid { get; set; } = false;
        public bool canAdc { get; set; } = false;
        public bool canSup { get; set; } = false;
        public int wins { get; set; }

        public double initPickRate(Stats stats)
        {
            //Only Base
            var Sustain = baseHealth * (healthRegen) + baseMana * (manaRegen);
            Sustain -= manaCost * (16 - damageCooldown);
            if (baseMana == 0) { Sustain = (baseHealth * (healthRegen)) * 2; }
            //late game stuff
            Sustain += allyShield * 3;
            Sustain += selfShield * 3;
            Sustain += allyHeal * 3;
            Sustain *= 1 + spellVamp*((baseDamage+bonusDamage)/200);
            Sustain *= 1 + (lifeSteal+attackSpeed)*(baseAttackDamage+bonusAttackDamage/2);
            Sustain *= 1 + tenacity * ((baseHealth+bonusHealth)/ 200);
            //Debug.Log(Sustain);
            //Damage can include late game stuff
            var Damage = 100 * baseAttackDamage * (attackSpeed);
            Damage += baseDamage * (16 - damageCooldown);
            Damage += allyDamageBoost * 4;
            Damage += (criticalStrikeChance * criticalStrikeDamage) * (baseAttackDamage + bonusAttackDamage*4); //crit chance only
            Damage += (baseAttackDamage + bonusAttackDamage) * ((baseAttackRange + bonusAttackRange + damageRange) / 125);
            Damage += abilityPower;
            Damage += bonusDamage;
            Damage *= (1 + ((specialPenetration + physicalPenetration) * .6));
            /*Debug.Log(Damage);*/
            //Safety and Mobility late game stuff
            var Mobility = (mobility + evasion) * 5;
            Mobility += Damage * (damageRange - 125) * .025;
            Mobility += (50 - clearSpeed) * 48;
            Mobility += cc * 250;
            Mobility += ace * 250;
            Mobility += teamfight * 250;
            //Debug.Log("===" + name + "===");
            //Stat reaction
            var totalStats = Damage + Sustain + Mobility;
            //Debug.Log("Damage: " + Damage + " | Sustain: " + Sustain + " | Mobility: " + Mobility + " | totalStats: " + totalStats);
            //Debug.Log("Added all: " + totalStats);
            totalStats *= 1 + ((damageRange - 124) * .025);
            //Debug.Log("Damage range counting: " + totalStats);
            totalStats *= (movementSpeed - 324) * 1.1;
            //Debug.Log("Movementspeed multiplier: " + totalStats);
            totalStats += (300000 * (-1 + (1 + stats.wins / 1 + stats.loses)));
            totalStats += (300000 * (-1 + (1 + stats.bans / 1 + stats.matches)));
            //Debug.Log("Winrate = more picks: " + totalStats);
            totalStats *= (1 + stats.wins / 1 + stats.loses);//Higher winrate = they want to play more.
            //Debug.Log("Winrate = more picks 2: " + totalStats);
            totalStats *= (1 + (intrest * .002));
            //THis is for characters that have interesting design and/or lore and has nothing to do with the kit.
            //Debug.Log("Intrest: " + totalStats);
            if (totalStats < 100000) { totalStats = 100000; }
            //Debug.Log("total stats:" + totalStats);
            return totalStats;
        }

        public double initBanRate(Stats stats)
        {
            var Sustain = 0.0;
            //late game stuff
            Sustain += allyShield * 3;
            Sustain += selfShield * 3;
            Sustain += allyHeal * 3;
            Sustain *= 1 + spellVamp * ((baseDamage + bonusDamage) / 150);
            Sustain *= 1 + (lifeSteal + attackSpeed) * (baseAttackDamage + bonusAttackDamage / 1.75);
            Sustain *= 1 + tenacity * ((baseHealth + bonusHealth) / 300);
            Sustain *= healthRegen/4;
            //Debug.Log(Sustain);
            //Damage can include late game stuff
            var Damage = 100 * baseAttackDamage * (attackSpeed);
            Damage += baseDamage * (14 - damageCooldown);
            Damage += allyDamageBoost * 3;
            Damage += (criticalStrikeChance * criticalStrikeDamage) * (baseAttackDamage + bonusAttackDamage * 4); //crit chance only
            Damage += (baseAttackDamage + bonusAttackDamage) * ((baseAttackRange + bonusAttackRange + damageRange) / 200);
            Damage += (abilityPower + damageRange)*10;
            Damage += bonusDamage;
            Damage *= (1+((specialPenetration + physicalPenetration) * .77));
            Damage += (healingReduction * 70000)+1;
            //Debug.Log(Damage);
            //Safety and Mobility late game stuff
            var Mobility = (mobility + evasion + unevadable + movementSpeed*4 + cooldownReduction) * 9;
            Mobility += Damage * (damageRange - 125) * .0325;
            Mobility += (50 - clearSpeed) * 64;
            Mobility += cc * 325;
            Mobility += ace * 275;
            Mobility += teamfight * 300;
            //Debug.Log("===" + name + "===");
            //Stat reaction
            var totalStats = Damage + Sustain + Mobility;
            //Debug.Log("Damage: " + Damage + " | Sustain: " + Sustain + " | Mobility: " + Mobility + " | totalStats: " + totalStats);
            //Debug.Log("Added all: " + totalStats);
            totalStats *= 1 + ((damageRange - 124) * .025);
            //Debug.Log("Damage range counting: " + totalStats);
            totalStats *= 1+(movementSpeed - 324) * 1.5;
            //Debug.Log("Movementspeed multiplier: " + totalStats);
            totalStats += (650000 * (-1 + (1 + stats.picks / 1 + stats.matches)));//negative is here just to make sure it's .74 instead of like 1.33 or smthn
            //Debug.Log("pciks = more bans: " + totalStats);
            totalStats *= (1 + stats.picks / 1 + stats.matches);//Higher pickrate = they want to ban more.
            //Debug.Log("wins = more bans 2: " + totalStats);
            totalStats *= (1 + stats.wins / 1 + stats.loses);
            totalStats *= (1 + (vex * .004));
            //THis is for characters that have interesting design and/or lore and has nothing to do with the kit.
            //Debug.Log("Intrest: " + totalStats);
            if (totalStats < 100000) { totalStats = 100000; }
            //Debug.Log("total stats:" + totalStats);
            return totalStats;
        }
        public static List<Characters> FromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Characters>>(json);
        }
        //After every win. record and create files for the character who won against who.

        //Check for top 33% of champions for a role.
        public static Tuple<Characters, double> Top(Characters c)
        {
            var manaless = 0;
            if (c.baseMana == 0) { manaless = 1; }

            double score =
                ((c.baseDamage * c.scalingDamage * c.scalingDamage)-(325/c.baseAttackRange)*300) * c.damageCooldown + //Damage
                ((c.baseHealth + c.scalingHealth * 17) * 10 * (c.healthRegen * 1.4)) +// Health
                ((c.baseMana + c.scalingMana * 17) * 12 * (c.manaRegen * 1.4)) +// Mana
                (manaless * 10000000) +
                (c.waveClear * 1406000) +
                ((100 - c.clearSpeed) * 1000000) +
                (c.tenacity * 25000000) +
                (c.lifeSteal * 32000000) +
                (c.spellVamp * 26000000) +
                ((c.movementSpeed - 324) * 950000) +
                (c.physicalResistance * (c.physicalResistanceScaling * 17) * 100000) +
                (c.specialResistance * (c.specialResistanceScaling * 17) * 70000) +
                (c.mobility * 120000) +
                (c.selfShield * 120000) +
                (c.evasion * 120000) +
                (c.dueling * 140000) +
                (c.cc * 1500000) +
                +1;
            return Tuple.Create(c, score);
        }
        public static Tuple<Characters, double> Jg(Characters c)
        {
            var manaless = 0;
            if (c.baseMana == 0) { manaless = 1; }

            double score =
                (c.baseDamage * c.scalingDamage * c.scalingDamage)*c.damageCooldown + //Damage
                ((c.baseHealth + c.scalingHealth*14) * 6 * (c.healthRegen * .4)) +// Health
                ((c.baseMana + c.scalingMana * 14) * 6 * (c.manaRegen * .4)) +// Mana
                (manaless * 60000000) +
                (c.waveClear * 112000) +
                ((100 - c.clearSpeed) * 5400000) +
                (c.lifeSteal * 135000000) +
                (c.spellVamp * 135000000) +
                ((c.movementSpeed - 324) * 1025000) +
                (c.physicalResistance * (c.physicalResistanceScaling * 17) * 90000) +
                (c.specialResistance * (c.specialResistanceScaling * 17) * 30000) +
                (c.mobility * 160000) +
                (c.objective * 2000000) +
                (c.unevadable * 275000) +
                (c.evasion * 300000) +
                (c.selfShield * 160000) +
                (c.dueling * 120000) +
                (c.cc * 1800000) +
                +1;
            return Tuple.Create(c, score);
        }
        public static Tuple<Characters, double> Mid(Characters c)
        {
            var manaless = 0;
            if (c.baseMana == 0) { manaless = 1; }

            double score =
                ((c.baseDamage * c.scalingDamage * c.scalingAttackDamage) * (c.damageRange/275)) * c.damageCooldown + //Damage
                ((c.baseHealth + c.scalingHealth * 17) * 10 * (c.healthRegen * 1.4)) +// Health
                ((c.baseMana + c.scalingMana * 17) * 10 * (c.manaRegen * 1.2)) +// Mana
                (manaless * 100000000) +
                (c.waveClear * 2360000) +
                (c.tenacity * 180000000) +
                (c.lifeSteal * 110000000) +
                (c.spellVamp * 180000000) +
                ((c.movementSpeed - 324) * 1000000) +
                (c.physicalResistance * (c.physicalResistanceScaling * 17) * 31000) +
                (c.specialResistance * (c.specialResistanceScaling * 17) * 62000) +
                (c.mobility * 160000) +
                (c.objective * 2600000) +
                (c.selfShield * 120000) +
                (c.unevadable * 220000) +
                (c.evasion * 140000) +
                (c.dueling * 90000) +
                (c.cc * 12000) +
                +1;
            return new Tuple<Characters, double>(c, score);
        }
        public static Tuple<Characters, double> ADC(Characters c)
        {
            var manaless = 0;
            if (c.baseMana == 0) { manaless = 1; }

            double score =
                ((c.baseDamage * (c.scalingDamage / 9) * (c.scalingAttackDamage*2)) * (c.baseAttackDamage * c.scalingAttackDamage) * (c.damageRange / 200) * (c.baseAttackRange / 125)) * c.damageCooldown + //Damage
                ((c.baseHealth + c.scalingHealth * 17) * 17 * (c.healthRegen * .8)) +// Health
                ((c.baseMana + c.scalingMana * 17) * 17 * (c.manaRegen * 1.1)) +// Mana
                (manaless * 7000000) +
                (c.waveClear * 100000) +
                (c.lifeSteal * 1700000) +
                (c.spellVamp * 1000000) +
                (c.physicalResistance * (c.physicalResistanceScaling * 17) * 70000) +
                (c.specialResistance * (c.specialResistanceScaling * 17) * 15000) +
                (c.mobility * 100000) +
                (c.objective * 8000000) +
                (c.selfShield * 160000) +
                (c.unevadable * 160000) +
                (c.evasion * 240000) +
                (c.teamfight * 500000) +
                (c.attackSpeed * 120000000 * (1+c.attackSpeedScaling)) +
                (c.damageRange * 12000) +
                +1;
            if (c.baseAttackRange < 475) { score *= .8; }
            if (c.damageRange < 475) { score *= .8; }
            return new Tuple<Characters, double>(c, score);
        }
        public static Tuple<Characters, double> Sup(Characters c)
        {
            var manaless = 0;
            if (c.baseMana == 0) { manaless = 1; }

            double score =
                ((c.baseDamage * c.scalingDamage * c.scalingDamage/1.6) * c.damageCooldown + //Damage
                ((c.baseHealth + c.scalingHealth * 12) * 10 * (c.healthRegen * 1.1)) +// Health
                ((c.baseMana + c.scalingMana * 12) * 12 * (c.manaRegen * 2)) +// Mana
                (manaless * 7000000) +
                (c.waveClear * 400000) +
                (c.tenacity * 15000000) +
                (c.spellVamp * 105000000) +
                (c.physicalResistance * (c.physicalResistanceScaling * 17) * 70000) +
                (c.specialResistance * (c.specialResistanceScaling * 17) * 15000) +
                ((c.movementSpeed - 324) * 950000) +
                (c.mobility * 100000) +
                (c.objective * 1400000) +
                (c.selfShield * 120000) +
                (c.allyShield * 260000) +
                (c.allyHeal * 600000) +
                (c.unevadable * 157000) +
                (c.evasion * 1070000) +
                (c.teamfight * 120000) +
                (c.cc * 7200000) +
                +1);
            if (c.damageRange < 360 && c.cc < 20) 
            { score *= .75; }
            return Tuple.Create(c, score);
        }
        public override string ToString()
        {
            return $"Character: {name}";
        }
    }
}
