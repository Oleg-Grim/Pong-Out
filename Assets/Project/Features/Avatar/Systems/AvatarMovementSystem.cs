using ME.ECS;
using Project.Components;
using UnityEngine;

namespace Project.Features.Avatar.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class AvatarMovementSystem : ISystemFilter
    {
        public World world { get; set; }
        
        private AvatarFeature _feature;

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
            return Filter.Create("Filter-AvatarMovementSystem")
                .With<AvatarTag>()
                .With<MoveInput>()
                .WithoutShared<Components.GamePaused>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var id = ref entity.Read<Owner>().Value.Read<PlayerTag>().Value;
            var direction = id == 1 ? 1 : -1;
            
            ref readonly var speed = ref entity.Read<MoveSpeed>().Value;
            ref readonly var width = ref entity.Read<PadWidth>().Value;
            
            ref var input = ref entity.Get<MoveInput>().Value;
            input = Mathf.Clamp(input, -1, 1);

            var pos = entity.GetLocalPosition();
            var newZ = Mathf.Clamp(pos.z, -7f + width, 7f - width);
            var tmpPos = new Vector3(pos.x, pos.y, newZ);
            
            var target = new Vector3(0, 0, input * direction);
            var newPos = tmpPos + target;
            
            if (input != 0)
            {
                entity.SetLocalPosition(Vector3.MoveTowards(pos, newPos, speed * deltaTime));
            }
        }
    }
}