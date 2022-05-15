using UnityEngine;
using DG.Tweening;
public class MaterialTiling : MonoBehaviour
{
    private Renderer _rend;
    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        StartScroll();
    }

    private void StartScroll()
    {
        _rend.material.DOOffset(new Vector2(0, -2), 20f).OnComplete(ResetMaterial).SetEase(Ease.Linear);
    }

    private void ResetMaterial()
    {
        _rend.material.DOOffset(Vector2.zero, 0f).OnComplete(StartScroll);
    }
}
