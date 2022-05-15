using System;
using ME.ECS;
using Photon.Pun;
using Project.Components;
using Project.Features;
using TMPro;
using UnityEngine;

public class ScoreCounters : MonoBehaviour
{
    public TextMeshProUGUI PingScore;
    public TextMeshProUGUI PongScore;

    public GlobalEvent TimeSynced;
    public GlobalEvent PlayerScored;

    private Entity _ping;
    private Entity _pong;

    private void Start()
    {
        TimeSynced.Subscribe(SetPlayer);
        PlayerScored.Subscribe(UpdateScore);
    }

    private void SetPlayer(in Entity entity)
    {
        _ping = Worlds.current.GetFeature<AvatarFeature>().GetPlayerByID(1);
        _pong = Worlds.current.GetFeature<AvatarFeature>().GetPlayerByID(2);
    }

    private void UpdateScore(in Entity entity)
    {
        PingScore.SetText(_ping.Read<PlayerScore>().Value.ToString("00"));
        PongScore.SetText(_pong.Read<PlayerScore>().Value.ToString("00"));
    }

    private void OnDestroy()
    {
        TimeSynced.Unsubscribe(SetPlayer);
        PlayerScored.Unsubscribe(UpdateScore);
    }
}
