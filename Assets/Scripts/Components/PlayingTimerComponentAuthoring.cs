using UnityEngine;
using Unity.Entities;

public class PlayingTimerComponentAuthoring : MonoBehaviour {
    public float Goal;

    class Baker : Baker<PlayingTimerComponentAuthoring> {
        public override void Bake(PlayingTimerComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHavePlayingTimerComponent>(entity);
            AddComponent(entity, new PlayingTimerComponent {
                Goal = authoring.Goal
            });
            SetComponentEnabled<PlayingTimerComponent>(entity, false);
        }
    }
}

public struct CanHavePlayingTimerComponent : IComponentData {}
public struct PlayingTimerComponent : IComponentData, IEnableableComponent {
    public float Goal;
    public float Timer;
}
