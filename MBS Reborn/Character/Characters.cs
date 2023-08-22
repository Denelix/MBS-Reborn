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
        public int baseMana { get; set; }
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
        public double damageCooldown { get; set; }
        public double damageRange { get; set; }
        public double physicalResistance { get; set; }
        public double specialResistance { get; set; }
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
        public int mobility { get; set; } //1 per dash or movementspeed buff in kit. * mobility ammount (units a second~ instant would be 2000) synergizes with cooldown
        public double clearSpeed { get; set; }
        public double teamfight { get; set; }
        public double healingReduction { get; set; }
        public double specialPenetration { get; set; }
        public double physicalPenetration { get; set; }
        public double tenacity { get; set; }
        public double objective { get; set; }
        public double waveClear { get; set; }
        public int cc { get; set; }
        public int vex { get; set; }
        public int intrest { get; set; }
        public int ace { get; set; }

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
            Debug.Log(Sustain);
            //Damage can include late game stuff
            var Damage = 100 * baseAttackDamage * (attackSpeed);
            Damage += baseDamage * (16 - damageCooldown);
            Damage += allyDamageBoost * 4;
            Damage += (criticalStrikeChance * criticalStrikeDamage) * (baseAttackDamage + bonusAttackDamage*4); //crit chance only
            Damage += (baseAttackDamage + bonusAttackDamage) * ((baseAttackRange + bonusAttackRange + damageRange) / 125);
            Damage += abilityPower;
            Damage += bonusDamage;
            Damage *= (1 + ((specialPenetration + physicalPenetration) * .6));
            Debug.Log(Damage);
            //Safety and Mobility late game stuff
            var Mobility = (mobility + evasion) * 5;
            Mobility += Damage * (damageRange - 125) * .025;
            Mobility += (50 - clearSpeed) * 48;
            Mobility += cc * 250;
            Mobility += ace * 250;
            Mobility += teamfight * 250;
            Debug.Log("===" + name + "===");
            //Stat reaction
            var totalStats = Damage + Sustain + Mobility;
            Debug.Log("Damage: " + Damage + " | Sustain: " + Sustain + " | Mobility: " + Mobility + " | totalStats: " + totalStats);
            Debug.Log("Added all: " + totalStats);
            totalStats *= 1 + ((damageRange - 124) * .025);
            Debug.Log("Damage range counting: " + totalStats);
            totalStats *= (movementSpeed - 324) * 1.1;
            Debug.Log("Movementspeed multiplier: " + totalStats);
            totalStats += (300000 * (-1 + (1 + stats.wins / 1 + stats.loses)));
            totalStats += (300000 * (-1 + (1 + stats.bans / 1 + stats.matches)));
            Debug.Log("Winrate = more picks: " + totalStats);
            totalStats *= (1 + stats.wins / 1 + stats.loses);//Higher winrate = they want to play more.
            Debug.Log("Winrate = more picks 2: " + totalStats);
            totalStats *= (1 + (intrest * .002));
            //THis is for characters that have interesting design and/or lore and has nothing to do with the kit.
            Debug.Log("Intrest: " + totalStats);
            if (totalStats < 100000) { totalStats = 100000; }
            Debug.Log("total stats:" + totalStats);
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
            Debug.Log(Sustain);
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
            Debug.Log(Damage);
            //Safety and Mobility late game stuff
            var Mobility = (mobility + evasion + unevadable + movementSpeed*4 + cooldownReduction) * 9;
            Mobility += Damage * (damageRange - 125) * .0325;
            Mobility += (50 - clearSpeed) * 64;
            Mobility += cc * 325;
            Mobility += ace * 275;
            Mobility += teamfight * 300;
            Debug.Log("===" + name + "===");
            //Stat reaction
            var totalStats = Damage + Sustain + Mobility;
            Debug.Log("Damage: " + Damage + " | Sustain: " + Sustain + " | Mobility: " + Mobility + " | totalStats: " + totalStats);
            Debug.Log("Added all: " + totalStats);
            totalStats *= 1 + ((damageRange - 124) * .025);
            Debug.Log("Damage range counting: " + totalStats);
            totalStats *= 1+(movementSpeed - 324) * 1.5;
            Debug.Log("Movementspeed multiplier: " + totalStats);
            totalStats += (650000 * (-1 + (1 + stats.picks / 1 + stats.matches)));//negative is here just to make sure it's .74 instead of like 1.33 or smthn
            Debug.Log("pciks = more bans: " + totalStats);
            totalStats *= (1 + stats.picks / 1 + stats.matches);//Higher pickrate = they want to ban more.
            Debug.Log("wins = more bans 2: " + totalStats);
            totalStats *= (1 + stats.wins / 1 + stats.loses);
            totalStats *= (1 + (vex * .004));
            //THis is for characters that have interesting design and/or lore and has nothing to do with the kit.
            Debug.Log("Intrest: " + totalStats);
            if (totalStats < 100000) { totalStats = 100000; }
            Debug.Log("total stats:" + totalStats);
            return totalStats;
        }
        public static List<Characters> FromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Characters>>(json);
        }
        //After every win. record and create files for the character who won against who.
    }
}
