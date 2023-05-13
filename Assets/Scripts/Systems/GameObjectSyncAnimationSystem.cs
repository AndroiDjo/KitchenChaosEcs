using Unity.Burst;
using Unity.Entities;

[BurstCompile]
partial struct GameObjectSyncAnimationSystem : ISystem {
    private const string IS_WALKING = "IsWalking";
    private const string OPEN_CLOSE = "OpenClose";
    private const string CUT = "Cut";
    
    public void OnUpdate(ref SystemState state) {
        foreach (var animator in SystemAPI.Query<GameObjectAnimatorComponent>()
                     .WithAll<CanHaveWalkingAnimationComponent, PlayerAnimationIsWalkingComponent>()) {
            if (!animator.Animator.GetBool(IS_WALKING)) {
                animator.Animator.SetBool(IS_WALKING, true);
            }
        }
        
        foreach (var animator in SystemAPI.Query<GameObjectAnimatorComponent>()
                     .WithAll<CanHaveWalkingAnimationComponent>()
                     .WithNone<PlayerAnimationIsWalkingComponent>()
                 ) {
            if (animator.Animator.GetBool(IS_WALKING)) {
                animator.Animator.SetBool(IS_WALKING, false);
            }
        }
        
        foreach (var (animator, entity) in SystemAPI.Query<GameObjectAnimatorComponent>().WithEntityAccess()
                     .WithAll<CanHaveIsOpenAnimationComponent, IsOpenAnimationComponent>()) {
            SystemAPI.SetComponentEnabled<IsOpenAnimationComponent>(entity, false);
            animator.Animator.SetBool(OPEN_CLOSE, true);
        }
        
        foreach (var (animator, entity) in SystemAPI.Query<GameObjectAnimatorComponent>().WithEntityAccess()
                     .WithAll<CanHaveIsCuttingAnimationComponent, IsCuttingAnimationComponent>()) {
            SystemAPI.SetComponentEnabled<IsCuttingAnimationComponent>(entity, false);
            animator.Animator.SetBool(CUT, true);
        }
    }
}