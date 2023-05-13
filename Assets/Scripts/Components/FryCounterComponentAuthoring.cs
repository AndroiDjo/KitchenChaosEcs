using UnityEngine;
using Unity.Entities;

public class FryCounterComponentAuthoring : MonoBehaviour {
    public float Goal;
    public float Speed;
    class Baker : Baker<FryCounterComponentAuthoring> {
        public override void Bake(FryCounterComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new FryCounterComponent {
                Goal = authoring.Goal,
                Speed = authoring.Speed
            });
        }
    }
}

public struct FryCounterComponent : IComponentData {
    public float Counter;
    public float Goal;
    public float Speed;
}