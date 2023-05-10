using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Helpers;

partial class PlayerMoveSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((ref LocalTransform transform, ref LastInputDirectionComponent lastInputDirection, 
                in InputMoveComponent input, in MoveSpeedComponent moveSpeed, in PhysicsCollider collider, in CollisionToleranceComponent collisionTolerance) => {
            if (!InputHelper.HasMovement(input))
                return;
            
            lastInputDirection.Value = input.Value;
            
            float3 moveDirection = InputHelper.GetMoveDirection(input.Value);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider, collisionTolerance)) {
                return;
            }

            if (input.Value.x.Equals(0f) || input.Value.y.Equals(0f)) {
                return;
            }
            
            // If we are moving diagonally, lets try to move in one direction.
            moveDirection = new float3(input.Value.x, 0f, 0f);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider, collisionTolerance)) {
                return;
            }
            
            moveDirection = new float3(0f, 0f, input.Value.y);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider, collisionTolerance)) {
                return;
            }
            
            }).Schedule();
    }

    private static bool CheckForCollisionAndMove(ref LocalTransform transform,PhysicsWorldSingleton physicsWorld, in float dt, in float3 moveDirection, 
        in MoveSpeedComponent moveSpeed, in PhysicsCollider collider, in CollisionToleranceComponent collisionTolerance) {
        float3 moveStep = moveDirection * moveSpeed.Value * dt;
        float3 checkPosition = transform.Position + moveStep * collisionTolerance.Value;
            
        ColliderCastInput colliderInput = new ColliderCastInput(collider.Value, checkPosition,
            checkPosition, transform.Rotation, 1.0f);
        bool isHit = physicsWorld.CastCollider(colliderInput);
        if (!isHit) {
            transform.Position += moveStep;
        }

        return isHit;
    }
}