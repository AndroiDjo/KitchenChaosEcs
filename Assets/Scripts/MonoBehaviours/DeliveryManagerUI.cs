using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private DeliveryManagerElement templateElement;
    
    public static DeliveryManagerUI Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public GameObject CreateElement(string recipeName, IngredientType[] ingredientTypes) {
        GameObject spawned = GameObject.Instantiate(templateElement.gameObject, container);
        DeliveryManagerElement deliveryManager = spawned.GetComponent<DeliveryManagerElement>();
        deliveryManager.SetText(recipeName);
        deliveryManager.SetIcons(ingredientTypes);
        
        return spawned;
    }
}
