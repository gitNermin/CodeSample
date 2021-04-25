using UnityEditor;
using UnityEngine;

namespace ScriptableObjectEvent
{
    [CreateAssetMenu(fileName = "soGameEvent", menuName = "soGameEvents/soFloatGameEvent", order = 1)]
    public class FloatGameEvent : SOGameEventInpt<float> { }
}