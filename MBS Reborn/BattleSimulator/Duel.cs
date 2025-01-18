using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS_Reborn.xTeam;
using MBS_Reborn.Character;
using System.Reflection;
using System.Media;
using static MBS_Reborn.Character.Characters;

namespace MBS_Reborn.BattleSimulator
{
    public class FightSimulator
    {
        private FightCharacter char1;
        private FightCharacter char2;
        private double timer = 0.0; // in seconds
        private const double tickInterval = 0.25; // 0.25 seconds per tick

        private Random random = new Random();

        public FightSimulator(FightCharacter character1, FightCharacter character2)
        {
            Random x = new Random();

            char1 = character1;
            char2 = character2;

            // Apply scaling before the fight starts
            char1.ApplyScaling();
            char2.ApplyScaling();

            // Initialize positions (assuming a 1D field for simplicity)
            char1.Position = 0.0;
            char2.Position = x.Next(0,1200); 
        }

        public void Duel()
        {
            char1.isDead = false;
            char2.isDead = false;
            char1.Health = char1.BaseHealth + char1.BonusHealth;
            char2.Health = char2.BaseHealth + char2.BonusHealth;
            char1.Health = char1.BaseHealth + char1.BonusHealth;
            char2.Health *= random.Next(70, 100) * .01;
            char1.Health *= random.Next(70, 100) * .01;
            char2.isDead = false;
            while (!char1.isDead && !char2.isDead && timer < 60.0)
            {
                //Console.WriteLine($"{char1.Name} {char1.Health} vs {char2.Name} {char2.Health} <> {Math.Abs(char1.Position - char2.Position)}");
                //Console.WriteLine($"{char1.Effects.Count} effects vs {char2.Effects.Count}");
                // Increment timer
                timer += tickInterval;

                // Update cooldowns and effects
                UpdateCooldowns(char1);
                UpdateCooldowns(char2);
                UpdateEffects(char1, char2);
                UpdateEffects(char2, char1);
                UpdateRegen(char1);
                UpdateRegen(char2);

                // Decide actions for both characters
                if (random.Next(0,1)>.5)
                {
                    DecideAction(char1, char2);
                    DecideAction(char2, char1);
                }
                else
                {
                    DecideAction(char2, char1);
                    DecideAction(char1, char2);
                }

                // Check if any character died in this tick
                if (char1.Health <= 0)
                {
                    char1.isDead = true;
                    char2.Kills += 1;
                    char1.Deaths += 1;
                    char2.Gold += CalculateGold(char2, char1);
                    var fsa = 0;
                    while (fsa < 60)
                    {
                        UpdateEffects(char1, char2);
                        UpdateEffects(char2, char1);
                        fsa++;
                    }
                    break;
                }
                if (char1.Health > char1.BaseHealth + char1.BonusHealth)
                    char1.Health = char1.BaseHealth + char1.BonusHealth;
                if (char2.Health > char2.BaseHealth + char2.BonusHealth)
                    char2.Health = char2.BaseHealth + char2.BonusHealth;
                if (char1.Mana > char1.BaseMana + char1.BonusMana)
                    char1.Mana = char1.BaseMana + char1.BonusMana;
                if (char2.Mana > char2.BaseMana + char2.BonusMana)
                    char2.Mana = char2.BaseMana + char2.BonusMana;
                if (char2.Health <= 0)
                {
                    char2.isDead = true;
                    char1.Kills += 1;
                    char2.Deaths += 1;
                    char1.Gold += CalculateGold(char1, char2);
                    var fsa = 0;
                    while (fsa < 60)
                    {
                        UpdateEffects(char1, char2);
                        UpdateEffects(char2, char1);
                        fsa++;
                    }
                    break;
                }
                //Thread.Sleep(10);
            }

            // Determine the outcome if timer exceeded
            if (!char1.isDead && !char2.isDead)
            {
                if (char1.Health < char2.Health)
                {
                    char2.Kills += 1;
                    char1.Deaths += 1;
                    char2.Gold += CalculateGold(char2, char1);
                }
                else if (char2.Health < char1.Health)
                {
                    char1.Kills += 1;
                    char2.Deaths += 1;
                    char1.Gold += CalculateGold(char1, char2);
                }
                else
                {
                    // Same HP, choose randomly
                    if (random.NextDouble() < 0.5)
                    {
                        char1.Kills += 1;
                        char2.Deaths += 1;
                        char1.Gold += CalculateGold(char1, char2);
                    }
                    else
                    {
                        char2.Kills += 1;
                        char1.Deaths += 1;
                        char2.Gold += CalculateGold(char2, char1);
                    }
                }
            }

            // Output the fight results
            //Console.WriteLine($"Fight ended at {timer} seconds.");
            //Console.WriteLine($"{char1.Name} - HP: {char1.Health}, Level: {char1.Level}");
            //Console.WriteLine($"{char2.Name} - HP: {char2.Health}, Level: {char2.Level}");
            //Console.WriteLine($"=========================================");
        }

