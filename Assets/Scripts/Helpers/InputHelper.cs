using Unity.Mathematics;

namespace Helpers {
    public class InputHelper {
        public static float3 GetMoveDirection(in float2 inputVector) {
            return new float3(inputVector.x, 0f, inputVector.y);
        }
        
        public static bool HasMovement(in InputMoveComponent inputMove) {
            return !inputMove.Value.Equals(float2.zero);
        }
    }
}