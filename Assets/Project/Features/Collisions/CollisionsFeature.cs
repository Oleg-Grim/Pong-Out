using ME.ECS;
using Project.Features.Collisions.Systems;

namespace Project.Features
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class CollisionsFeature : Feature
    {
        protected override void OnConstruct()
        {
            AddSystem<CollisionDetectionSystem>();
        }

        protected override void OnDeconstruct() {}
    }
}