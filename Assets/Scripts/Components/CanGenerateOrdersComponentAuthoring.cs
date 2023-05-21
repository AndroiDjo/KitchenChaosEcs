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
            AddComponent<IsGenerateOrdersRestrictedComponent>(entity);
            SetComponentEnabled<IsGenerateOrdersRestrictedComponent>(entity, false);
        }
    }
}

public struct CanGenerateOrdersComponent : IComponentData {
    public int OrdersLimit;
    public float GenerateDelay;
    public float Timer;
}

public struct IsGenerateOrdersRestrictedComponent : IComponentData, IEnableableComponent {}