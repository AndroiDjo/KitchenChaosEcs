using UnityEngine;
using Unity.Entities;

public class CutCounterComponentAuthoring : MonoBehaviour {
    public int Goal;
    class Baker : Baker<CutCounterComponentAuthoring> {
        public override void Bake(CutCounterComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CutCounterComponent {
                Goal = authoring.Goal
            });
        }
    }
}

public struct CutCounterComponent : IComponentData {
    public int Counter;
    public int Goal;
}