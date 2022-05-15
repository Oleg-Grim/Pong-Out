using ME.ECS;
using Photon.Pun;
using Project.Markers;

namespace Project.Features.Initialization.Modules
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif

    public sealed class InitConnectionModule : IModule, IUpdate
    {
        private InitializationFeature _feature;

        public World world { get; set; }

        void IModuleBase.OnConstruct()
        {
            _feature = world.GetFeature<InitializationFeature>();
        }

        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime)
        {
            if (world.GetMarker(out NetworkSetActivePlayer connectionMarker))
            {
                _feature.OnInitPlayer(connectionMarker);
            }

            if (world.GetMarker(out NetworkPlayerConnectedTimeSynced syncedMarker))
            {
                _feature.OnTimeSynced(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }
}