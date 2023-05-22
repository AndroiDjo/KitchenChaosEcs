using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        MainMenuScene,
        LoadingScene,
        GameScene
    }

    private static Scene targetScene;

    public static void Load(Scene scene) {
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadingCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
    
}