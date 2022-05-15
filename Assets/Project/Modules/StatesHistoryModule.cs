namespace Project.Modules {

    using TState = ProjectState;
    
    /// <summary>
    /// We need to implement our own StatesHistoryModule class without any logic just to catch your State type into ECS.StatesHistory
    /// You can use some overrides to setup history config for your project
    /// </summary>
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TState> {

        private uint ticks;
        
        protected override uint GetQueueCapacity() {

            // Here you can set up history states capacity
            return 10u;

        }

        protected override uint GetTicksPerState() {

            // Every N ticks network module should clone current world's state 
            return 20u;

        }

        public void SetTicksForInput(uint ticks) {

            this.ticks = ticks;

        }
        
        protected override uint GetTicksForInput() {

            return this.ticks;

        }

    }

}