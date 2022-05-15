using System;
using ME.ECS;
using Project.Components;
using UnityEngine;

namespace Project.Features.Collisions.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class CollisionDetectionSystem : ISystemFilter
    {
        public World world { get; set; }
        
        private CollisionsFeature _feature;
        private Filter _ballFilter;
        
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _feature);
            Filter.Create("BallCollision-Filter")
                .With<BallTag>()
                .Without<Despawn>()
                .Push(ref _ballFilter);
        }
        void ISystemBase.OnDeconstruct() {}
#if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
#endif
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter()
        {
            return Filter.Create("Filter-CollisionDetectionSystem")
                .With<CollisionRect>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var rect = ref entity.Read<CollisionRect>().Value;
            var position = entity.GetPosition();
            
            foreach (var ball in _ballFilter)
            {
                ref readonly var radius = ref ball.Read<BallRadius>().Value;
                var ballPosition = ball.GetPosition();

                ref readonly var normal = ref entity.Read<Normal>().Value;
                ref var direction = ref ball.Get<BallDirection>().Value;
                ref var speed = ref ball.Get<MoveSpeed>().Value;
                var velocity = direction * speed * deltaTime;
            
                var box = new Vector3(Mathf.Max(position.x - rect.x, Mathf.Min(position.x + rect.x, ballPosition.x)), 
                    0, Mathf.Max(position.z - rect.y, Mathf.Min(position.z + rect.y, ballPosition.z)));
            
                if ((box - (ballPosition + velocity)).sqrMagnitude <= radius * radius)
                {
                    var finPos = entity.GetPosition() - entity.Read<BallDirection>().Value * (entity.GetPosition() - box).magnitude;

                    entity.SetPosition(finPos);
                    direction = Vector3.Reflect(direction, normal);
                    speed += 0.05f;

                    if (entity.Has<ScoreWall>())
                    {
                        ball.Remove<MoveSpeed>();
                        ball.Get<CurrentPlayer>().Value = entity.Read<ScoreWall>().Value;
                        ball.Set(new Despawn());
                        return;
                    }
                }
            }
        }
    }
}