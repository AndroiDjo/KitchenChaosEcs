using Unity.Entities;

partial class StoveCounterStateSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<CanFryIngredientComponent>()
            .ForEach((ref EnableOnActionComponent enableOnAction, in IngredientEntityComponent ingredient) => {
                if (ingredient.Entity != Entity.Null && !enableOnAction.Enabled) {
                    enableOnAction.Enabled = true;
                    ecb.SetEnabled(enableOnAction.Entity, true);
                } else if (ingredient.Entity == Entity.Null && enableOnAction.Enabled) {
                    enableOnAction.Enabled = false;
                    ecb.SetEnabled(enableOnAction.Entity, false);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class StoveCounterSoundSystem : SystemBase {
    protected override void OnUpdate() {

        foreach (var (enableOnAction, audioSourceComponent) in SystemAPI
                     .Query<EnableOnActionComponent, GameObjectAudioSourceComponent>()
                     .WithAll<CanFryIngredientComponent>()
                ) {
            if (enableOnAction.Enabled) {
                audioSourceComponent.AudioSourceHolder.Play();
            }
            else {
                audioSourceComponent.AudioSourceHolder.Pause();
            }
        }
    }
}