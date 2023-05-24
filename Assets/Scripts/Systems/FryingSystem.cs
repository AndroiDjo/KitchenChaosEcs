using Helpers;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

partial class FryingSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((Entity entity, ref FryCounterComponent fryCounter, ref ProgressBarValueComponent progressBar, 
                in IngredientNextStagePrefabComponent nextStagePrefab, in HoldedByComponent holdedBy,
                in Parent parentEntity, in LocalTransform localTransform) => {
                
                if (holdedBy.HolderType != HolderType.StoveCounter) {
                    return;
                }
        
                fryCounter.Counter += fryCounter.Speed * dt;
                progressBar.Value = fryCounter.Counter / fryCounter.Goal;
                if (fryCounter.Counter >= fryCounter.Goal) {
                    ecb.DestroyEntity(entity);
                    Entity nextStageEntity = ecb.Instantiate(nextStagePrefab.Prefab);
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, nextStageEntity, new ItemPlaceholderComponent {
                        Entity = parentEntity.Value,
                        LocalPosition = localTransform
                    }, true);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class FryingWarningSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        
        foreach (var (stepOverTime, progressBarValue, warningOnLimit, holdedBy, warningSignUI, localTransform) in SystemAPI
                     .Query<RefRW<StepOverTimeComponent>, ProgressBarValueComponent, WarningOnLimitComponent, 
                         HoldedByComponent,  GameObjectWarningSignUIComponent, LocalTransform>()
                     .WithAll<CanPlayWarningSoundComponent>()
                 ) {
            bool isWarning = holdedBy.HolderType == HolderType.StoveCounter &&
                             progressBarValue.Value >= warningOnLimit.WarningOnLimit;
            warningSignUI.WarningSignUI.SetWarning(isWarning);

            if (!isWarning) {
                return;
            }

            stepOverTime.ValueRW.Timer += dt;
            if (stepOverTime.ValueRO.Timer < stepOverTime.ValueRO.Goal) {
                return;
            }

            stepOverTime.ValueRW.Timer = 0f;
            SoundsManager.Instance.PlayWarningSound(localTransform.Position);
        }
    }
}