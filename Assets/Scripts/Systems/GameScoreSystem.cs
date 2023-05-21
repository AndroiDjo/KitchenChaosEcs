using Unity.Collections;
using Unity.Entities;

partial class GameScoreSystem : SystemBase {
    protected override void OnUpdate() {
        
        NativeArray<int> successOrdersNative = new NativeArray<int>(1, Allocator.TempJob);

        Entities
            .ForEach((in CanDeliverMealsComponent canDeliverMeals) => {
                successOrdersNative[0] += canDeliverMeals.SuccessOrders;
            }).Schedule();
        
        Entities
            .WithDisposeOnCompletion(successOrdersNative)
            .ForEach((ref GameScoreComponent gameScore) => {
                gameScore.Score = successOrdersNative[0];
            }).Schedule();
    }
}