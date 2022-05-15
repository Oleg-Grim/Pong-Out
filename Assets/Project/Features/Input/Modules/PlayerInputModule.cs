using ME.ECS;
using Photon.Pun;
using Project.Markers;
using UnityEngine;

namespace Project.Features.Input.Modules
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif

    public sealed class PlayerInputModule : IModule, IUpdate
    {
        public World world { get; set; }
        
        private InputFeature _feature;

        void IModuleBase.OnConstruct()
        {
            _feature = world.GetFeature<InputFeature>();
        }

        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                world.AddMarker(new LeftKeyMarker {PlayerID = PhotonNetwork.LocalPlayer.ActorNumber, State = KeyState.Pressed});
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.A))
            {
                world.AddMarker(new LeftKeyMarker {PlayerID = PhotonNetwork.LocalPlayer.ActorNumber, State = KeyState.Released});
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                world.AddMarker(new RightKeyMarker {PlayerID = PhotonNetwork.LocalPlayer.ActorNumber, State = KeyState.Pressed});
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.D))
            {
                world.AddMarker(new RightKeyMarker {PlayerID = PhotonNetwork.LocalPlayer.ActorNumber, State = KeyState.Released});
            }
        }
    }
}