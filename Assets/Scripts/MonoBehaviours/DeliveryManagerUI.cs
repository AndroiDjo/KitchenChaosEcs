using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private DeliveryManagerElement templateElement;
    
    private static DeliveryManagerUI _instance;

    public static DeliveryManagerUI Instance {
        get
        {
            if (_instance == null) {
                _instance = FindObjectOfType<DeliveryManagerUI>();
            }

            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }

    public GameObject CreateElement(string recipeName, IngredientType[] ingredientTypes) {
        GameObject spawned = GameObject.Instantiate(templateElement.gameObject, container);
        DeliveryManagerElement deliveryManager = spawned.GetComponent<DeliveryManagerElement>();
        deliveryManager.SetText(recipeName);
        deliveryManager.SetIcons(ingredientTypes);
        
        return spawned;
    }
}
