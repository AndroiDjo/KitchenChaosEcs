using Unity.Entities;

partial class GameOverUISystem : SystemBase {
    protected override void OnUpdate() {
        foreach (var (gameScore, gameState) in SystemAPI
                     .Query<GameScoreComponent, GameStateComponent>()
                 ) {
            if (gameState.GameState != GameState.GameOver) {
                GameOverUI.Instance.Hide();
            }
            else {
                GameOverUI.Instance.ShowScore(gameScore.Score);
            }
        }
    }
}