using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private GameObject templateElement;
    
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

    public GameObject CreateElement(string recipeName) {
        Debug.Log("recipe for "+ recipeName);
        GameObject spawned = GameObject.Instantiate(templateElement, container);
        if (spawned.TryGetComponent<DeliveryManagerElement>(out DeliveryManagerElement deliveryManagerElement)) {
            deliveryManagerElement.SetText(recipeName);
        }

        return spawned;
    }
}
