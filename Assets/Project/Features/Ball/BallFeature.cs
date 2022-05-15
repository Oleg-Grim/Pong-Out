using ME.ECS;
using ME.ECS.Views.Providers;
using Project.Components;
using Project.Features.Ball.Systems;
using UnityEngine;

namespace Project.Features
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class BallFeature : Feature
    {
        public GlobalEvent PlayerScored;
        public MonoBehaviourViewBase BallView;
        private ViewId _ballID;
        private AvatarFeature _avatar;

        protected override void OnConstruct()
        {
            AddSystem<BallDespawnSystem>();
            AddSystem<BallMovementSystem>();
            AddSystem<BallSpawnSystem>();
            AddSystem<BallLaunchSystem>();
            
            world.GetFeature(out _avatar);
            
            _ballID = world.RegisterViewSource(BallView);
        }

        protected override void OnDeconstruct() {}

        protected override void InjectFilter(ref FilterBuilder builder)
        {
            builder.WithoutShared<GamePaused>();
        }

        public Entity CreateBall()
        {
            var ball = new Entity("ball");
            ball.Set(new BallTag());
            ball.Get<Owner>().Value = _avatar.GetPlayerByID(0);

            ball.Get<BallDirection>().Value = new Vector3(world.GetRandomRange(-1f, 1f), 0f, world.GetRandomRange(-1f, 1f));
            
            ball.Get<MoveSpeed>().Value = 0f;
            ball.Get<BallRadius>().Value = 0.25f;
            
            ball.SetPosition(Vector3.zero);
            ball.InstantiateView(_ballID);

            return ball;
        }
    }
}