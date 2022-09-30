using UnityEngine;

public class LevelData : LevelDataBase
{
    protected override void OnLevelLoadingComplete()
    {
        Debug.Log($"Level {LevelIndex + 1} Loading Complete!");
    }
}
