using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectScaler : MonoBehaviour
{
    public enum Orientation
    {
        Horizontal, Vertical        
    }

    public enum Position
    {
        Left, Right
    }
    
    private RectTransform _rect;

    [SerializeField] private RectTransform _parent;
    [SerializeField] private Orientation _orient;
    [SerializeField] private Position _position;
    [SerializeField] private float _scale;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        Rescale();
    }

    private void Rescale()
    {
        switch (_orient)
        {
            case Orientation.Horizontal:
            {
                var sizeDelta = _parent.sizeDelta;
                _rect.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.x) * _scale;

                switch (_position)
                {
                    case Position.Left:
                        _rect.anchoredPosition = new Vector2(0 + sizeDelta.x / 2, 0);
                        break;
                    case Position.Right:
                        _rect.anchoredPosition = new Vector2(sizeDelta.x - sizeDelta.x / 2, 0);
                        break;
                }
                break;
            }
            case Orientation.Vertical:
            {
                var sizeDelta = _parent.sizeDelta;
                _rect.sizeDelta = new Vector2(sizeDelta.y, sizeDelta.y) * _scale;

                switch (_position)
                {
                    case Position.Left:
                        _rect.anchoredPosition = new Vector2(-sizeDelta.x / 2 + _rect.sizeDelta.x /2, 0);
                        break;
                    case Position.Right:
                        _rect.anchoredPosition = new Vector2(sizeDelta.x/2 - _rect.sizeDelta.x / 2, 0);
                        break;
                }
                
                break;
            }
        }
    }
}
