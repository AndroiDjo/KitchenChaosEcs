using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SelectedItemInteractSystem))]
partial class CuttingSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithAll<TryToCutIngredientComponent>()
            .ForEach((Entity entity, ref CutCounterComponent cutCounter, in SlicedEntityPrefabComponent slicedPrefab, 
                in LastInteractedEntityComponent lastInteracted) => {
                
                ecb.SetComponentEnabled<TryToCutIngredientComponent>(entity, false);
                cutCounter.Counter++;
                if (cutCounter.Counter >= cutCounter.Goal) {
                    Entity slicedEntity = ecb.Instantiate(slicedPrefab.Prefab);
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(slicedEntity, true);
                    ecb.SetComponentEnabled<MustGrabIngredientComponent>(lastInteracted.Entity, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}