        private void UpdateCooldowns(FightCharacter character)
        {
            // Reduce cooldowns by tickInterval
            if (character.ability1Cooldown > 0)
                character.ability1Cooldown -= tickInterval;
            if (character.ability2Cooldown > 0)
                character.ability2Cooldown -= tickInterval;
            if (character.ability3Cooldown > 0)
                character.ability3Cooldown -= tickInterval;
            if (character.ability4Cooldown > 0)
                character.ability4Cooldown -= tickInterval;
            if (character.AutoAttackCD > 0)
                character.AutoAttackCD -= tickInterval;
        }

        public static void UpdateEffects(FightCharacter receiver, FightCharacter source)
        {
            var effectsToRemove = new List<Effect>();

            foreach (Effect x in receiver.Effects)
            {
                x.Duration -= tickInterval;
                if (x.Duration <= 0)
                {
                    effectsToRemove.Add(x); // Mark for removal
                }
            }

            // Remove effects after iteration.. this stops errorsss
            foreach (var effect in effectsToRemove)
            {
                removeEffect(receiver, effect);
            }
        }


        private void DecideAction(FightCharacter actor, FightCharacter target)
        {
            // Check available abilities in order (you can prioritize differently)
            // For simplicity, check Ability1 to Ability4, then auto attack

            // Determine distance
            double distance = Math.Abs(actor.Position - target.Position);

            // Attempt to use abilities
            for (int i = 1; i <= 4; i++)
            {
                if (actor.Name == "Jasmine" && i == 2)
                    i++;
                var ability = GetAbilityByIndex(actor, i);
                if (ability != null && actor.CanCast && IsAbilityReady(actor, i))
                {
                    int idx = GetAbilityPoints(actor, i);
                    if (idx < ability.DamageRange.Count && ability.Point>-1)
                    {
                        double abilityRange = ability.DamageRange[ability.Point];
                        if (ability.DashRange[ability.Point] > abilityRange)
                            abilityRange = ability.DashRange[ability.Point];
                        if (distance <= abilityRange)
                        {
                            // Attempt to cast the ability
                            if (CanCastAbility(actor, ability, i))
                            {
                                UseAbility(actor, target, ability, i);
                                return;
                            }
                        }
                    }
                }
                else
                {
                        //Console.WriteLine($"Ability {i} skipped. isNull: {ability == null}, Ready: {IsAbilityReady(actor, i)}");

                }
            }

            // Attempt auto attack if in range
            if (actor.CanAuto && actor.AutoAttackCD <= 0)
            {
                // Assuming auto attack range is defined, else define a default range
                double autoAttackRange = actor.BaseAttackRange + actor.BonusAttackRange; // Default auto attack range
                if (distance <= autoAttackRange)
                {
                    // Perform auto attack
                    PerformAutoAttack(actor, target);
                }
                else
                {
                    // Move towards the target
                    MoveTowards(actor, target);
                }
            }
            else
            {
                // Move towards the target if not in range
                double autoAttackRange = 5.0;
                if (distance > autoAttackRange)
                {
                    //Console.WriteLine(actor + " advanced.");
                    MoveTowards(actor, target);
                }
                else if (autoAttackRange>distance)
                {
                    //Console.WriteLine(actor + " kited.");
                    MoveAway(actor, target);
                }
                else
                {
                    // Cannot attack and in range, possibly reposition or do nothing
                    // For this simulation, we choose to do nothing
                }
            }
        }

        private Characters.Ability GetAbilityByIndex(FightCharacter character, int index)
        {
            return index switch
            {
                1 => character.Ability1,
                2 => character.Ability2,
                3 => character.Ability3,
                4 => character.Ultimate, // Assuming Ultimate is treated as Ability4
                _ => null,
            };
        }

