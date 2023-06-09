using UnityEngine;
using Unity.Entities;

public class GameStateComponentAuthoring : MonoBehaviour {

    class Baker : Baker<GameStateComponentAuthoring> {
        public override void Bake(GameStateComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GameStateComponent {
                GameState = GameState.WaitingToStart
            });
            AddComponent<GameScoreComponent>(entity);
        }
    }
}

public struct GameStateComponent : IComponentData {
    public GameState GameState;

    public bool IsGameActive() {
        switch (GameState) {
            case GameState.GamePlaying:
                return true;
        }

        return false;
    }
}

public enum GameState {
    None,
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver
}

public struct GameScoreComponent : IComponentData {
    public int Score;
}
