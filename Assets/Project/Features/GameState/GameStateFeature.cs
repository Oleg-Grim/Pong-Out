using ME.ECS;
using Project.Features.GameState.Systems;

namespace Project.Features
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class GameStateFeature : Feature
    {
        public GlobalEvent PlayersReady;
        public GlobalEvent EndGame;
        protected override void OnConstruct()
        {
            AddSystem<StartGameSystem>();
            AddSystem<RunGameSystem>();
        }

        protected override void OnDeconstruct() {}
    }
}