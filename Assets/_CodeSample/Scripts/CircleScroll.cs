using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ScrollStart
{
    left,
    center
}
public class CircleScroll : MonoBehaviour
{
    [Header("Elipse Properties")]
    public Vector2 Center;
    [SerializeField]
    float radiusX = 2;
    [SerializeField]
    float radiusY = 2;
    //[Space(20)]

    [Header("Layout Properties")]
    [SerializeField] int visibleItemsCount = 2;
    [SerializeField]
    float itemsCount;
    [SerializeField]
    float startAngle;
    [SerializeField]
    ScrollStart _startFrom;
    [SerializeField]
    bool _reverse;
    public bool test;
    [SerializeField]
    Button _nextButton;
    [SerializeField]
    Button _prevButton;
    //[Space(20)]

    [Header("Deafault Item Prefab")]
    [SerializeField]
    GameObject _itemPrefab;
    //[Space(20)]

    [Header("Pagining")]
    [SerializeField]
    CircleScroll _pages;
    //[Space(20)]

    [Header("Scale Animation")]
    [SerializeField]
    bool _scaleAnimation;
    [SerializeField]
    AnimationCurve _scaleCurve;
    [SerializeField]
    float _minScale;
    [SerializeField]
    float _maxScale;

    public UnityAction<int> OnItemChanged;

    private List<CircleScrollItem> Items = new List<CircleScrollItem>();

    private Vector2 _center;

    private float _itemSize;
    private float _shiftAngle;
    private float _angleDelta = 0;
    private int _firstVisibleItem;
    int _selectedItem;
    public int SelectedItem
    {
        get
        {
            return _selectedItem;
        }

        private set
        {
            _selectedItem = value;
            OnItemChanged?.Invoke(value);
        }
    }

    void Awake()
    {
        GetItems();
        
        Initialize();

        Setup();

        UpdateItemsScale();

        int limit = Mathf.Min(visibleItemsCount, Items.Count);
        for (int i = 0; i < limit; i++)
        {
            Items[i].Show();
        }

        if (_reverse)
            GoToItem(Items.Count);

        CheckButtons();
    }

