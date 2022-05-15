namespace ME.ECS {

    public static partial class ComponentsInitializer {

        static partial void InitTypeIdPartial() {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.AvatarTag>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallAvatar>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallDirection>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallLaunched>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallLaunchTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallRadius>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallSpawnTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallTag>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.CollisionRect>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.CurrentPlayer>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Despawn>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GamePaused>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameStartTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.HalfTime>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.IsNeutral>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.MoveInput>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.MoveSpeed>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Normal>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Owner>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PadWidth>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerAvatar>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerScore>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerTag>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.RunGame>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.ScoreWall>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.StartGame>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.TimerDefault>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.WallTag>(true, true, false, false, false, false, false, false);

        }

        static partial void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer, ref ME.ECS.StructComponentsContainer noStateStructComponentsContainer) {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.AvatarTag>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallAvatar>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallDirection>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallLaunched>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallLaunchTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallRadius>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallSpawnTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.BallTag>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.CollisionRect>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.CurrentPlayer>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Despawn>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GamePaused>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameStartTime>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.HalfTime>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.IsNeutral>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.MoveInput>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.MoveSpeed>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Normal>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.Owner>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PadWidth>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerAvatar>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerScore>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.PlayerTag>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.RunGame>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.ScoreWall>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.StartGame>(true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.TimerDefault>(false, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.WallTag>(true, true, false, false, false, false, false, false);

            ComponentsInitializerWorld.Setup(ComponentsInitializerWorldGen.Init);
            CoreComponentsInitializer.Init(ref structComponentsContainer);


            structComponentsContainer.Validate<Project.Components.AvatarTag>(true);
            structComponentsContainer.Validate<Project.Components.BallAvatar>(false);
            structComponentsContainer.Validate<Project.Components.BallDirection>(false);
            structComponentsContainer.Validate<Project.Components.BallLaunched>(true);
            structComponentsContainer.Validate<Project.Components.BallLaunchTime>(false);
            structComponentsContainer.Validate<Project.Components.BallRadius>(false);
            structComponentsContainer.Validate<Project.Components.BallSpawnTime>(false);
            structComponentsContainer.Validate<Project.Components.BallTag>(true);
            structComponentsContainer.Validate<Project.Components.CollisionRect>(false);
            structComponentsContainer.Validate<Project.Components.CurrentPlayer>(false);
            structComponentsContainer.Validate<Project.Components.Despawn>(true);
            structComponentsContainer.Validate<Project.Components.GamePaused>(true);
            structComponentsContainer.Validate<Project.Components.GameStartTime>(false);
            structComponentsContainer.Validate<Project.Components.HalfTime>(true);
            structComponentsContainer.Validate<Project.Components.IsNeutral>(true);
            structComponentsContainer.Validate<Project.Components.MoveInput>(false);
            structComponentsContainer.Validate<Project.Components.MoveSpeed>(false);
            structComponentsContainer.Validate<Project.Components.Normal>(false);
            structComponentsContainer.Validate<Project.Components.Owner>(false);
            structComponentsContainer.Validate<Project.Components.PadWidth>(false);
            structComponentsContainer.Validate<Project.Components.PlayerAvatar>(false);
            structComponentsContainer.Validate<Project.Components.PlayerScore>(false);
            structComponentsContainer.Validate<Project.Components.PlayerTag>(false);
            structComponentsContainer.Validate<Project.Components.RunGame>(true);
            structComponentsContainer.Validate<Project.Components.ScoreWall>(false);
            structComponentsContainer.Validate<Project.Components.StartGame>(true);
            structComponentsContainer.Validate<Project.Components.TimerDefault>(false);
            structComponentsContainer.Validate<Project.Components.WallTag>(true);

        }

    }

    public static class ComponentsInitializerWorldGen {

        public static void Init(Entity entity) {


            entity.ValidateData<Project.Components.AvatarTag>(true);
            entity.ValidateData<Project.Components.BallAvatar>(false);
            entity.ValidateData<Project.Components.BallDirection>(false);
            entity.ValidateData<Project.Components.BallLaunched>(true);
            entity.ValidateData<Project.Components.BallLaunchTime>(false);
            entity.ValidateData<Project.Components.BallRadius>(false);
            entity.ValidateData<Project.Components.BallSpawnTime>(false);
            entity.ValidateData<Project.Components.BallTag>(true);
            entity.ValidateData<Project.Components.CollisionRect>(false);
            entity.ValidateData<Project.Components.CurrentPlayer>(false);
            entity.ValidateData<Project.Components.Despawn>(true);
            entity.ValidateData<Project.Components.GamePaused>(true);
            entity.ValidateData<Project.Components.GameStartTime>(false);
            entity.ValidateData<Project.Components.HalfTime>(true);
            entity.ValidateData<Project.Components.IsNeutral>(true);
            entity.ValidateData<Project.Components.MoveInput>(false);
            entity.ValidateData<Project.Components.MoveSpeed>(false);
            entity.ValidateData<Project.Components.Normal>(false);
            entity.ValidateData<Project.Components.Owner>(false);
            entity.ValidateData<Project.Components.PadWidth>(false);
            entity.ValidateData<Project.Components.PlayerAvatar>(false);
            entity.ValidateData<Project.Components.PlayerScore>(false);
            entity.ValidateData<Project.Components.PlayerTag>(false);
            entity.ValidateData<Project.Components.RunGame>(true);
            entity.ValidateData<Project.Components.ScoreWall>(false);
            entity.ValidateData<Project.Components.StartGame>(true);
            entity.ValidateData<Project.Components.TimerDefault>(false);
            entity.ValidateData<Project.Components.WallTag>(true);

        }

    }

}
