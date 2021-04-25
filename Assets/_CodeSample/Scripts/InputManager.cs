using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectEvent;

namespace NAH
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        SOGameEvent _onBeginDrag;
        [SerializeField]
        Vector2GameEvent _onDrag;
        [SerializeField]
        Vector2GameEvent _onEndDrag;

        private Vector2 _direction;
        private Vector2 _initialTouchPosition;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialTouchPosition = Input.mousePosition;
                _onBeginDrag.Raise();
            }

            if (Input.GetMouseButton(0))
            {
                _direction = (Vector2)Input.mousePosition - _initialTouchPosition;
                _direction.Normalize();
                _onDrag.Raise(_direction);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _direction = (Vector2)Input.mousePosition - _initialTouchPosition;
                _direction.Normalize();
                _onEndDrag.Raise(_direction);
            }
        }
    }
}
