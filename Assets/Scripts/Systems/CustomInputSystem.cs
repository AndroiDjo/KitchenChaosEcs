using Unity.Entities;
using Unity.Mathematics;

partial class CustomInputSystem : SystemBase {
    protected override void OnUpdate() {
        float2 currentInput = PlayerInputBuffer.Instance.GetMoveInput();
        Entities
            .ForEach((ref InputComponent input) => {
                input.MoveInput = currentInput;
            }).Schedule();
    }
}