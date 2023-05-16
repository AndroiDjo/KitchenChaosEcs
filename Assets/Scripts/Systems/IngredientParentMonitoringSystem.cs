using Unity.Entities;
using Unity.Transforms;

partial class IngredientParentMonitoringSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();
        var childBufferLookup = SystemAPI.GetBufferLookup<Child>(true);
        var ingredientTypeLookup = SystemAPI.GetComponentLookup<IngredientTypeComponent>(true);
        
        Entities
            .WithReadOnly(childBufferLookup)
            .WithReadOnly(ingredientTypeLookup)
            .ForEach((int entityInQueryIndex, ref IngredientEntityComponent ingredient, 
                in ItemPlaceholderComponent itemPlaceholder, in CanHoldIngredientComponent canHold) => {

                if (!childBufferLookup.TryGetBuffer(itemPlaceholder.Entity, out DynamicBuffer<Child> childBuffer)) {
                    ingredient = new IngredientEntityComponent();
                    return;
                }

                if (childBuffer.Length == 0) {
                    ingredient = new IngredientEntityComponent();
                    return;
                }
                
                foreach (var child in childBuffer) {
                    if (!ingredientTypeLookup.TryGetComponent(child.Value, out IngredientTypeComponent ingredientType)) {
                        continue;
                    }

                    ingredient = new IngredientEntityComponent {
                        Entity = child.Value,
                        IngredientType = ingredientType
                    };
                    ecb.AddComponent(entityInQueryIndex, child.Value, new HoldedByComponent { HolderType = canHold.HolderType });
                }
            })
            .ScheduleParallel();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}