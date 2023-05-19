using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class GameObjectVisualComponentAuthoring : MonoBehaviour {
    public GameObject VisualPrefab;

    class Baker : Baker<GameObjectVisualComponentAuthoring> {
        public override void Bake(GameObjectVisualComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new GameObjectVisualComponent {
                Prefab = authoring.VisualPrefab
            });
        }
    }
}

public class GameObjectVisualComponent : IComponentData {
    public GameObject Prefab;
}

public class GameObjectTransformComponent : ICleanupComponentData {
    public Transform Transform;
}

public class GameObjectBindingComponent : ICleanupComponentData {
    public GameObject GameObject;
}

public class GameObjectAnimatorComponent : IComponentData {
    public Animator Animator;
}

public class GameObjectProgressBarComponent : IComponentData {
    public GameObject ProgressBarGO;
    public Image Image;
}

public class GameObjectIngredientIconsUIComponent : IComponentData {
    public IngredientIconsUI IngredientIconsUI;
    public const int INGREDIENT_ICONS_LIMIT = 9;
}
