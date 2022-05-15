using ME.ECS;
using Project.Components;
using UnityEngine;

namespace Project.Views
{
    using ME.ECS.Views.Providers;

    public class PlayerMono : MonoBehaviourView
    {
        public override bool applyStateJob => true;
        public override void OnInitialize() {}
        public override void OnDeInitialize() {}
        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately) {}
        public override void ApplyState(float deltaTime, bool immediately)
        {
            transform.position = entity.GetPosition();

            var width = entity.Read<PadWidth>().Value;
            transform.localScale = new Vector3(1f, 1f, width);
        }
    }
}