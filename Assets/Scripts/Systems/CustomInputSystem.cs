using Unity.Entities;
using Unity.Mathematics;

partial class CustomInputSystem : SystemBase {
    protected override void OnUpdate() {
        float2 currentInput = PlayerInputBuffer.Instance.GetMoveInput();
        if (currentInput.Equals(float2.zero)) {
            foreach (var (input, entity) in SystemAPI.Query<InputComponent>().WithEntityAccess()) {
                SystemAPI.SetComponentEnabled<InputComponent>(entity, false);
            }
        }
        else {
            foreach (var (input, entity) in SystemAPI.Query<InputComponent>()
                         .WithEntityAccess()
                         .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                     ) {
                if (!SystemAPI.IsComponentEnabled<InputComponent>(entity)) {
                    SystemAPI.SetComponentEnabled<InputComponent>(entity, true);
                }
            }
            
            Entities
                .ForEach((ref InputComponent input) => {
                    input.MoveInput = currentInput;
                }).Schedule();
        }
    }
}