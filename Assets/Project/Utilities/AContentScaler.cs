using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AContentScaler : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private RectTransform _parentRect;
    [SerializeField] private bool _runtime;
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _size;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchorMin = Vector2.zero;
        _rectTransform.anchorMax = Vector2.zero;
        
        Rescale();
    }
    private void Update()
    {
        if (_runtime)
        {
            Rescale();
        }
    }

    private void Rescale()
    {
        float hFraction = _parentRect.rect.width / 100f;
        float vFraction = _parentRect.rect.height / 100f;

        _rectTransform.anchoredPosition = new Vector2(hFraction * _position.x , vFraction * _position.y);
        _rectTransform.sizeDelta = new Vector2(hFraction * _size.x, vFraction * _size.y);
    }
}
