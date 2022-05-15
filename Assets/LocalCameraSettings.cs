using Cinemachine;
using ME.ECS;
using Photon.Pun;
using Project.Components;
using Project.Features;
using UnityEngine;

public class LocalCameraSettings : MonoBehaviour
{
    [SerializeField] private GlobalEvent _onPassLocalPlayer;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;

    private void Start()
    {
        _onPassLocalPlayer.Subscribe(SetCameraPosition);
    }

    private void SetCameraPosition(in Entity entity)
    {
        if(entity != Worlds.current.GetFeature<AvatarFeature>().GetPlayerByID(PhotonNetwork.LocalPlayer.ActorNumber)) return;

        var id = entity.Read<PlayerTag>().Value;
        
        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].m_Priority = id == i ? 1 : 0;
        }
    }

    private void OnDestroy()
    {
        _onPassLocalPlayer.Unsubscribe(SetCameraPosition);
    }
}
