using ME.ECS;
using ME.ECS.DataConfigs;
using ME.ECS.Views.Providers;
using Project.Components;
using Project.Features.Initialization.Modules;
using Project.Markers;
using Project.Modules;
using UnityEngine;

namespace Project.Features
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class InitializationFeature : Feature
    {
        public GlobalEvent TimeSynced;

        public DataConfig[] WallConfigs;
        public DataConfig NeutralConfig;

        private RPCId _onPlayerInit, _onTimeSynced;

        protected override void OnConstruct()
        {
            AddModule<InitConnectionModule>();

            var net = world.GetModule<NetworkModule>();
            net.RegisterObject(this);
            
            world.SetSharedData(new GamePaused());
            
            _onPlayerInit = net.RegisterRPC(new System.Action<int>(InitPlayer_RPC).Method);
            _onTimeSynced = net.RegisterRPC(new System.Action<int>(TimeSynced_RPC).Method);
        }

        protected override void OnConstructLate()
        {
            ConstructWalls();
            CreateNeutralPlayer();
        }

        protected override void OnDeconstruct() {}

        private void ConstructWalls()
        {
            for (int i = 0; i < 4; i++)
            {
                var wall = new Entity("Wall");
                WallConfigs[i].Apply(wall);
            }
        }

        private void CreateNeutralPlayer()
        {
            var entity = new Entity("neutralPlayer");
            NeutralConfig.Apply(entity);
        }
        
        public void OnInitPlayer(NetworkSetActivePlayer connectionMarker)
        {
            var net = world.GetModule<NetworkModule>();
            net.RPC(this, _onPlayerInit, connectionMarker.PlayerID);
        }

        private void InitPlayer_RPC(int id)
        {
            var player = new Entity("Player");
            player.Set(new PlayerTag {Value = id});
            player.Set(new PlayerScore());
        }

        public void OnTimeSynced(int id)
        {
            var net = world.GetModule<NetworkModule>();
            net.RPC(this, _onTimeSynced, id);
        }

        private void TimeSynced_RPC(int id)
        {
            Debug.Log("time synced");
            TimeSynced.Execute();
            world.RemoveSharedData<GamePaused>();
        }
    }
}