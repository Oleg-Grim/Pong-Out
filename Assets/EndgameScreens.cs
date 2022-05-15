using System;
using DG.Tweening;
using ME.ECS;
using Photon.Pun;
using Project.Features;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndgameScreens : MonoBehaviour
{
    public GlobalEvent EndGame;

    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _loseText;
    [SerializeField] private Image _background;
    
    [SerializeField] private Color[] _colors;

    private void Start()
    {
        EndGame.Subscribe(ShowEndScreen);
        _background.DOFade(0, 0f);
        _winText.DOFade(0, 0f);
        _loseText.DOFade(0,0f);
    }

    private void ShowEndScreen(in Entity entity)
    {
        var local = Worlds.current.GetFeature<AvatarFeature>().GetPlayerByID(PhotonNetwork.LocalPlayer.ActorNumber);
        _background.DOFade(1, 0.5f);
        if (local == entity)
        {
            _winText.color = _colors[PhotonNetwork.LocalPlayer.ActorNumber];
            _winText.DOFade(1, 2f).SetEase(Ease.OutBounce);
        }
        else
        {
            _loseText.color = _colors[PhotonNetwork.LocalPlayer.ActorNumber];
            _loseText.DOFade(1, 2f).SetEase(Ease.OutBounce);
        }
    }

    private void OnDestroy()
    {
        EndGame.Unsubscribe(ShowEndScreen);
    }
}
