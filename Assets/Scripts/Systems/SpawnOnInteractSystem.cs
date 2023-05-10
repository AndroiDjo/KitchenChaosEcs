using System;
using Unity.Entities;

partial class SpawnOnInteractSystem : SystemBase {
    protected override void OnCreate() {
        PlayerInputBuffer.Instance.OnInteractAction += InstanceOnOnInteractAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithAll<IsSelectedItemComponent>()
            .ForEach((Entity entity, in SpawnPrefabComponent spawnPrefab, in SpawnPositionComponent spawnPosition) => {
                Entity spawnedEntity = ecb.Instantiate(spawnPrefab.Prefab);
                ecb.SetComponent(spawnedEntity, spawnPosition.Value);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}