        private short GetAbilityPoints(FightCharacter character, int index)
        {
            return index switch
            {
                1 => character.Ability1.Point,
                2 => character.Ability2.Point,
                3 => character.Ability3.Point,
                4 => character.Ultimate.Point,
                _ => 1,
            };
        }

        private bool IsAbilityReady(FightCharacter character, int index)
        {
            return index switch
            {
                1 => character.ability1Cooldown <= 0,
                2 => character.ability2Cooldown <= 0,
                3 => character.ability3Cooldown <= 0,
                4 => character.ability4Cooldown <= 0,
                _ => false,
            };
        }

        private bool CanCastAbility(FightCharacter caster, Characters.Ability ability, int index)
        {
            int skillPoints = ability.Point;
            if (skillPoints <= 0)
            {
                return false;
            }

            // Check mana/energy cost
            if (caster.Mana < ability.DamageCost.ElementAtOrDefault(skillPoints))
            {
                return false;
            }

            // Deduct mana/energy cost
            caster.Mana -= ability.DamageCost.ElementAtOrDefault(skillPoints);
            return true;
        }

        private void UseAbility(FightCharacter caster, FightCharacter target, Characters.Ability ability, int index)
        {
            string abilityName = $"Ability{index}";
            int skillPoints = ability.Point;
            int dmg = ability.BaseDamage.ElementAtOrDefault(skillPoints) +
                      (int)((caster.totalAttackDamage) * (ability.ADScaling.ElementAtOrDefault(skillPoints) / 100.0)) +
                      (int)(caster.AbilityPower * (ability.APScaling.ElementAtOrDefault(skillPoints) / 100.0));

            // Check hit chance
            int hitChance = ability.DamageHitChance.ElementAtOrDefault(skillPoints);
            if (caster.Ultimate==ability)
            {
                //Console.WriteLine(caster.Name + " casted Ultimate");
                if (caster.Name == "Zylia")
                    addEffect(caster, "ZyliaEnrage", 6.0, (double)ability.Point);
                if (caster.Name == "Zaguro")
                    addEffect(caster, "ZaguroSerene", 6.0, (double)ability.Point);
            }
            // Handle cooldown
            double cooldownValue = ability.Cooldown.ElementAtOrDefault(skillPoints); // Assuming DamageCost holds cooldown, adjust if different
            SetAbilityCooldown(caster, index, cooldownValue);

            // Handle dash if any
            if (ability.DashRange.ElementAtOrDefault(skillPoints) != 0)
            {
                int dashRange = ability.DashRange.ElementAtOrDefault(skillPoints);
                // Decide direction based on ability's nature (towards or away)
                // For simplicity, let's say positive dash towards target
                if (caster.Position < target.Position)
                    caster.Position += dashRange;
                else
                    caster.Position -= dashRange;
            }
            // Handle other effects like self-heal, shields, etc.
            caster.Health += ability.SelfHeal;
            caster.Shield += ability.SelfShield;
            // Implement other effects as needed

            if (caster.Name == "Zaguro")
            {
                if (caster.Ability2 == ability)
                {
                    addEffect(caster, "ZaguroW", 1, 1);
                }
            }
            if (caster.Name == "Zylia")
            {
                if (caster.Ability2 == ability)
                {
                    addEffect(caster, "MS%ChangeW", 1, .2*(1+ability.Point));
                }
            }
            if ((caster.Name == "Zaguro" || caster.Name == "Nalme") && ability == caster.Ability1)
                abilityName = "auto";
            if (caster.Name == "Zaysaku" && ability == caster.Ability3)
                dmg += (int)(target.BaseHealth + target.BonusHealth);
            if (caster.Name == "Zaysaku" && ability == caster.Ultimate)
                addEffect(caster, "MS%ChangeR", 2, .6 * (1 + ability.Point*.16));


            // EXECUTE ON HIT ================================
            if (random.Next(100) < hitChance)
            {
                if (caster.Name == "Zaysaku" && ability == caster.Ability2)
                { 
                    addEffect(caster, "ZaysakuSpecialSteal", 6, (target.SpecialResistance) * .1);
                    addEffect(target, "ZaysakuPhysicalSpecialShred", 6, 1);
                    addEffect(caster, "ZaysakuPhysicalSteal", 6, 1);
                }

                if (target.Effects.Find(x => x.EffectName.Contains("ZaysakuPhysicalSpecialShred")) != null && target.Effects.Find(x => x.EffectName.Contains("ZaysakuPhysicalPhysicalShred")) != null && target.Effects.Find(x => x.EffectName.Contains("Shred")).EffectName.Contains("Shred"))
                {
                    try
                    {
                        var Presistances = target.Effects.Find(x => x.EffectName.Contains("ZaysakuPhysicalPhysicalShred")).Magnitude * (target.PhysicalResistance) * .1;
                        var Sresistances = target.Effects.Find(x => x.EffectName.Contains("ZaysakuPhysicalSpecialShred")).Magnitude * (target.SpecialResistance) * .1;
                        caster.Effects.Find(x => x.EffectName.Contains("ZaysakuSpecialSteal")).Magnitude = Sresistances;
                        caster.Effects.Find(x => x.EffectName.Contains("ZaysakuPhysicalSteal")).Magnitude = Presistances;
                    }
                    catch 
                    { 
                        //Console.WriteLine("null cannot steal or give stolen whatever");
                            
                    }
                }

                // EXECUTE DAMAGE ================================
                ApplyDamage(caster, target, dmg, ability.DamageType, $"Ability{index}");
            }
        }
        private void addEffect(FightCharacter actor, string effect, double duration, double strength)
        {
            if (effect == "ZyliaEnrage")
            {
                actor.AttackSpeed += (.4) * (.15 * strength);
                addEffect(actor, "MS%Change", duration, 0.5);
            }
            if (effect == "ZaguroScerene")
            {
                for (int i = 0; i < actor.Ability1APScaling.Count; i++)
                {
                    actor.Ability1APScaling[i] *= 1.25 + (0.25 * strength);
                    actor.Ability2APScaling[i] *= 1.25 + (0.25 * strength);
                    actor.Ability3APScaling[i] *= 1.25 + (0.25 * strength);
                }
            }
            if (effect.Contains("MS%Change"))
            {
                strength = actor.BaseMovementSpeed * strength;
                actor.MovementSpeed += strength;
            }
            if (effect.Contains("MSChange"))
            {
                actor.MovementSpeed += strength;
            }

            actor.Effects.Add(new Effect(effect, duration, strength));
        }
        private static void removeEffect(FightCharacter actor, Effect effect)
        {
            actor.Effects.Remove(new Effect(effect.EffectName, effect.Duration, effect.Magnitude));
            var strength = effect.Magnitude;

            if (effect.EffectName == "ZyliaEnrage")
            {
                actor.AttackSpeed -= (.4) * (.15 * effect.Magnitude);
            }
            if (effect.EffectName.Contains("MS%Change"))
            {
                actor.MovementSpeed -= strength;
            }
            if (effect.EffectName.Contains("MSChange"))
            {
                actor.MovementSpeed -= strength;
            }
            if (effect.EffectName == "ZaguroScerenes")
            {
                for (int i = 0; i < actor.Ability1APScaling.Count; i++)
                {
                    actor.Ability1APScaling[i] =50;
                    actor.Ability2APScaling[i] =0;
                    actor.Ability3APScaling[i] *=30;
                }
            }
        }

