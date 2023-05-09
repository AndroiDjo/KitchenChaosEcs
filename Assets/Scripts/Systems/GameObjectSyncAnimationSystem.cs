using Unity.Burst;
using Unity.Entities;

[BurstCompile]
partial struct GameObjectSyncAnimationSystem : ISystem {
    private const string IS_WALKING = "IsWalking";
    
    public void OnUpdate(ref SystemState state) {
        foreach (var animator in SystemAPI.Query<GameObjectAnimatorComponent>()
                     .WithAll<PlayerAnimationIsWalkingComponent, PlayerTagComponent>()) {
            if (!animator.Animator.GetBool(IS_WALKING)) {
                animator.Animator.SetBool(IS_WALKING, true);
            }
        }
        
        foreach (var animator in SystemAPI.Query<GameObjectAnimatorComponent>()
                     .WithAll<PlayerTagComponent>()
                     .WithNone<PlayerAnimationIsWalkingComponent>())
        {
            if (animator.Animator.GetBool(IS_WALKING)) {
                animator.Animator.SetBool(IS_WALKING, false);
            }
        }
    }
}