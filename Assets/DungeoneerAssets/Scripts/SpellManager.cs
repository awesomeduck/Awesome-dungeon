using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class SpellManager : MonoBehaviour
    {
        public EffectManager fxManager;
        public AudioManager audioManager;
        public Player player;
        public BattleManager battleManager;
        public FloorManager floorManager;
        public UIManager uiManager;

        public bool CanCastSpell(SpellDefinition def, out string reason)
        {
            reason = "";
            if (!this.player.CheckMPRequirement(def.Name))
            {
                reason = "Not enough MP to cast " + def.DisplayName;
                return false;
            }
            if (!this.player.CanCastSpell(def.Name))
            {
                reason = "You don't need to use this spell right now.";
                return false;
            }
            if (def.StatRequirement != null)
            {
                if (!def.RequirementsMet(this.player.GetStatRequirement()))
                {
                    reason = "You don't meet the requirements to use this spell.";
                    return false;
                }
            }
            if (def.Name == "Scan" && BattleManager.InBattle && battleManager.scanResultsDisplay.gameObject.activeInHierarchy)
            {
                reason = "You don't need to use this spell right now.";
                return false;
            }
            return true;
        }

        public int CastSpell(string spellName)
        {
            int amount = this.player.CastSpell(spellName);
            switch (spellName)
            {
                case "Fire":
                    this.fxManager.ShowEffect(Effect.FireSpell);
                    this.audioManager.PlaySFX(SFX.FireSpell);
                    break;

                case "Heal":
                    this.fxManager.ShowEffect(Effect.HealSpell);
                    this.audioManager.PlaySFX(SFX.HealSpell);
                    break;

                case "Scan":
                    this.fxManager.ShowEffect(Effect.ScanSpell);
                    this.audioManager.PlaySFX(SFX.ScanSpell);
                    break;

                case "Shield":
                    this.fxManager.StartEffect(Effect.ShieldSpell);
                    this.audioManager.PlaySFX(SFX.ShieldSpell);
                    break;

                case "Barrier":
                    this.fxManager.StartEffect(Effect.BarrierSpell);
                    this.audioManager.PlaySFX(SFX.ShieldSpell);
                    break;

                case "Exit":
                    this.fxManager.ShowEffect(Effect.ExitSpell);
                    this.audioManager.PlaySFX(SFX.ExitSpell);
                    this.uiManager.DeactivateAll();
                    floorManager.Invoke("GoToTown", this.fxManager.exitSpell.GetComponent<ParticleSystem>().duration);
                    break;

                case "Focus":
                    this.fxManager.ShowEffect(Effect.Focus);
                    this.audioManager.PlaySFX(SFX.Focus);
                    break;

                case "Meditate":
                    this.fxManager.ShowEffect(Effect.Meditate);
                    this.audioManager.PlaySFX(SFX.Meditate);
                    break;

                case "FierceAttack":
                    this.fxManager.ShowEffect(Effect.FierceAttack);
                    this.audioManager.PlaySFX(SFX.PlayerAttack);
                    break;

                case "Stun":
                    this.fxManager.ShowEffect(Effect.Stun);
                    this.audioManager.PlaySFX(SFX.Stun);
                    break;

                case "DoubleAttack":
                    this.fxManager.ShowEffect(Effect.DoubleAttack);
                    this.audioManager.PlaySFX(SFX.DoubleAttack);
                    break;

                case "ExtendedGuard":
                    this.fxManager.ShowEffect(Effect.ExtendedGuard);
                    this.audioManager.PlaySFX(SFX.ExtendedGuard);
                    break;

                case "PerfectGuard":
                    this.fxManager.ShowEffect(Effect.PerfectGuard);
                    this.audioManager.PlaySFX(SFX.PerfectGuard);
                    break;
            }

            return amount;

        }

        public string GetDescriptionText(string spellName, int amount)
        {
            switch (spellName)
            {
                case "Fire":
                    return string.Format("Spell hits for {0} damage.", amount);

                case "Heal":
                    return string.Format("Player healed for {0} HP.", amount);

                case "Scan":
                    return string.Format("Enemy has {0} remaining health.", battleManager.Enemy.Stats.HP);

                case "Shield":
                    return "Physical damage reduced.";

                case "Barrier":
                    return "Magical damage reduced.";

                case "Focus":
                    return "Focusing to strengthen the next spell.";

                case "Meditate":
                    return string.Format("Meditating restored {0} MP.", amount);

                case "FierceAttack":
                    return string.Format("Fierce attack hit for {0} HP.", amount);

                case "Stun":
                    return amount == 1 ? "The enemy was stunned." : "Attempt to stun failed.";

                case "DoubleAttack":
                    return "Player used double attack.";

                case "ExtendedGuard":
                    return "Damage reduced";

                case "PerfectGuard":
                    return "Damage negated";
            }
            return null;
        }
    }
}