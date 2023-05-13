using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class GameObjectVisualComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject VisualPrefab;

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

public class GameObjectTransformComponent : IComponentData {
    public Transform Transform;
}

public class GameObjectAnimatorComponent : IComponentData {
    public Animator Animator;
}

public class GameObjectProgressBarComponent : IComponentData {
    public GameObject ProgressBarGO;
    public Image Image;
}