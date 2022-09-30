// @By Mehmet Doğan Date 2022-09-28 //
public abstract class GameManagerBase : Singleton<GameManagerBase>
{
    protected void Awake() => Init();
    protected new void Init()
    {
        LevelManager.OnLevelStart += OnLevelStart;
        LevelManager.OnLevelComplete += OnLevelComplete;
        LevelManager.OnLevelFail += OnLevelFail;
#if SCENE_MODE
        LevelManager.OnReturnMainMenu += OnReturnMainMenu;
#endif
        base.Init();
        OnAwake();
    }

    protected abstract void OnAwake();
    protected abstract void OnLevelFail(int levelIndex);
    protected abstract void OnLevelComplete(int levelIndex, int score);
    protected abstract void OnLevelStart(LevelDataBase levelData, int levelIndex);
#if SCENE_MODE
    protected abstract void OnReturnMainMenu();
#endif

    public void CompleteLevel(int score = 100)
    {
        //Do some stuff for level complete
        LevelManager.OnLevelCompleted(score);
    }

    public void FailLevel()
    {
        //Do some stuff for level fail
        LevelManager.OnLevelFailed();
    }

    public void NextLevel()
    {
        LevelManager.LoadNextLevel();
    }

    public void RestartLevel()
    {
        LevelManager.RestartLevel();
    }

#if SCENE_MODE
    public void LoadMainMenu()
    {
        LevelManager.LoadMainMenu();
    }
#endif

    private new void OnDestroy()
    {
        base.OnDestroy();
        LevelManager.OnLevelStart -= OnLevelStart;
        LevelManager.OnLevelComplete -= OnLevelComplete;
        LevelManager.OnLevelFail -= OnLevelFail;
    }
}
