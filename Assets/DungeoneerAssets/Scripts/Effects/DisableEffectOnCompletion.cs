using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class DisableEffectOnCompletion : MonoBehaviour
    {
        private ParticleSystem ps;

        private void OnEnable()
        {
            if (ps == null)
                ps = this.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (ps != null && !ps.isPlaying)
                this.gameObject.SetActive(false);
        }
    }
}