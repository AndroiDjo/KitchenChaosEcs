using Unity.Entities;
using UnityEngine;

public class MoveSpeedComponentAuthoring : MonoBehaviour {
    [SerializeField]
    private float MoveSpeed;

    class Baker : Baker<MoveSpeedComponentAuthoring> {
        public override void Bake(MoveSpeedComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveSpeedComponent{ Value = authoring.MoveSpeed});
        }
    }
}
public struct MoveSpeedComponent : IComponentData {
    public float Value;
}

