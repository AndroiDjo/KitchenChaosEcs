using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

partial class PlayerMoveSystem : SystemBase {
    private const float COLLISION_TOLERANCE = 3f;
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((ref LocalTransform transform, in InputComponent input, in MoveSpeedComponent moveSpeed, in PhysicsCollider collider) => {
            if (input.MoveInput.Equals(float2.zero))
                return;
            
            float3 moveDirection = new float3(input.MoveInput.x, 0f, input.MoveInput.y);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider)) {
                return;
            }

            if (input.MoveInput.x.Equals(0f) || input.MoveInput.y.Equals(0f)) {
                return;
            }
            
            // If we are moving diagonally, lets try to move in one direction.
            moveDirection = new float3(input.MoveInput.x, 0f, 0f);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider)) {
                return;
            }
            
            moveDirection = new float3(0f, 0f, input.MoveInput.y);
            if (!CheckForCollisionAndMove(ref transform, physicsWorld, dt, moveDirection, moveSpeed, collider)) {
                return;
            }
            
            }).Schedule();
    }

    private static bool CheckForCollisionAndMove(ref LocalTransform transform,PhysicsWorldSingleton physicsWorld, in float dt, in float3 moveDirection, 
        in MoveSpeedComponent moveSpeed, in PhysicsCollider collider) {
        float3 moveStep = moveDirection * moveSpeed.Value * dt;
        float3 checkPosition = transform.Position + moveStep * COLLISION_TOLERANCE;
            
        ColliderCastInput colliderInput = new ColliderCastInput(collider.Value, checkPosition,
            checkPosition, transform.Rotation, 1.0f);
        bool isHit = physicsWorld.CastCollider(colliderInput);
        if (!isHit) {
            transform.Position += moveStep;
        }

        return isHit;
    }
}