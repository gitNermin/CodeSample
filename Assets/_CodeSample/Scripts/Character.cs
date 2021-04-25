using ScriptableObjectVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        [SerializeField]
        private SOFloat _speed;
        [SerializeField]
        private SOVector2 _position;

        private Rigidbody2D _myRigidBody;

        private void Start()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
        }

        public void MoveCharacter(Vector2 direction)
        {
            _myRigidBody.velocity = direction * _speed.value;
        }

        private void Update()
        {
            _position.value = transform.position;
        }

    }

}
