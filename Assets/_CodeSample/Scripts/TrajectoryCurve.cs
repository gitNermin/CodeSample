using ScriptableObjectVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAH
{
    public class TrajectoryCurve : MonoBehaviour
    {
        [SerializeField]
        private int _pointsCount;
        [SerializeField]
        private float _charaterRadius = 0.8f;
        [SerializeField]
        private Transform _hitPoint;
        [SerializeField]
        LayerMask _layer;
        [SerializeField]
        AnimationCurve _sizeCurve;
        [SerializeField]
        Gradient _colorGradient;

        [SerializeField]
        SOFloat _speed;
        [SerializeField]
        SOVector2 _characterPosition;
        [SerializeField]
        TrajectoryPathPoint _dotPrefab;

        TrajectoryPathPoint[] _dots;

        private void Awake()
        {
            Initialize();
        }
        public void Initialize()
        {
            _dots = new TrajectoryPathPoint[_pointsCount];
            for (int i = 0; i < _pointsCount; i++)
            {
                _dots[i] = Instantiate(_dotPrefab);
                _dots[i].transform.SetParent(transform);
                _dots[i].Visible = false;
            }
        }

        public void Draw(Vector2 movementDirection)
        {
            var initialVelocity = movementDirection * _speed.value;
            var startPosition = _characterPosition.value + movementDirection * _charaterRadius;
            _hitPoint.gameObject.SetActive(false);
            Vector2 position = Vector2.zero;
            Vector2 lastPosition = startPosition;
            Vector2 direction = Vector2.zero;
            float distance = 0;
            float time = 0;
            int i;
            for ( i= 0; i < _pointsCount; i++)
            {
                position.x = startPosition.x + initialVelocity.x * time;
                position.y = startPosition.y + initialVelocity.y * time - 0.5f * 9.8f * time * time;
                direction = position - lastPosition;
                distance = direction.magnitude;
                direction.Normalize();
                RaycastHit2D hit = Physics2D.Raycast(lastPosition, direction, distance, _layer);
                if (!hit)
                {
                    _dots[i].Position = position;
                    ShowPoint(i);
                    time += Time.fixedUnscaledDeltaTime;
                    lastPosition = position;
                }
                else
                {
                    if ((i+1) < _pointsCount && hit.transform.CompareTag("Bouncy"))
                    {
                        _dots[i].Position = hit.point;
                        ShowPoint(i);
                        Vector2 normal = hit.normal;
                        
                        Vector2 velocity = Vector2.zero;
                        velocity.x = initialVelocity.x;
                        velocity.y = initialVelocity.y - 9.8f * time;

                        startPosition = hit.point;
                        initialVelocity = Vector2.Reflect(velocity,  -normal);
                        time = Time.fixedUnscaledDeltaTime;

                        position.x = startPosition.x + initialVelocity.x * time;
                        position.y = startPosition.y + initialVelocity.y * time - 0.5f * 9.8f * time * time;

                        ++i;
                        _dots[i].Position = position;
                        ShowPoint(i);

                        time += Time.fixedUnscaledDeltaTime;
                        lastPosition = position;

                    }
                    else
                    {
                        _hitPoint.position = hit.point;
                        _hitPoint.gameObject.SetActive(true);
                        break;
                    }
                }
            }

            for (; i < _pointsCount; i++)
            {
                _dots[i].Visible = false;
            }
           
        }


        void ShowPoint(int index)
        {
            if (index > _pointsCount) return;
            float percentage = (float)index / (_pointsCount - 1);
            _dots[index].Visible = true;
            _dots[index].Size = _sizeCurve.Evaluate(percentage);
            _dots[index].Color = _colorGradient.Evaluate(percentage);
        }
        public void Show()
        {
            for (int i = 0; i < _pointsCount; i++)
            {
                _dots[i].Visible = true;
            }
        }

        public void Hide()
        {
            _hitPoint.gameObject.SetActive(false);
            for (int i = 0; i < _pointsCount; i++)
            {
                _dots[i].Visible = false;
            }
        }
    }

}
