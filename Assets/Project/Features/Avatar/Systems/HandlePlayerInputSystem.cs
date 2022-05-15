using System;
using ExitGames.Client.Photon.StructWrapping;
using ME.ECS;
using Photon.Pun;
using Project.Components;
using Project.Markers;
using Project.Modules;

namespace Project.Features.Avatar.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class HandlePlayerInputSystem : ISystem, IUpdate
    {
        public World world { get; set; }
        
        private AvatarFeature _feature;
        private RPCId _left, _right;
        
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _feature);
            RegisterRPSs();
        }

        void ISystemBase.OnDeconstruct() {}

        private void RegisterRPSs()
        {
            var net = world.GetModule<NetworkModule>();
            net.RegisterObject(this);

            _left = net.RegisterRPC(new Action<LeftKeyMarker>(LeftKey_RPC).Method);
            _right = net.RegisterRPC(new Action<RightKeyMarker>(RightKey_RPC).Method);
        }
        
        void IUpdate.Update(in float deltaTime)
        {
            if (world.GetMarker(out LeftKeyMarker lkm))
            {
                var net = world.GetModule<NetworkModule>();
                net.RPC(this, _left, lkm);
            }
            
            if (world.GetMarker(out RightKeyMarker rkm))
            {
                var net = world.GetModule<NetworkModule>();
                net.RPC(this, _right, rkm);
            }
        }

        private void LeftKey_RPC(LeftKeyMarker lkm)
        {
            var player = world.GetFeature<AvatarFeature>().GetPlayerByID(lkm.PlayerID);
            if(!player.IsAlive()) return;
            
            switch (lkm.State)
            {
                case KeyState.Pressed:
                    player.Get<PlayerAvatar>().Value.Get<MoveInput>().Value += 1f;
                    break;
                case KeyState.Released:
                    player.Get<PlayerAvatar>().Value.Get<MoveInput>().Value -= 1f;
                    break;
            }
        }
        
        private void RightKey_RPC(RightKeyMarker rkm)
        {
            var player = world.GetFeature<AvatarFeature>().GetPlayerByID(rkm.PlayerID);
            if(!player.IsAlive()) return;
        
            switch (rkm.State)
            {
                case KeyState.Pressed:
                    player.Get<PlayerAvatar>().Value.Get<MoveInput>().Value -= 1f;
                    break;
                case KeyState.Released:
                    player.Get<PlayerAvatar>().Value.Get<MoveInput>().Value += 1f;
                    break;
            }
        }
    }
}