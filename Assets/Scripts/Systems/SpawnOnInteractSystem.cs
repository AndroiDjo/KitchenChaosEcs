using System;
using Unity.Entities;
using Unity.Transforms;

partial class SpawnOnInteractSystem : SystemBase {
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
                if (ingredientEntity.Entity.Equals(Entity.Null)) {
                    Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                    ecb.AddComponent(spawnedEntity, new Parent {
                        Value = itemPlaceholder.Entity
                    });
                    ingredientEntity.Entity = spawnedEntity;
                    ecb.SetComponent(entity, ingredientEntity);
                    return;
                }

                if (playerItemPlaceholder.Placeholder.Entity.Equals(Entity.Null))
                    return;

                ecb.SetComponent(ingredientEntity.Entity, new Parent {
                    Value = playerItemPlaceholder.Placeholder.Entity
                });
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}