        private void PerformAutoAttack(FightCharacter attacker, FightCharacter target)
        {
            // Assuming auto attack damage is BaseAttackDamage
            double dmg = attacker.totalAttackDamage;

            // Check critical strike
            if (random.NextDouble() < attacker.CriticalStrikeChance)
            {
                dmg *= attacker.CriticalStrikeDamage;
            }

            ApplyDamage(attacker, target, (int)dmg, "Physical", "auto");
            if (attacker.Name == "Proto") { ApplyDamage(attacker, target, (int)(attacker.AbilityPower * .7), "Physical", "auto"); }
            if (attacker.Name == "Zylia") { ApplyDamage(attacker, target, (int)(attacker.AbilityPower * .25)+(int)(attacker.Level*12), "Special", "auto"); }

            // Reset auto attack cooldown
            attacker.AutoAttackCD = 1.0 / attacker.AttackSpeed; // Example cooldown based on attack speed
        }

        private double CalculateDamageWithResistance(int damage, double resistance)
        {
            if (resistance >= 0)
            {
                return damage / (1 + (resistance / 100));
            }
            else
            {
                return damage * (2 - (100 / (100 - resistance)));
            }
        }



        private void SetAbilityCooldown(FightCharacter character, int index, double cooldown)
        {
            switch (index)
            {
                case 1:
                    character.ability1Cooldown = cooldown;
                    break;
                case 2:
                    character.ability2Cooldown = cooldown;
                    break;
                case 3:
                    character.ability3Cooldown = cooldown;
                    break;
                case 4:
                    character.ability4Cooldown = cooldown;
                    break;
            }
        }

