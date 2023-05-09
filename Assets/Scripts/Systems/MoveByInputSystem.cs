using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial class MoveByInputSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;

        Entities
            .ForEach((ref LocalTransform transform, in InputComponent input, in MoveSpeedComponent moveSpeed) => {
            if (input.MoveInput.Equals(float2.zero))
                return;

            float3 moveDirection = new float3(input.MoveInput.x, 0f, input.MoveInput.y);
            transform.Position += moveDirection * moveSpeed.Value * dt;
        }).Schedule();
    }
}