
namespace ME.ECS.BlackBox {

    [Category("Entity/Views")]
    public class InstantiateParticlesView : BoxNode {

        public ME.ECS.Views.Providers.ParticleViewSourceBase viewSource;
        private ViewId viewId;
        
        public override void OnCreate() {

            this.viewId = Worlds.currentWorld.RegisterViewSource(this.viewSource);

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            entity.InstantiateView(this.viewId);
            
            return this.next;
            
        }

    }

}