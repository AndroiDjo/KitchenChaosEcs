using System;
using UnityEngine;
using Unity.Entities;

public class GameStateComponentAuthoring : MonoBehaviour {

    class Baker : Baker<GameStateComponentAuthoring> {
        public override void Bake(GameStateComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameStateComponent {
                GameState = GameState.WaitingToStart,
                PreviousState = GameState.None
            });
            AddComponent<IsStateChangedComponent>(entity);
        }
    }
}

public struct GameStateComponent : IComponentData {
    public GameState GameState;
    public GameState PreviousState;

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

public struct IsStateChangedComponent: IComponentData, IEnableableComponent {}