        private void MoveTowards(FightCharacter mover, FightCharacter target)
        {
            double distance = target.Position - mover.Position;
            double moveDistance = mover.MovementSpeed * tickInterval;

            if (Math.Abs(distance) <= moveDistance)
            {
                mover.Position = target.Position;
            }
            else
            {
                mover.Position += Math.Sign(distance) * moveDistance;
            }

        }

        private void MoveAway(FightCharacter mover, FightCharacter target)
        {
            double moveDistance = mover.MovementSpeed * tickInterval;

            // Move away from the target
            mover.Position -= moveDistance;


        }

        private void UpdateRegen(FightCharacter actor)
        {
            actor.Health += actor.HealthRegen;
            actor.Mana += actor.ManaRegen;
        }

        private double CalculateGold(FightCharacter winner, FightCharacter loser)
        {
            double baseGold = 500.0;
            double goldDifference = winner.Gold - loser.Gold;

            if (goldDifference >= 500)
            {
                baseGold -= 150;
            }
            else if (goldDifference <= -500)
            {
                baseGold += 150;
            }

            baseGold = Math.Clamp(baseGold, 100, 1000);
            CalculateExp(winner, loser);
            return baseGold;
        }
        private void CalculateExp(FightCharacter winner, FightCharacter loser)
        {
            double baseExp = 300.0;
            double expDifference = winner.Exp - loser.Exp;

            if (expDifference >= 500)
            {
                baseExp -= 150;
            }
            else if (expDifference <= -500)
            {
                baseExp += 150;
            }
            baseExp = Math.Clamp(baseExp, 100, 1000);
            AddExp(winner, baseExp);
        }
        public void AddExp(FightCharacter winner,double Exp)
        {
            bool canSelectAbility = false;

            winner.Exp += Exp;
            if ((500 * winner.Level) < winner.Exp && winner.Level < 18)
            {
                winner.Level++;
                canSelectAbility = true;
            }

            if (canSelectAbility||Exp==0)
            {
                if (winner.Level >= 6 && winner.Level <= 16 && winner.Level % 5 == 1)
                {
                    // Select ultimate ability to level up (you might want to add a specific ability check)
                    winner.ability4Points = (short)Math.Min(winner.ability4Points + 1, 2);  // Ultimate ability can only go up to 3 points
                }
                else
                {
                    // Randomly level one of the other abilities (1, 2, or 3)
                    Random rand = new Random();
                    int abilityChoice = rand.Next(1, 4);  // Pick a number between 1 and 3

                    switch (abilityChoice)
                    {
                        case 1:
                            winner.Ability1.Point = (short)Math.Min(winner.Ability1.Point + 1, 4);
                            break;
                        case 2:
                            winner.Ability2.Point = (short)Math.Min(winner.Ability2.Point + 1, 4);
                            break;
                        case 3:
                            winner.Ability3.Point = (short)Math.Min(winner.Ability3.Point + 1, 4);
                            break;
                    }
                }
            }

            if (winner.Gold > 1900)
            {
                winner.Gold -= 1900;
                winner.BonusAttackDamage += 50;
                winner.BonusHealth += 350;
                winner.BonusMana += 350;
                winner.ManaRegen += 21;
                winner.AttackSpeed += .4;
                winner.AbilityPower += 90;
                winner.CriticalStrikeChance += .15;
                winner.MovementSpeed += 25;
                if (winner.Level>10)
                {
                    addEffect(winner, "bork", 5218097, .8);
                }
            }
        }

