using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SelectedItemInteractSystem))]
partial class CuttingSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithAll<TryToCutIngredientComponent>()
            .ForEach((Entity entity, ref CutCounterComponent cutCounter, ref ProgressBarValueComponent progressBar,
                in IngredientNextStagePrefabComponent nextStagePrefab, in LastInteractedEntityComponent lastInteracted) => {
                
                ecb.SetComponentEnabled<TryToCutIngredientComponent>(entity, false);
                cutCounter.Counter++;
                progressBar.Value = (float)cutCounter.Counter / cutCounter.Goal;
                if (cutCounter.Counter >= cutCounter.Goal) {
                    Entity nextStageEntity = ecb.Instantiate(nextStagePrefab.Prefab);
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(nextStageEntity, true);
                    ecb.SetComponentEnabled<MustGrabIngredientComponent>(lastInteracted.Entity, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}