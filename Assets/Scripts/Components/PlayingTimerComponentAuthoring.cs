using UnityEngine;
using Unity.Entities;

public class PlayingTimerComponentAuthoring : MonoBehaviour {
    public float Goal;

    class Baker : Baker<PlayingTimerComponentAuthoring> {
        public override void Bake(PlayingTimerComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayingTimerComponent {
                Goal = authoring.Goal
            });
        }
    }
}

public struct PlayingTimerComponent : IComponentData {
    public float Goal;
    public float Timer;
}
