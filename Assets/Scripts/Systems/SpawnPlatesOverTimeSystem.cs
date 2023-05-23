using Unity.Entities;
using Unity.Transforms;

partial class SpawnPlatesOverTimeSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithNone<IsSpawnPlatesRestricted>()
            .ForEach((Entity entity, 
            ref SpawnPlatesOverTimeComponent spawnOverTime, 
            in DynamicBuffer<ItemsBufferComponent> itemsBuffer,
            in SpawnPrefabComponent spawnPrefab,
            in ItemPlaceholderComponent itemPlaceholder
            ) => {
            if (itemsBuffer.Length >= spawnOverTime.ItemsLimit) {
                return;
            }

            spawnOverTime.Timer += dt;
            if (spawnOverTime.Timer < spawnOverTime.SpawnAfter) {
                return;
            }

            spawnOverTime.Timer = 0f;
            var newEntityPosition = itemPlaceholder.LocalPosition;
            newEntityPosition.Position.y += spawnPrefab.EntityHeight * itemsBuffer.Length;
            Entity spawnedItem = ecb.Instantiate(spawnPrefab.Prefab);
            ecb.AppendToBuffer(entity, new ItemsBufferComponent {Item = spawnedItem});
            ecb.AddComponent(spawnedItem, new Parent{ Value = itemPlaceholder.Entity });
            ecb.SetComponent(spawnedItem, newEntityPosition);
        }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}