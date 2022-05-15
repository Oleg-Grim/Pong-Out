using ME.ECS;
using Project.Components;

namespace Project.Features.GameState.Systems
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class RunGameSystem : ISystemFilter
    {
        public World world { get; set; }
        
        private GameStateFeature _feature;
        private Filter _ballFilter;
        
        void ISystemBase.OnConstruct()
        {
            this.GetFeature(out _feature);

            Filter.Create("Ball-Filter")
                .With<BallTag>()
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
            return Filter.Create("Filter-RunGameSystem")
                .With<PlayerScore>()
                .Push();
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime)
        {
            ref readonly var score = ref entity.Read<PlayerScore>().Value;

            if (score >= 5)
            {
                world.SetSharedData(new GamePaused());
                _feature.EndGame.Execute(entity);
            }
        }
    }
}