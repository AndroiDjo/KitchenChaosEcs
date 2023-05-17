using Unity.Entities;

partial class UpdateIconsUISystem : SystemBase {
    protected override void OnUpdate() {
        foreach (var (iconsUIComponent, burgerIngredientsBuffer, entity) in SystemAPI
                     .Query<GameObjectIngredientIconsUIComponent, DynamicBuffer<BurgerIngredientsBufferComponent>>()
                     .WithAll<NeedUpdateUIComponent>()
                     .WithEntityAccess()
                 ) {
            iconsUIComponent.IngredientIconsUI.UpdateIcons(burgerIngredientsBuffer);
            SystemAPI.SetComponentEnabled<NeedUpdateUIComponent>(entity, false);
        }
    }
}