using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DungeoneerAssets
{
    public class Shake : MonoBehaviour
    {
        private Vector3 startPosition;
        public float minDelta;
        public float maxDelta;

        private void OnEnable()
        {
            this.startPosition = this.transform.position;
        }

        private void OnDisable()
        {
            this.transform.position = this.startPosition;
        }

        private void Update()
        {
            float val = UnityEngine.Random.Range(minDelta, maxDelta);
            Vector3 newVal = this.startPosition + (this.transform.right * val * Time.deltaTime);
            val = UnityEngine.Random.Range(minDelta, maxDelta);
            newVal += (this.transform.up * val * Time.deltaTime);
            this.transform.position = newVal;
        }
    }
}