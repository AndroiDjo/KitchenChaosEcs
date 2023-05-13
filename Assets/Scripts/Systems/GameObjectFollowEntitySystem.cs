using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct GameObjectFollowEntitySystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach (var (goTransform, entityTransform, localTransform) in SystemAPI.Query<GameObjectTransformComponent, LocalToWorld, LocalTransform>()) {
            goTransform.Transform.position = entityTransform.Position;
            goTransform.Transform.rotation = entityTransform.Rotation;
            goTransform.Transform.localScale = new Vector3(localTransform.Scale, localTransform.Scale, localTransform.Scale);
        }
    }
}