using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NAH
{
    public class TrajectoryPathPoint : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public float Size
        {
            set
            {
                transform.localScale = value * Vector3.one;
                //transform.DOScale(value, 0.05f);           
            }
        }

        public Color Color
        {
            set
            {
                _spriteRenderer.color = value;
            }
        }

        public Vector2 Position
        {
            set
            {

                transform.position = value;
                //transform.DOMove(value, 0.05f);
            }
        }


        public bool Visible
        {
            set
            {
                gameObject.SetActive(value);
            }
        }


    }
}
