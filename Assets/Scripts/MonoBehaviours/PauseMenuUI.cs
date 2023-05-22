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
        
        resumeButton.onClick.AddListener(OnClickResume);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
    }

    private void OnClickMainMenu() {
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void OnClickResume() {
        pauseGameSystem.TogglePause();
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
