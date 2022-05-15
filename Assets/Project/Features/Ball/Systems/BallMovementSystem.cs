using ME.ECS;
using Project.Components;
using UnityEngine;

namespace Project.Features.Ball.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class BallMovementSystem : ISystemFilter
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
            return Filter.Create("Filter-BallMovementSystem")
                .With<BallTag>()
                .With<BallDirection>()
                .With<MoveSpeed>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var speed = ref entity.Read<MoveSpeed>().Value;
            ref readonly var dir = ref entity.Read<BallDirection>().Value;
            var pos = entity.GetPosition();

            var newPos = pos + dir.normalized * speed * deltaTime;
            entity.SetPosition(newPos);
        }
    }
}