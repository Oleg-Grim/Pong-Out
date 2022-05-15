using DG.Tweening;
using ME.ECS;
using UnityEngine;

public class WallsFadein : MonoBehaviour
{
    public GlobalEvent FadeWalls;
    [SerializeField] private Renderer[] _walls;

    private void Start()
    {
        FadeWalls.Subscribe(FadeIn);
        foreach (var wall in _walls)
        {
            wall.material.DOFade(0f, 0f);
        }
    }

    private void FadeIn(in Entity entity)
    {
        foreach (var wall in _walls)
        {
            wall.material.DOFade(1f, 0.5f).SetEase(Ease.InExpo);
        }
    }

    private void OnDestroy()
    {
        FadeWalls.Unsubscribe(FadeIn);
    }
}
