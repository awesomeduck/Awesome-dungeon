using UnityEngine;
using System.Collections;

namespace DungeoneerAssets
{
    public class StairsCollisionHandler : MonoBehaviour
    {
        public TileType tileType;

        private void OnTriggerEnter(Collider col)
        {
            if (col.name == "Main Camera")
            {
                GameObject go = GameObject.Find("System");
                if (go != null)
                {
                    FloorManager fm = go.GetComponent<FloorManager>();
                    if (fm != null)
                    {
                        if (tileType == TileType.StairsDown)
                            fm.GoToNextFloor();
                        else if (tileType == TileType.StairsUp)
                            fm.GoToPrevFloor();
                    }
                }
            }
        }
    }
}