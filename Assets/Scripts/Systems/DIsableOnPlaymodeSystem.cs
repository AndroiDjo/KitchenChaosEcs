using Unity.Entities;

partial class DisableOnPlaymode : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();
        Entities
            .WithAll<DisableOnPlaymodeComponent>()
            .ForEach((Entity entity, int entityInQueryIndex) => {
                ecb.SetEnabled(entityInQueryIndex, entity, false);
            }).ScheduleParallel();
    }
}