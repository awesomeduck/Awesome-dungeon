using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class EffectManager : MonoBehaviour
    {
        public GameObject playerAttack;
        public GameObject fireSpell;
        public GameObject playerHealSpell;
        public GameObject scanSpell;
        public GameObject smokeBomb;
        public GameObject shieldSpell;
        public GameObject barrierSpell;
        public GameObject enemyFireSpell;
        public GameObject exitSpell;
        public GameObject enemyHealSpell;
        public GameObject enemyShieldSpell;
        public GameObject enemyBarrierSpell;

        public GameObject focusEffect;
        public GameObject meditateEffect;
        public GameObject FierceAttackEffect;
        public GameObject StunEffect;
        public GameObject DoubleAttackEffect;
        public GameObject ExtendedGuardEffect;
        public GameObject PerfectGuardEffect;

        public void ShowEffect(Effect fx)
        {
            GameObject effect = this.GetEffect(fx);
            if (effect != null)
            {
                effect.SetActive(false);
                effect.SetActive(true);
            }
        }

        public void StartEffect(Effect fx)
        {
            GameObject effect = this.GetEffect(fx);
            if (effect != null)
                effect.SetActive(true);
        }

        public void StopEffect(Effect fx)
        {
            GameObject effect = this.GetEffect(fx);
            if (effect != null)
                effect.SetActive(false);
        }

        public void StopAllEffects()
        {
            this.playerAttack.SetActive(false);
            this.fireSpell.SetActive(false);
            this.playerHealSpell.SetActive(false);
            this.scanSpell.SetActive(false);
            this.smokeBomb.SetActive(false);
            this.shieldSpell.SetActive(false);
            this.barrierSpell.SetActive(false);
            this.enemyFireSpell.SetActive(false);
        }

        public void StopEffects(params Effect[] effects)
        {
            if (effects != null && effects.Length > 0)
            {
                foreach (Effect fx in effects)
                {
                    GameObject go = this.GetEffect(fx);
                    if (go != null)
                        go.SetActive(false);
                }
            }
        }

        private GameObject GetEffect(Effect fx)
        {
            switch (fx)
            {
                case Effect.PlayerAttack:
                    return this.playerAttack;
                case Effect.HealSpell:
                    return this.playerHealSpell;
                case Effect.FireSpell:
                    return this.fireSpell;
                case Effect.ShieldSpell:
                    return this.shieldSpell;
                case Effect.BarrierSpell:
                    return this.barrierSpell;
                case Effect.ScanSpell:
                    return this.scanSpell;
                case Effect.SmokeBomb:
                    return this.smokeBomb;
                case Effect.EnemyFireSpell:
                    return this.enemyFireSpell;
                case Effect.ExitSpell:
                    return this.exitSpell;
                case Effect.EnemyHealSpell:
                    return this.enemyHealSpell;
                case Effect.EnemyShieldSpell:
                    return this.enemyShieldSpell;
                case Effect.EnemyBarrierSpell:
                    return this.enemyBarrierSpell;
                case Effect.Focus:
                    return this.focusEffect;
                case Effect.Meditate:
                    return this.meditateEffect;
                case Effect.FierceAttack:
                    return this.FierceAttackEffect;
                case Effect.Stun:
                    return this.StunEffect;
                case Effect.DoubleAttack:
                    return this.DoubleAttackEffect;
                case Effect.ExtendedGuard:
                    return this.ExtendedGuardEffect;
                case Effect.PerfectGuard:
                    return this.PerfectGuardEffect;
                default:
                    break;
            }
            return null;
        }
    }

    public enum Effect
    {
        PlayerAttack,
        HealSpell,
        FireSpell,
        EnemyFireSpell,
        ShieldSpell,
        BarrierSpell,
        ScanSpell,
        SmokeBomb,
        ExitSpell,
        EnemyHealSpell,
        EnemyShieldSpell,
        EnemyBarrierSpell,
        Focus,
        Meditate,
        FierceAttack,
        Stun,
        DoubleAttack,
        ExtendedGuard,
        PerfectGuard
    }
}