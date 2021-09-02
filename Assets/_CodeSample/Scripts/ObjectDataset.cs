using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingTut
{
    public class ObjectDataset<T> : ScriptableObject
    {
        [SerializeField]
        protected List<T> _Objects;

        public T this[int index]
        {
            get
            {
                _lastSelectedIndex = index;
                return _Objects[index];
            }
            set
            {
                if(index<_Objects.Count)
                {
                    _Objects[index] = value;
                }
                else
                {
                    _Objects.Add(value);
                }
            }
        }

        public int Count
        {
            get
            {
                return _Objects.Count;
            }
        }

        private int _lastSelectedIndex;

        public int LastSelectedIndex
        {
            get
            {
                return _lastSelectedIndex;
            }
        }


        public T RandomObject
        {
            get
            {
                _lastSelectedIndex = Random.Range(0, _Objects.Count);
                return _Objects[_lastSelectedIndex];
            }
        }

        public void Clear()
        {
            _Objects.Clear();
        }

        public int GetIndex(T obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) return -1;
            return _Objects.IndexOf(obj);
        }

        public string[] Names
        {
            get
            {
                string[] names = new string[_Objects.Count];
                for (int i = 0; i < _Objects.Count; i++)
                {
                    names[i] = _Objects[i].ToString();
                }
                return names;
            }
        }
    }
}