        private void ApplyDamage(FightCharacter attacker, FightCharacter target, int damage, string damageType, string attackType)
        {
            double effectiveDamage = damage;
            double shrededPhysical = 0;
            double shrededSpecial = 0;

            float TrueDamage = 0;

            if (attacker.hasEffect("bork") && attackType.Contains("auto"))
                effectiveDamage += target.Health * .08;
            //check target's special shreded.
            try
            {
                if (target.Effects.Find(x => x.EffectName.Contains("Shred")) != null && target.Effects.Find(x => x.EffectName.Contains("Shred")).EffectName.Contains("Special"))
                {
                    shrededSpecial += target.SpecialResistance * (.1 * target.Effects.Find(x => x.EffectName.Contains("Zaysaku")).Magnitude);
                }
            }
            catch
            { }
            //check target's physical shreded.
            try
            {
                if (target.Effects.Find(x => x.EffectName.Contains("Shred")) != null && target.Effects.Find(x => x.EffectName.Contains("Shred")).EffectName.Contains("Physical"))
                {
                    shrededSpecial += target.SpecialResistance * (.1 * target.Effects.Find(x => x.EffectName.Contains("Zaysaku")).Magnitude);
                }
            }
            catch { }

            if (target.Effects.Find(x => x.EffectName.Contains("ZaysakuWShred")) != null && target.Effects.Find(x => x.EffectName.Contains("Zaysaku")).EffectName.Contains("Shred") && attacker.Name == "Zaysaku")
            {
                if (target.Effects.Find(x => x.EffectName.Contains("ZaysakuWShred")).Magnitude >= 1)
                {
                    if (Math.Abs(target.Position - attacker.Position) < 450 + (50 * attacker.Ability2.Point))
                    {
                        effectiveDamage += (.05 + (.0012 * (attacker.BaseHealth + attacker.BonusHealth))) * target.Health;
                        effectiveDamage *= 1.75;
                    }
                }
            }
            else if (target.Effects.Find(x => x.EffectName.Contains("Zaysaku")) != null && attacker.Name == "Zaysaku")
            {
                target.Effects.Find(x => x.EffectName.Contains("Zaysaku")).Magnitude += 1;
            }

            //Regular Damage types.. and Zaguro
            if (attacker.Name == "Zaguro")
            {
                float ConvertPercent = MathF.Pow(attacker.Level, 1.593279542f) / 100f;
                ConvertPercent = MathF.Min(ConvertPercent, 1.0f);
                TrueDamage = damage * ConvertPercent;
                effectiveDamage *= (1 - ConvertPercent);
            }
            else if (damageType == "True Damage")
            {
                effectiveDamage = CalculateDamageWithResistance(damage, target.PhysicalResistance - shrededPhysical * (1 - attacker.PhysicalPenetration));
            }
            else if (damageType == "Physical")
            {
                effectiveDamage = CalculateDamageWithResistance(damage, target.PhysicalResistance - shrededPhysical * (1 - attacker.PhysicalPenetration));
            }
            else if (damageType == "Special")
            {
                effectiveDamage = CalculateDamageWithResistance(damage, target.SpecialResistance - shrededSpecial * (1 - attacker.SpecialPenetration));
            }
            else
            {
                effectiveDamage = CalculateDamageWithResistance(damage, target.SpecialResistance);
            }

            // Add true damage to effective damage
            effectiveDamage += TrueDamage;

            if (attackType == "auto")
            {
                Heal(attacker, attacker.LifeSteal * effectiveDamage);
            }
            if (attackType.Contains("Ability") || attackType.Contains("Ultimate"))
            {
                Heal(attacker, attacker.SpellVamp * effectiveDamage);
            }

            // Ensure damage is not negative
            effectiveDamage = Math.Max(effectiveDamage, 0);

            if (target.hasEffect("ZaguroW"))
            {
                var a = target;
                target = attacker;
                attacker = a;
                effectiveDamage *= 2;
                //Console.WriteLine($"REFLECTED");
            }
            // Apply shield first
            if (target.Shield > 0)
            {
                if (target.Shield >= effectiveDamage)
                {
                    target.Shield -= effectiveDamage;
                    effectiveDamage = 0;
                }
                else
                {
                    effectiveDamage -= target.Shield;
                    target.Shield = 0;
                }
            }
            // Subtract HP
            target.Health -= effectiveDamage;

            //Console.WriteLine($"{attacker.Name} hit {target.Name} with {attackType} for {effectiveDamage} damage. AP: {attacker.AbilityPower}  AD: {attacker.BaseAttackDamage}:{attacker.BonusAttackDamage}");
        }

        private void Heal(FightCharacter actor, double amount)
        {
            actor.Health += amount;

        }
    }

}
