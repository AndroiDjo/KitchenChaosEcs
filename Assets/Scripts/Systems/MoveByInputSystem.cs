using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial class MoveByInputSystem : SystemBase {
    protected override void OnUpdate() {
        var dt = SystemAPI.Time.DeltaTime;
        
        Entities.ForEach((ref LocalTransform transform, in InputComponent input, in MoveSpeedComponent moveSpeed) => {
            float3 moveDirection = new float3(input.MoveInput.x, 0f, input.MoveInput.y);
            transform.Position += moveDirection * moveSpeed.Value * dt;
        }).Schedule();
    }
}