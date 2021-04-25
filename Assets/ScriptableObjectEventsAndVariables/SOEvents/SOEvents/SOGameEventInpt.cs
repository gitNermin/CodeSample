using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectEvent
{
    public class SOGameEventInpt<T> : ScriptableObject
    {
        //List of all objects/methods subscribed to this GameEvent
        private List<GameEventListenerInpt<T>> listeners = new List<GameEventListenerInpt<T>>();

        //Description of when this event is raised
        [TextArea]
        [Tooltip("When is this event raised")]
        public string eventDescription = "[When does this event trigger]";

        public void Raise(T value)
        {
#if UNITY_EDITOR
            //Debug - show the event has been raised
            //Debug.Log(this.name + " event raised");
#endif
            //Loop through the listener list and raise the events passed
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        //Add the gameEventListener to the listener list
        public void RegisterListener(GameEventListenerInpt<T> listener)
        {
            listeners.Add(listener);
        }

        //Remove the gameEventListener to the listener list
        public void UnregisterListener(GameEventListenerInpt<T> listener)
        {
            listeners.Remove(listener);
        }
    }
}
