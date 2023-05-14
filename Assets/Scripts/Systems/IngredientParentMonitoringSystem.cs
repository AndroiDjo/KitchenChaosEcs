using Unity.Entities;
using Unity.Transforms;

partial class IngredientParentMonitoringSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((ref IngredientEntityComponent ingredient,
                in ItemPlaceholderComponent itemPlaceholder, in CanHoldIngredientComponent canHold) => {
                if (!SystemAPI.HasBuffer<Child>(itemPlaceholder.Entity)) {
                    ingredient = new IngredientEntityComponent();
                    return;
                }

                var childBuffer = SystemAPI.GetBuffer<Child>(itemPlaceholder.Entity);
                if (childBuffer.Length == 0) {
                    ingredient = new IngredientEntityComponent();
                }
                else {
                    foreach (var child in childBuffer) {
                        ingredient.Entity = child.Value;
                        ecb.AddComponent(child.Value, new HoldedByComponent { HolderType = canHold.HolderType });
                    }
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}