using System;
using Unity.Entities;
using Unity.Transforms;

partial class GrabOnInteractSystem : SystemBase {
    protected override void OnCreate() {
        PlayerInputBuffer.Instance.OnInteractAction += InstanceOnOnInteractAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        Entities
            .WithAll<IsSelectedItemComponent>()
            .ForEach((Entity entity, ref IngredientEntityComponent ingredientEntity, in SpawnPrefabComponent ingredientPrefab, 
                in ItemPlaceholderComponent itemPlaceholder, in InteractedPlayerItemPlaceholderComponent playerItemPlaceholder) => {
                Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                ingredientEntity.Entity = spawnedEntity;
                ecb.SetComponent(entity, ingredientEntity);
                ecb.AddComponent(ingredientEntity.Entity, new Parent {
                    Value = playerItemPlaceholder.Placeholder.Entity
                });
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}