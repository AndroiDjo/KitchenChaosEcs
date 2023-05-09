using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial class MoveByInputSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        NativeArray<bool> isMoved = new NativeArray<bool>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((ref LocalTransform transform, in InputComponent input, in MoveSpeedComponent moveSpeed) => {
            if (input.MoveInput.Equals(float2.zero))
                return;

            isMoved[0] = true;
            float3 moveDirection = new float3(input.MoveInput.x, 0f, input.MoveInput.y);
            transform.Position += moveDirection * moveSpeed.Value * dt;
        }).Schedule();

        Entities
            .WithAll<PlayerTagComponent, PlayerAnimationIsWalkingComponent>()
            .ForEach((Entity entity) => {
                if (!isMoved[0]) {
                    ecb.SetComponentEnabled<PlayerAnimationIsWalkingComponent>(entity, false);
                }
        }).Schedule();
        
        Entities
            .WithAll<PlayerTagComponent>()
            .WithNone<PlayerAnimationIsWalkingComponent>()
            .WithDisposeOnCompletion(isMoved)
            .ForEach((Entity entity) => {
                if (isMoved[0]) {
                    ecb.SetComponentEnabled<PlayerAnimationIsWalkingComponent>(entity, true);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}