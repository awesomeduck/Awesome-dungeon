using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public class TimedStatModifier
    {
        public int RemainingDuration { get; set; }
        public Stat AffectedStat { get; set; }
        public int Amount { get; set; }
        public Source StatSource { get; set; }

        public enum Stat
        {
            PhysAttack,
            PhysDefense,
            MagAttack,
            MagDefense,
            Stun
        }

        public enum Source
        {
            BarrierSpell,
            ShieldSpell,
            Stun,
            ExtendedGuard,
            PerfectGuard,
            Focus
        }
    }
}