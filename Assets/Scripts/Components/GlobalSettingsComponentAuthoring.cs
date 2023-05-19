using System;
using UnityEngine;
using Unity.Entities;

public class GlobalSettingsComponentAuthoring : MonoBehaviour {

    class Baker : Baker<GlobalSettingsComponentAuthoring> {
        public override void Bake(GlobalSettingsComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GlobalSettingsComponent {
            });
        }
    }
}

public struct GlobalSettingsComponent : IComponentData {
}