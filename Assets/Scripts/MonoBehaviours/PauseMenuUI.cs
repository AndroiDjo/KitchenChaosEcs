using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour {
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    public static PauseMenuUI Instance { get; private set; }
    private PauseGameSystem pauseGameSystem;
    
    private void Awake() {
        Instance = this;
        pauseGameSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PauseGameSystem>();
        
        resumeButton.onClick.AddListener((() => {
            pauseGameSystem.TogglePause();
        }));
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            OptionsMenuUI.Instance.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
