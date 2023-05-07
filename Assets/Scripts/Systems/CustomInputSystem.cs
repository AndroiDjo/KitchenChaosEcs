using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
partial struct CustomInputSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        float2 currentInput = PlayerInputBuffer.Instance.GetMoveInput();

        if (currentInput.Equals(float2.zero)) {
            foreach (var (input, entity) in SystemAPI.Query<InputComponent>().WithEntityAccess()) {
                SystemAPI.SetComponentEnabled<InputComponent>(entity, false);
            }
        }
        else {
            foreach (var (input, entity) in SystemAPI.Query<RefRW<InputComponent>>()
                         .WithEntityAccess()
                         .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                     ) {
                if (!SystemAPI.IsComponentEnabled<InputComponent>(entity)) {
                    SystemAPI.SetComponentEnabled<InputComponent>(entity, true);
                }
                input.ValueRW.MoveInput = currentInput;
            }
        }
    }
}