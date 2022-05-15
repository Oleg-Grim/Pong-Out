using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AScaler : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private bool _runtime;
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _size;
    private void Awake()
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
        float hFraction = (float)Screen.width / 100;
        float vFraction = (float)Screen.height / 100;

        _rectTransform.anchoredPosition = new Vector2(hFraction * _position.x , vFraction * _position.y);
        _rectTransform.sizeDelta = new Vector2(hFraction * _size.x, vFraction * _size.y);
    }
}
