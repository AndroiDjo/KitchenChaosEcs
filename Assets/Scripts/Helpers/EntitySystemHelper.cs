using Unity.Entities;
using Unity.Transforms;

namespace Helpers {
    public class EntitySystemHelper {
        public static void SetNewParentToEntity(ref EntityCommandBuffer ecb, in Entity entity, 
            in ItemPlaceholderComponent itemPlaceholder, in bool newEntity) {
            Parent newParent = new Parent { Value = itemPlaceholder.Entity };
            if (newEntity) {
                ecb.AddComponent(entity, newParent);
            }
            else {
                ecb.SetComponent(entity, newParent);
            }
            ecb.SetComponent(entity, itemPlaceholder.LocalPosition);
        }
    }
}