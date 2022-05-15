using ME.ECS;
using Project.Components;
using UnityEngine;

namespace Project.Features.GameState.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class StartGameSystem : ISystemFilter
    {
        public World world { get; set; }
        
        private GameStateFeature _feature;
        private AvatarFeature _avatar;
        
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _feature);
            world.GetFeature(out _avatar);
        }
        void ISystemBase.OnDeconstruct() {}
#if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
#endif
        Filter ISystemFilter.filter { get; set; }

        Filter ISystemFilter.CreateFilter()
        {
            return Filter.Create("Filter-StartGameSystem")
                .With<PlayerTag>()
                .With<IsNeutral>()
                .WithoutShared<GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var time = ref entity.Get<GameStartTime>().Value;
            ref readonly var timeDefault = ref entity.Read<TimerDefault>().Value;
            time -= deltaTime;

            if (time <= timeDefault /2 && !entity.Has<HalfTime>())
            {
                _avatar.PassLocalPlayer();
                entity.Set(new HalfTime());
            }

            if (time <= 0)
            {
                _feature.PlayersReady.Execute();
                world.RemoveSharedData<GamePaused>();
                entity.Remove<HalfTime>();
            }
        }
    }
}