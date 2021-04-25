using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectEvent
{
    [CreateAssetMenu(fileName = "soGameEvent", menuName = "soGameEvents/soStringGameEvent", order = 1)]
    public class StringGameEvent : SOGameEventInpt<string> { }
}
