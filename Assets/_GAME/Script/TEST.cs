using UnityEngine;
using SimpleUnityObserver;

public class TEST : MonoBehaviour
{
    private GameManager m_GameManager;
    private void Awake()
    {
        SimpleObserver.AddCallback(LevelComplete);
        SimpleObserver.AddCallback(LevelFail);
        SimpleObserver.AddCallback(RestartLevel);
        SimpleObserver.AddCallback(NextLevel);
#if SCENE_MODE
        SimpleObserver.AddCallback(LoadMainMenu);
#endif
        m_GameManager = GameManager.Instance as GameManager;
    }
    
#if SCENE_MODE
    private void LoadMainMenu()
    {
        m_GameManager.LoadMainMenu();
    }
#endif

    private void NextLevel()
    {
        m_GameManager.NextLevel();
    }

    public void LevelComplete()
    {
        m_GameManager.CompleteLevel();
    }

    public void LevelFail()
    {
        m_GameManager.FailLevel();
    }

    private void RestartLevel()
    {
        m_GameManager.RestartLevel();
    }
}
