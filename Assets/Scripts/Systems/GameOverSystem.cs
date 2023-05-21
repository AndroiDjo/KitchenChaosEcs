using Unity.Collections;
using Unity.Entities;

partial class GameOverSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        NativeArray<int> successOrdersNative = new NativeArray<int>(1, Allocator.TempJob);

        Entities
            .ForEach((in CanDeliverMealsComponent canDeliverMeals) => {
                successOrdersNative[0] += canDeliverMeals.SuccessOrders;
            }).Schedule();
        
        Entities
            .WithDisposeOnCompletion(successOrdersNative)
            .WithNone<NeedShowGameOverUIComponent>()
            .WithAll<IsGameOverStateComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponent(entity, new NeedShowGameOverUIComponent{SuccessOrders = successOrdersNative[0]});
                ecb.SetComponentEnabled<NeedShowGameOverUIComponent>(entity, true);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class GameOverUISystem : SystemBase {
    protected override void OnUpdate() {
        foreach (var (isGameOver, entity) in SystemAPI
                     .Query<NeedShowGameOverUIComponent>()
                     .WithEntityAccess()
                     .WithNone<GameOverUIAreShownComponent>()
                 ) {
            GameOverUI.Instance.ShowScore(isGameOver.SuccessOrders);
            SystemAPI.SetComponentEnabled<NeedShowGameOverUIComponent>(entity, false);
            SystemAPI.SetComponentEnabled<GameOverUIAreShownComponent>(entity, true);
        }
    }
}