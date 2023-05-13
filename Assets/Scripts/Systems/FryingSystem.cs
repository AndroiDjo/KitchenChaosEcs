using Unity.Entities;

partial class FryingSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();
        
        Entities
            .ForEach((int entityInQueryIndex, ref FryCounterComponent fryCounter, ref ProgressBarValueComponent progressBar, 
                in IngredientNextStagePrefabComponent nextStagePrefab, in LastInteractedEntityComponent lastInteracted) => {
                if (!SystemAPI.HasComponent<CanFryIngredientComponent>(lastInteracted.Entity)) {
                    return;
                }

                fryCounter.Counter += fryCounter.Speed * dt;
                progressBar.Value = fryCounter.Counter / fryCounter.Goal;
                if (fryCounter.Counter >= fryCounter.Goal) {
                    Entity nextStageEntity = ecb.Instantiate(entityInQueryIndex, nextStagePrefab.Prefab);
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(entityInQueryIndex, nextStageEntity, true);
                    ecb.SetComponentEnabled<MustGrabIngredientComponent>(entityInQueryIndex, lastInteracted.Entity, true);
                }
            }).ScheduleParallel();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
