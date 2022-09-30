// @By Mehmet Doğan, Date 2022-09-28 //
using System.Collections.Generic;
using UnityEngine;
using Tymski;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum LoopMode { EndlessRandom, EndlessContinuous }
// TODO: more cycle modes can be added
// TODO: level indicator can be increased infinitely regardless of the level index
public enum LevelType { Scene, Prefab }
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] protected LevelContainer m_LevelContainer;
#if SCENE_MODE
    [SerializeField] protected LoadingScreenController m_LoadingScreenPrefab;
    protected LoadingScreenController m_LoadingScreenController;
#endif
    public static int GetLevelCount() => Instance.m_LevelContainer.Levels.Count;
    public static int GetLastLevelIndex() => Instance.m_LevelContainer.Levels.Count - 1;
    public static bool IsSaveDataExist => PlayerPrefs.HasKey(KEY_IS_SAVE_DATA_EXIST);
    private static int m_CurrentLevelIndex;
    public static int CurrentLevelIndex { get => m_CurrentLevelIndex; }
    private LevelDataBase m_LoadedLevelData;

    //--------------Events----------------//
    public static event Action<LevelDataBase, int> OnLevelStart;
    public static event Action<int, int> OnLevelComplete;
    public static event Action<int> OnLevelFail;
    public static event Action OnReturnMainMenu;

    //-----------------Keys-----------------//
    private const string KEY_CURRENT_LEVEL_INDEX = "CurrentLevelIndex";
    private const string IS_LAST_LEVEL_COMPLETED = "IsLastLevelCompleted";
    private const string KEY_IS_SAVE_DATA_EXIST = "IsSaveDataExist";
    
#if SCENE_MODE
    protected LoadingScreenController GetLoadingScreen
    {
        get
        {
            if (!m_LoadingScreenController)
            {
                return Instantiate(m_LoadingScreenPrefab, transform);
            }
            else
            {
                Debug.LogError("Loading Screen is not assigned!");
                return null;
            }
        }
    }
#endif


    private void Awake() => base.Init();

#if UNITY_EDITOR && SCENE_MODE
    private void BuildSettings()//AutoUpdate for scene list in build settings
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        //Add MainMenu
        string scenePath = m_LevelContainer.MainScene.ScenePath;
        if (!string.IsNullOrEmpty(scenePath))
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));

        //Add Levels
        for (int i = 0; i < m_LevelContainer.Levels.Count; i++)
        {
            scenePath = m_LevelContainer.Levels[i].ScenePath;
            if (!string.IsNullOrEmpty(scenePath))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }
#endif

    private void Start()
    {
        m_CurrentLevelIndex = GetLastSavedLevelIndex();
#if SCENE_MODE
        m_LoadingScreenController = GetLoadingScreen;

        if (m_LevelContainer.MainScene == null)
        {
            Debug.LogError("Main Scene is not assigned!");
            return;
        }
        else
        {
            LoadMainMenu();
        }
#else
        LoadPrefabLevel(m_CurrentLevelIndex);
#endif
    }

    public static void StartGame(LevelType levelType)
    {
        PlayerPrefs.DeleteAll();
        m_CurrentLevelIndex = 0;
        PlayerPrefs.SetInt(KEY_IS_SAVE_DATA_EXIST, 1);
        PlayerPrefs.Save();
#if SCENE_MODE
        LoadScene(Instance.m_LevelContainer.Levels[CurrentLevelIndex]);
#else
        LoadPrefabLevel(m_CurrentLevelIndex);
#endif
    }

    public static void LoadNextLevel()
    {
        LoopMode _loopMode = Instance.m_LevelContainer.LoopMode;
        if (_loopMode == LoopMode.EndlessContinuous)
        {
            m_CurrentLevelIndex = m_CurrentLevelIndex < GetLastLevelIndex() ? ++m_CurrentLevelIndex : 0;
        }
        else if (_loopMode == LoopMode.EndlessRandom)
        {
            if (!IsLastLevelCompleted() && m_CurrentLevelIndex == GetLastLevelIndex())
            {
                OnLastLevelComplete();
            }
            else if (IsLastLevelCompleted())
            {
                m_CurrentLevelIndex = UnityEngine.Random.Range(0, GetLevelCount());
                Debug.Log(m_CurrentLevelIndex);
            }
            else
            {
                m_CurrentLevelIndex++;
            }
        }

        SaveCurrentLevelIndex();
#if SCENE_MODE
        LoadScene(Instance.m_LevelContainer.Levels[m_CurrentLevelIndex]);
#else
        LoadPrefabLevel(m_CurrentLevelIndex);
#endif
    }

    private static void OnLastLevelComplete()
    {
        PlayerPrefs.SetInt(IS_LAST_LEVEL_COMPLETED, 1);
        PlayerPrefs.Save();
    }
    
    public static bool IsLastLevelCompleted() => PlayerPrefs.GetInt(IS_LAST_LEVEL_COMPLETED, -1) == 1;