    private void Start()
    {
        if (_pages)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                AddPageItem();
            }
        }
    }

    void Initialize()
    {
        _shiftAngle = startAngle;
        _itemSize = 2 * Mathf.PI / itemsCount;
        _shiftAngle = _shiftAngle * Mathf.Deg2Rad;
        _center = transform.TransformPoint(Center);

        if (_startFrom == ScrollStart.center && Items.Count < visibleItemsCount)
        {
            _shiftAngle -= (((visibleItemsCount - Items.Count) / 2f) * _itemSize);
        }
    }

    void GetItems()
    {
        foreach (Transform trans in transform)
        {
            Items.Add(trans.GetComponent<CircleScrollItem>());
        }
    }

    void Setup()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Vector2 p2 = GetPosition(i);
            Items[i].transform.position = p2;
            Items[i].transform.up = (p2 - _center).normalized;
        }
    }

    void UpdateItemsScale()
    {
        if (!_scaleAnimation) return;
        for (int i = 0; i < Items.Count; i++)
        {
            if (_startFrom == ScrollStart.center && Items.Count < visibleItemsCount)
            {
                float modefiedIndex = (visibleItemsCount - (float)Items.Count) / 2 + i;
                Items[i].transform.localScale = GetItemScale(modefiedIndex) * Vector3.one;
            }
            else
            {
                Items[i].transform.localScale = GetItemScale(i) * Vector3.one;
            }
        }
    }

    void AddPageItem()
    {
        PageCircleScrollItem item = _pages.AddItem() as PageCircleScrollItem;
        item.MyCircleScroll = this;
    }

    void CheckButtons()
    {
        if (_nextButton)
        {
            _nextButton.interactable = Items.Count > visibleItemsCount && _selectedItem < (Items.Count - 1);
        }
        if (_prevButton)
        {
            _prevButton.interactable = Items.Count > visibleItemsCount && _selectedItem > 0;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Destroy(Items[i].gameObject);
        }
        Items.Clear();
        _firstVisibleItem = 0;
        _selectedItem = 0;
        OnItemChanged = null;
        _shiftAngle = 0;
        _angleDelta = 0;
        
        if (_pages)
        {
            _pages.Clear();
        }

        Initialize();

        //GetItems();

        Setup();

        UpdateItemsScale();

        CheckButtons();
        
    }


    public CircleScrollItem AddItem(bool goToItem = false)
    {
        GameObject item = Instantiate(_itemPrefab);
        CircleScrollItem circleScrollItem = item.GetComponent<CircleScrollItem>();
        AddItem(circleScrollItem, goToItem);
        CheckButtons();
        return circleScrollItem;
    }

    public void AddItem(CircleScrollItem item, bool goToItem = false)
    {
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        item.transform.position = GetPosition(Items.Count);

        item.transform.up = item.transform.position - (Vector3)_center;
        Items.Add(item);
        if (Items.Count <= _firstVisibleItem + visibleItemsCount) item.Show();
        else item.Hide();
        if (_pages)
        {
            AddPageItem();
        }
        if (_startFrom == ScrollStart.center && Items.Count <= visibleItemsCount)
        {
            _shiftAngle += _itemSize / 2f;
            Setup();
        }
        UpdateItemsScale();

        if (goToItem) GoToItem(Items.Count);
    }

    public void Next()
    {
        Next(0.2f, null);
    }

    private void Next(float time, UnityAction onComplete)
    {
        if (_firstVisibleItem + visibleItemsCount >= transform.childCount)
        {
            if (SelectedItem + 1 < transform.childCount)
            {
                SelectedItem++;
                CheckButtons();
            }
            if (_pages)
                _pages.GoToItem(_firstVisibleItem);
            return;
        }
        if (_angleDelta != 0) return;
        float startShiftAngle = _shiftAngle;
        Items[_firstVisibleItem].FadeOut();
        Items[_firstVisibleItem + visibleItemsCount].FadeIn();
        DOTween.To(() => _angleDelta, a => _angleDelta = a, _itemSize, time).OnUpdate(() =>
        {
            _shiftAngle = startShiftAngle + _angleDelta;
            Setup();
        }).OnComplete(() =>
        {
            _firstVisibleItem++;
            SelectedItem++;
            _angleDelta = 0;
            onComplete?.Invoke();
            CheckButtons();
            if (_pages)
                _pages.GoToItem(_firstVisibleItem);
        });

        if (_scaleAnimation)
        {
            for (int i = _firstVisibleItem; i < _firstVisibleItem + visibleItemsCount; i++)
            {
                Items[i].transform.DOScale(GetItemScale(i - 1), 0.2f);
            }
        }
    }

    public void Prev()
    {
        Prev(0.2f, null);
    }

    private void Prev(float time, UnityAction onComplete)
    {
        if (_nextButton) _nextButton.interactable = true;
        if (_firstVisibleItem <= 0)
        {
            if (SelectedItem > 0)
            {
                SelectedItem--;
                CheckButtons();
            }
            return;
        }
        if (_angleDelta != 0) return;
        float startShiftAngle = _shiftAngle;
        Items[_firstVisibleItem - 1].FadeIn();
        Items[_firstVisibleItem + visibleItemsCount - 1].FadeOut();
        DOTween.To(() => _angleDelta, a => _angleDelta = a, _itemSize, time).OnUpdate(() =>
        {
            _shiftAngle = startShiftAngle - _angleDelta;
            Setup();
        }).OnComplete(() =>
        {
            _firstVisibleItem--;
            SelectedItem--;
            onComplete?.Invoke();
            if (_pages)
                _pages.GoToItem(_firstVisibleItem);
            CheckButtons();
            _angleDelta = 0;
        });

        if (_scaleAnimation)
        {
            for (int i = _firstVisibleItem - 1; i < _firstVisibleItem + visibleItemsCount - 1; i++)
            {
                Items[i].transform.DOScale(GetItemScale(i + 1), 0.2f);
            }
        }
    }

    Vector3 GetPosition(int index)
    {
        float theta = _shiftAngle - index * _itemSize;
        float x = radiusX * Mathf.Cos(theta);
        float y = radiusY * Mathf.Sin(theta);
        Vector3 p2 = _center + new Vector2(x, y);
        p2.z = transform.position.z;
        return p2;
    }

    float GetItemScale(float index)
    {
        float position = (index - _firstVisibleItem) / (float)(visibleItemsCount - 1);
        if (position < 0) position = 0;
        else if (position > 1) position = 1;
        float percentage = _scaleCurve.Evaluate(position);
        return percentage * _maxScale + (1 - percentage) * _minScale;
    }


    public void GoToItem(int index)
    {
        if (_angleDelta != 0) return;
        int selectedItem = index;
        if (selectedItem >= Items.Count) selectedItem = Items.Count - 1;
        float startShiftAngle = _shiftAngle;
        if (index > Items.Count - visibleItemsCount) index = Items.Count - visibleItemsCount;
        if (index < 0) index = 0;
        if (index == _firstVisibleItem)
        {
            SelectedItem = selectedItem;
            CheckButtons();
            return;
        }
        int delta = index - _firstVisibleItem;
        if (delta > 0)
        {
            float time = 0.2f / delta;
            float k = 0;
            for (int i = _firstVisibleItem; i < index; i++)
            {
                Items[i].FadeOut(time, k * time);
                Items[i + visibleItemsCount].FadeIn(time, k * time);
                k++;

                if (_scaleAnimation)
                {
                    Items[i].transform.DOScale(GetItemScale(-1), 0.2f);
                }
            }
            if (_scaleAnimation)
            {
                for (int i = index, j = 0; i < index + visibleItemsCount; i++, j++)
                {
                    Items[i].transform.DOScale(GetItemScale(_firstVisibleItem + j), 0.2f);
                }
            }
        }
        else
        {
            delta = -delta;
            float time = 0.2f / delta;
            float k = 0;
            for (int i = _firstVisibleItem; i > index; i--)
            {
                Items[i - 1].FadeIn(time, k * time);
                Items[i + visibleItemsCount - 1].FadeOut(time, k * time);
                if (_scaleAnimation)
                {
                    Items[i].transform.DOScale(GetItemScale(-1), 0.2f);
                }
                k++;
            }
            if (_scaleAnimation)
            {
                for (int i = index, j = 0; i < index + visibleItemsCount; i++, j++)
                {
                    Items[i].transform.DOScale(GetItemScale(_firstVisibleItem + j), 0.2f);
                }
            }
        }
        float deltaAngle = (index - _firstVisibleItem) * _itemSize;
        DOTween.To(() => _angleDelta, a => _angleDelta = a, deltaAngle, 0.2f).OnUpdate(() =>
        {
            _shiftAngle = startShiftAngle + _angleDelta;
            Setup();

        }).OnComplete(() =>
        {
            _firstVisibleItem = index;
            SelectedItem = selectedItem;
            CheckButtons();
            _angleDelta = 0;
        });
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Vector2 center = transform.TransformPoint(Center);
    //    Vector2 p1 = center + new Vector2(radiusX, 0);
    //    for (int theta = 10; theta <= 360; theta += 10)
    //    {
    //        float angle = Mathf.Deg2Rad * theta;
    //        float x = radiusX * Mathf.Cos(angle);
    //        float y = radiusY * Mathf.Sin(angle);

    //        Vector2 p2 = center + new Vector2(x, y);
    //        Gizmos.DrawLine(p1, p2);
    //        p1 = p2;
    //    }

    //    Gizmos.DrawSphere(transform.TransformPoint(Center), 0.1f);

    //    if (Application.isPlaying) return;
    //    Initialize();
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        Vector2 p2 = GetPosition(i);

    //        transform.GetChild(i).position = p2;
    //        transform.GetChild(i).up = (p2 - center).normalized;
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && test)
        {
            AddItem(true);
        }
    }
}
