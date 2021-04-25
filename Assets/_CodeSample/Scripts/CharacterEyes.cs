using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    
    public class CharacterEyes : MonoBehaviour
    {
        [SerializeField]
        Transform _pupil;
        [SerializeField]
        Range _blinkRate;

        private void Start()
        {
            StartCoroutine("BlinkCoroutine");
        }
        public void OnBeginDragHandler()
        {
            transform.DOScaleX(0.7f, 0.1f);
            StopCoroutine("BlinkCoroutine");
        }
        public void OnMouseDragHandler(Vector2 direction)
        {
            transform.localPosition = direction * 0.35f;
            _pupil.position = (Vector2)transform.position + direction*0.15f;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
        }

        public void OnEndDrag(Vector2 direction)
        {
            transform.DOLocalMove(Vector2.zero, 0.1f);
            transform.DOScaleX(0.9f, 0.1f);
            transform.rotation = Quaternion.identity;
            _pupil.DOLocalMove(Vector2.zero, 0.1f);
            StartCoroutine("BlinkCoroutine");
        }

        
        IEnumerator BlinkCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_blinkRate.Value);
                Blink();
            }
        }
        public void Blink()
        {
            transform.DOScaleY(0, 0.2f);
            transform.DOScaleY(0.9f, 0.1f).SetDelay(0.2f);
        }
    }
    
    [System.Serializable]
    public class Range
    {
        [SerializeField]
        float _min;
        [SerializeField]
        float _max;

        public float Value
        {
            get
            {
                return Random.Range(_min, _max);
            }
        }
    }
}
