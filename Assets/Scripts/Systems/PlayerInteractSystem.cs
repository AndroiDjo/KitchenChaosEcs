using Helpers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;

partial class PlayerInteractSystem : SystemBase {
    private const float INTERACT_DISTANCE = 1f;
    
    protected override void OnUpdate() {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((Entity entity, ref PlayerInteractTargetComponent interactTarget, in LocalTransform transform,
                in LastInputDirectionComponent lastDirection, in PhysicsCollider collider) => {
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
                if (physicsWorld.CastRay(raycastInput, out RaycastHit raycastHit)) {
                    interactTarget.TargetEntity = raycastHit.Entity;
                }
                else {
                    interactTarget.TargetEntity = Entity.Null;
                }

            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}