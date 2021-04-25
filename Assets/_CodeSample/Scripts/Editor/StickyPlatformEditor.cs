using UnityEditor;
using UnityEngine;

namespace NAH
{
    [CustomEditor(typeof(StickyPlatform))]
    public class StickyPlatformEditor : Editor
    {
        void OnSceneGUI()
        {
            Handles.color = Color.red;
            StickyPlatform platform = (StickyPlatform)target;
            float angle = 180 - platform.StickAngle;
            angle /= 2;
            Vector2 startAxis = Quaternion.Euler(0, 0, angle) * platform.Right;
            Handles.DrawSolidArc(platform.transform.position, Vector3.forward, startAxis, platform.StickAngle, 3);
        }
    }
}