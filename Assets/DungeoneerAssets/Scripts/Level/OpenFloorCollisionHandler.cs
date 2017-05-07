using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class OpenFloorCollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.name == "Main Camera")
            {
                GameObject go = GameObject.Find("System");
                BattleManager bm = go.GetComponent<BattleManager>();
                if (this.BossData != null)
                {
                    bm.BossData = BossData;
                    bm.StartBossBattle();
                }
                else
                {
                    bm.BossData = null;
                    bm.RollForBattle();
                }

            }
        }

        public MonsterCreationData BossData { get; set; }
    }
}