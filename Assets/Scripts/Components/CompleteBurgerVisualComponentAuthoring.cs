using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CompleteBurgerVisualComponentAuthoring : MonoBehaviour {
    [Serializable]
    public struct CompleteBurgerVisual {
        public GameObject Visual;
        public IngredientType IngredientType;
    }

    public List<CompleteBurgerVisual> CompleteBurgerVisuals;
    
    class Baker : Baker<CompleteBurgerVisualComponentAuthoring> {
        public override void Bake(CompleteBurgerVisualComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddBuffer<CompleteBurgerVisualBufferComponent>(entity);
            foreach (CompleteBurgerVisual burgerVisual in authoring.CompleteBurgerVisuals) {
                AppendToBuffer(entity, new CompleteBurgerVisualBufferComponent {
                    Visual = GetEntity(burgerVisual.Visual, TransformUsageFlags.None),
                    IngredientType = burgerVisual.IngredientType
                });
            }
        }
    }
}

public struct CompleteBurgerVisualBufferComponent : IBufferElementData {
    public Entity Visual;
    public IngredientType IngredientType;
}