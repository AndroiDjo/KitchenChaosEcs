using Helpers;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial class RotateByInputSystem : SystemBase {
    protected override void OnUpdate() {
        var dt = SystemAPI.Time.DeltaTime;
        
        Entities.ForEach((ref LocalTransform transform, in InputMoveComponent input, in RotateSpeedComponent rotateSpeed) => {
            if (!InputHelper.HasMovement(input))
                return;
            
            var targetRotation = quaternion.RotateY(math.atan2(input.Value.x, input.Value.y));
            transform.Rotation = math.slerp(transform.Rotation, targetRotation, rotateSpeed.Value * dt);
        }).Schedule();
    }
}