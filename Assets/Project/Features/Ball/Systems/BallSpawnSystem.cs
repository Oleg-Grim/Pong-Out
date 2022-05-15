﻿using ME.ECS;
using Project.Components;

namespace Project.Features.Ball.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class BallSpawnSystem : ISystemFilter
    {
        public World world { get; set; }
        
        private BallFeature _feature;
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _feature);
        }
        void ISystemBase.OnDeconstruct() {}
#if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
#endif
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter()
        {
            return Filter.Create("Filter-BallSpawnSystem")
                .With<PlayerTag>()
                .With<IsNeutral>()
                .Without<BallAvatar>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref var time = ref entity.Get<BallSpawnTime>().Value;
            ref readonly var timeDefault = ref entity.Read<TimerDefault>().Value;
            time -= deltaTime;
            
            
            if (time <= 0)
            {
                entity.Get<BallAvatar>().Value = _feature.CreateBall();
                time = timeDefault;
            }
        }
    }
}