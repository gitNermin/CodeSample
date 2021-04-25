using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    public class PlatformSticker : MonoBehaviour
    {
        Joint2D _joint;

        public bool IsFree
        {
            get
            {
                return !_joint;
            }
        }
        public void Stick(Joint2D joint)
        {
            _joint = joint;
        }

        public void BreakFree()
        {
            if(!IsFree)
            {
                Destroy(_joint);
            }
        }
    }
}
