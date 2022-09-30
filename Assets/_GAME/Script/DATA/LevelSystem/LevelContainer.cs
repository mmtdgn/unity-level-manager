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
#else
    public List<LevelDataBase> Levels;
#endif
    // TODO: Is level data empty or none throw error box
}