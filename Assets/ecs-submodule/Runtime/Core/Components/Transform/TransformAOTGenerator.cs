﻿
namespace ME.ECS {

    public static class TransformAOTCompileHelper {
    
        public static void IL2CPP() {
    
            new ME.ECS.StructComponents<ME.ECS.Transform.Nodes>();
            new ME.ECS.StructComponents<ME.ECS.Transform.Container>();

        }
    
    }

}
