using UnityEngine;
using Unity.Entities;

public class CanGenerateOrdersComponentAuthoring : MonoBehaviour {
    public int OrdersLimit;
    public float GenerateDelay;
    class Baker : Baker<CanGenerateOrdersComponentAuthoring> {
        public override void Bake(CanGenerateOrdersComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CanGenerateOrdersComponent {
                OrdersLimit = authoring.OrdersLimit,
                GenerateDelay = authoring.GenerateDelay
            });
        }
    }
}

public struct CanGenerateOrdersComponent : IComponentData {
    public int OrdersLimit;
    public float GenerateDelay;
    public float Timer;
}