#if SCENE_MODE
    public static void LoadMainMenu()
    {
        LoadScene(Instance.m_LevelContainer.MainScene);
    }
#endif

    public static void RestartLevel()
    {
#if SCENE_MODE
        LoadScene(Instance.m_LevelContainer.Levels[m_CurrentLevelIndex]);
#else
        LoadPrefabLevel(m_CurrentLevelIndex);
#endif
    }

#if SCENE_MODE
    public List<SceneReference> GetLevelList()
    {
        return m_LevelContainer.Levels;
    }
#else
    public List<LevelDataBase> GetLevelList()
    {
        return m_LevelContainer.Levels;
    }
#endif

    public static void LoadLevel(int levelIndex)
    {
#if SCENE_MODE
        LoadScene(Instance.m_LevelContainer.Levels[m_CurrentLevelIndex]);
#else
        LoadPrefabLevel(levelIndex);
#endif
    }

    public static void SaveCurrentLevelIndex()
    {
        PlayerPrefs.SetInt(KEY_CURRENT_LEVEL_INDEX, m_CurrentLevelIndex);
        PlayerPrefs.Save();
    }

    private static int GetLastSavedLevelIndex()
    {
        return PlayerPrefs.GetInt(KEY_CURRENT_LEVEL_INDEX, 0);
    }

    private static void StartLevel(LevelDataBase levelData, int levelIndex)
    {
        m_CurrentLevelIndex = levelIndex;
        OnLevelStart?.Invoke(levelData, levelIndex);
    }

    public static void OnLevelCompleted(int score = 100)
    {
        OnLevelComplete?.Invoke(CurrentLevelIndex, score);
    }

    public static void OnLevelFailed()
    {
        OnLevelFail?.Invoke(CurrentLevelIndex);
    }

#if SCENE_MODE
    public void OnReturnedtoMainMenu()
    {
        OnReturnMainMenu?.Invoke();
    }

    #region LoadingScreen
    private static void LoadScene(SceneReference scene, bool loadingScreenEnabled = true)
    {
        if (Instance.m_LoadingScreenController && loadingScreenEnabled)
            Instance.StartCoroutine(LoadSceneAsync(scene));
        else if (!Instance.m_LoadingScreenController && loadingScreenEnabled)
            throw new System.Exception("Loading Screen is not assigned!");
        else
            SceneManager.LoadScene(scene);

        if (Instance.m_LevelContainer.Levels.Contains(scene))
        {
            LevelDataBase LevelDataBase = FindObjectOfType<LevelDataBase>();
            StartLevel(LevelDataBase, m_CurrentLevelIndex);
        }
    }

    private static IEnumerator LoadSceneAsync(SceneReference scene)
    {
        Application.targetFrameRate = 60;
        Instance.m_LoadingScreenController.SetReady();
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            Instance.m_LoadingScreenController.SetFillBar(progressValue);
            yield return null;
        }
        Instance.m_LoadingScreenController.OnLoadingComplete();
    }
    #endregion
    
#else
    private static void LoadPrefabLevel(int levelIndex)
    {
        if (Instance.m_LoadedLevelData)
        {
            Destroy(Instance.m_LoadedLevelData.gameObject);
        }
        Instance.m_LoadedLevelData = Instantiate(Instance.m_LevelContainer.Levels[levelIndex]);
        Instance.m_LoadedLevelData.OnSpawn(m_CurrentLevelIndex);
        StartLevel(Instance.m_LoadedLevelData, levelIndex);
    }
#endif

#if SCENE_MODE
    private void OnValidate()
    {
        BuildSettings();
    }
#endif
}
