using System;
using ME.ECS;
using ME.ECS.DataConfigs;
using ME.ECS.Views.Providers;
using Project.Components;
using Project.Features.Avatar.Systems;
using UnityEngine;

namespace Project.Features
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class AvatarFeature : Feature
    {
        public MonoBehaviourViewBase PingView;
        public MonoBehaviourViewBase PongView;
        
        public GlobalEvent PassPlayer;

        private ViewId _ping, _pong;
        private Filter _playerFilter;

        protected override void OnConstruct()
        {
            AddSystem<AvatarSpawnSystem>();
            AddSystem<HandlePlayerInputSystem>();
            AddSystem<AvatarMovementSystem>();

            Filter.Create("Player-Filter")
                .With<PlayerTag>()
                .Push(ref _playerFilter);
            
            _ping = world.RegisterViewSource(PingView);
            _pong = world.RegisterViewSource(PongView);
        }

        protected override void OnDeconstruct() {}

        public void SpawnAvatar(Entity owner)
        {
            var avatar = new Entity("Player-Avatar");
            avatar.Set(new AvatarTag());
            
            avatar.Get<Owner>().Value = owner;
            owner.Get<PlayerAvatar>().Value = avatar;

            avatar.Get<MoveInput>().Value = 0;
            avatar.Get<MoveSpeed>().Value = 4;
            avatar.Get<PadWidth>().Value = 2;
            avatar.Get<CollisionRect>().Value.x = 0.25f;
            avatar.Get<CollisionRect>().Value.y = avatar.Read<PadWidth>().Value / 2;
            avatar.Get<Normal>().Value = owner.Read<PlayerTag>().Value == 1? Vector3.right :Vector3.left;
            avatar.InstantiateView(owner.Read<PlayerTag>().Value == 1 ? _ping : _pong);

            var startPos = owner.Read<PlayerTag>().Value == 1 ? new Vector3(-12f, 0f, 0f) : new Vector3(12f, 0f, 0f);
            avatar.SetPosition(startPos);
        }

        public Entity GetPlayerByID(int id)
        {
            foreach (var player in _playerFilter)
            {
                if (player.Read<PlayerTag>().Value == id)
                {
                    return player;
                }
            }

            Debug.LogWarning("you are trying to find entity that does not exist");
            return Entity.Empty;
        }

        public void PassLocalPlayer()
        {
            foreach (var entity in _playerFilter)
            {
                PassPlayer.Execute(entity);
            }
        }
    }
}