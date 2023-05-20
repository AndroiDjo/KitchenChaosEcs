using UnityEngine;
using Unity.Entities;

public class StepOverTimeComponentAuthoring : MonoBehaviour {
    public float Goal;

    class Baker : Baker<StepOverTimeComponentAuthoring> {
        public override void Bake(StepOverTimeComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new StepOverTimeComponent { Goal = authoring.Goal });
        }
    }
}

public struct StepOverTimeComponent : IComponentData {
    public float Goal;
    public float Timer;
}