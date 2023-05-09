using Helpers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;

partial class PlayerInteractSystem : SystemBase {
    private const float INTERACT_DISTANCE = 1f;
    
    protected override void OnUpdate() {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((in LocalTransform transform, in LastInputDirectionComponent lastDirection,
                in PhysicsCollider collider) => {
                if (lastDirection.Value.Equals(float2.zero))
                    return;

                float3 startPoint = transform.Position;
                float3 lookPoint = transform.Position +
                                   InputHelper.GetMoveDirection(lastDirection.Value) * INTERACT_DISTANCE;
                CollisionFilter playerCollisionFilter = collider.Value.Value.GetCollisionFilter();
                RaycastInput raycastInput = new RaycastInput {
                    Start = startPoint,
                    End = lookPoint,
                    Filter = playerCollisionFilter
                };
                PhysicsDebugDisplaySystem.Line(raycastInput.Start, raycastInput.End, Unity.DebugDisplay.ColorIndex.Red);
                if (physicsWorld.CastRay(raycastInput, out RaycastHit raycastHit)) {
                    //Debug.Log(raycastHit.Position);
                }

            }).Schedule();
    }
}