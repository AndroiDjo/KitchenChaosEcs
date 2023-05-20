using Unity.Entities;
using Unity.Mathematics;

partial class WalkingEffectsSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        float dt = SystemAPI.Time.DeltaTime;

        Entities
            .WithAll<CanHaveWalkingAnimationComponent, PlayerIsWalkingComponent>()
            .ForEach((Entity entity, in InputMoveComponent input) => {
                if (input.Value.Equals(float2.zero)) {
                    ecb.SetComponentEnabled<PlayerIsWalkingComponent>(entity, false);
                }
            }).Schedule();
        
        Entities
            .WithAll<CanHaveWalkingAnimationComponent>()
            .WithNone<PlayerIsWalkingComponent>()
            .ForEach((Entity entity, in InputMoveComponent input) => {
                if (!input.Value.Equals(float2.zero)) {
                    ecb.SetComponentEnabled<PlayerIsWalkingComponent>(entity, true);
                }
            }).Schedule();
        
        Entities
            .WithAll<PlayerIsWalkingComponent, CanPlayWalkingSoundComponent>()
            .ForEach((Entity entity, ref StepOverTimeComponent actionOverTime) => {
                actionOverTime.Timer += dt;
                if (actionOverTime.Timer < actionOverTime.Goal) {
                    return;
                }

                actionOverTime.Timer = 0f;
                ecb.SetComponentEnabled<PlayerIsWalkingSoundComponent>(entity, true);
            }).Schedule();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}