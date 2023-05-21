using UnityEngine;
using Unity.Entities;

public class CanWaitingToStartComponentAuthoring : MonoBehaviour {
    public float Goal;
    
    class Baker : Baker<CanWaitingToStartComponentAuthoring> {
        public override void Bake(CanWaitingToStartComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new WaitingToStartComponent{ Goal = authoring.Goal });
        }
    }
}

public struct WaitingToStartComponent : IComponentData {
    public float Goal;
    public float Timer;
}