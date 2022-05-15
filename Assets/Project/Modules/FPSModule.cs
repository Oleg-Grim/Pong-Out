#if FPS_MODULE_SUPPORT
namespace Project.Modules {
    
    using TState = ProjectState;
    
    /// <summary>
    /// We need to implement our own FPSModule class without any logic just to catch your State type into ECS.FPSModule
    /// You can use some overrides to setup FPS config for your project
    /// </summary>
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class FPSModule : ME.ECS.FPSModule<TState> {
        
    }
    
}
#endif