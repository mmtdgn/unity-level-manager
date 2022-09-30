using UnityEngine;

public class GameManager : GameManagerBase
{
    protected override void OnAwake()
    {
        Debug.Log("GameManager.OnAwake");
    }

    protected override void OnLevelStart(LevelDataBase levelData, int levelIndex)
    {
        Debug.Log($"Level {levelIndex + 1} Start!");
    }

    protected override void OnLevelComplete(int levelIndex, int score)
    {
        Debug.Log($"Level {levelIndex + 1} Completed!\n Score={score}");
    }

    protected override void OnLevelFail(int levelIndex)
    {
        Debug.Log($"Level {levelIndex + 1} failed!");
    }
#if SCENE_MODE
    protected override void OnReturnMainMenu()
    {
        Debug.Log("Return Main Menu");
    }
#endif
}
