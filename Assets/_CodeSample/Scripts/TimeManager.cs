using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    public class TimeManager : MonoBehaviour
    {
        [Header("slow motion")]
        [SerializeField]
        private float _slowdownFactor = 0.05f;

        public void ApplytSlowMotion()
        {
            Time.timeScale = _slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        public void ClearSlowMotion()
        {
            Time.timeScale = 1;
        }

        public void FreezTime()
        {
            Time.timeScale = 0;
        }

        public void UnFreezeTime()
        {
            Time.timeScale = 1;
        }
    }
}
