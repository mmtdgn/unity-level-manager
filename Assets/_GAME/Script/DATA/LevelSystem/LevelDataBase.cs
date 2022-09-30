// @By Mehmet Doğan Date 2022-09-28 //
using UnityEngine;

public abstract class LevelDataBase : MonoBehaviour
{
    public LevelDataBase() { }
    public LevelDataBase(int levelIndex) => m_LevelIndex = levelIndex;
    public void OnSpawn(int levelIndex)
    {
        m_LevelIndex = levelIndex;
        OnLevelLoadingComplete();
    }
    private int m_LevelIndex;
    protected int LevelIndex { get => m_LevelIndex; }
    protected abstract void OnLevelLoadingComplete();
}