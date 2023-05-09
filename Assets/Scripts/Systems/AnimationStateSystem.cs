using Unity.Entities;
using Unity.Mathematics;

partial class AnimationStateSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<CanHaveWalkingAnimationComponent, PlayerAnimationIsWalkingComponent>()
            .ForEach((Entity entity, in InputComponent input) => {
                if (input.MoveInput.Equals(float2.zero)) {
                    ecb.SetComponentEnabled<PlayerAnimationIsWalkingComponent>(entity, false);
                }
            }).Schedule();
        
        Entities
            .WithAll<CanHaveWalkingAnimationComponent>()
            .WithNone<PlayerAnimationIsWalkingComponent>()
            .ForEach((Entity entity, in InputComponent input) => {
                if (!input.MoveInput.Equals(float2.zero)) {
                    ecb.SetComponentEnabled<PlayerAnimationIsWalkingComponent>(entity, true);
                }
            }).Schedule();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}