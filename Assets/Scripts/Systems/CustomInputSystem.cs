using System;
using Unity.Entities;
using Unity.Mathematics;

partial class CustomInputSystem : SystemBase {

    protected override void OnCreate() {
        PlayerInputBuffer.Instance.OnInteractAction += InstanceOnOnInteractAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        foreach (var (_, entity) in SystemAPI.Query<CanHaveInputInteractComponent>().WithNone<InputInteractComponent>().WithEntityAccess()) {
            SystemAPI.SetComponentEnabled<InputInteractComponent>(entity, true);
        }
    }

    protected override void OnUpdate() {
        float2 currentInput = PlayerInputBuffer.Instance.GetMoveInput();
        Entities
            .ForEach((ref InputMoveComponent input) => {
                input.Value = currentInput;
            }).Schedule();
    }
}