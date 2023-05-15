using Unity.Entities;

partial class EnableOnActionSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<CanFryIngredientComponent>()
            .ForEach((in EnableOnActionComponent enableOnAction, in IngredientEntityComponent ingredient) => {
                bool isEntityDisabled = SystemAPI.HasComponent<Disabled>(enableOnAction.Entity);
                if (ingredient.Entity != Entity.Null && isEntityDisabled) {
                    ecb.SetEnabled(enableOnAction.Entity, true);
                } else if (ingredient.Entity == Entity.Null && !isEntityDisabled) {
                    ecb.SetEnabled(enableOnAction.Entity, false);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}