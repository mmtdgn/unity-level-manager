using System.Collections.Generic;
using Tymski;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "MD/LevelContainer", order = 0)]
public class LevelContainer : ScriptableObject
{
    public LoopMode LoopMode;

#if SCENE_MODE
    public SceneReference MainScene;
    public List<SceneReference> Levels;
    public bool IsMainMenuEnabled;

    public void BuildSettingsSetter()
    {
        LevelManager.ExternalBuildSettings();
    }
#else
    public List<LevelDataBase> Levels;
#endif
}