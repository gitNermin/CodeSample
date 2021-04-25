using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickyPlatform : MonoBehaviour
    {
        Rigidbody2D _rigidBody;
        [Range(0,180)]
        public float StickAngle = 180;
        public Vector2 Right = Vector2.right;


        Vector2 _up;

        private float _cosAngle;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _cosAngle = Mathf.Cos(StickAngle / 2f);
            _up = (new Vector2(-Right.y, Right.x)).normalized;
        }
        public void Attach(PlatformSticker sticker, Vector2 point)
        {
            FixedJoint2D joint = sticker.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = _rigidBody;
            joint.anchor = sticker.transform.InverseTransformPoint(point);
            sticker.Stick(joint);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            PlatformSticker sticker = collision.transform.GetComponent<PlatformSticker>();
            var contacts = collision.contacts;
            if(sticker && sticker.IsFree)
            {
                foreach (var contact in contacts)
                {
                    if (Vector2.Dot(_up, contact.normal) < _cosAngle)
                    {
                        Attach(sticker, collision.contacts[0].point);
                        break;
                    }
                }
            }
        }
    }
}
