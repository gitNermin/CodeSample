using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Oscillator : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _direction = Vector2.up;
        [SerializeField]
        private float _distance = 2;
        [SerializeField]
        private float _frequency = 1;

        [SerializeField]
        SpriteRenderer _dash;

        private Vector2 _startPosition;

        private Rigidbody2D _rigidBody;


        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.isKinematic = true;
            _startPosition = transform.position;
            _direction = _direction.normalized;
            SpriteRenderer dash = Instantiate(_dash);
            dash.transform.position = transform.position;
            dash.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
            Vector2 size = dash.size;
            size.x = _distance*2;
        }

        private void FixedUpdate()
        {
            _rigidBody.MovePosition(_startPosition + _direction * _distance * Mathf.Sin(Time.time *_frequency));
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying) return;
            Vector2 direction = _direction.normalized;
            Vector3 pointOne = transform.position + (Vector3) direction * _distance;
            Vector3 pointTwo = transform.position - (Vector3) direction * _distance;
            Gizmos.DrawLine(pointOne, pointTwo);
            Gizmos.DrawWireSphere(pointOne, 0.3f);
            Gizmos.DrawWireSphere(pointTwo, 0.3f);
        }
    }
}
