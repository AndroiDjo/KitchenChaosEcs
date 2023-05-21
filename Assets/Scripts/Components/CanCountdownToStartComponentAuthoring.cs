using UnityEngine;
using Unity.Entities;

public class CanCountdownToStartComponentAuthoring : MonoBehaviour {
    public float Goal;
    
    class Baker : Baker<CanCountdownToStartComponentAuthoring> {
        public override void Bake(CanCountdownToStartComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CountdownToStartComponent{ Goal = authoring.Goal });
        }
    }
}
public struct CountdownToStartComponent : IComponentData {
    public float Goal;
    public float Timer;
}
