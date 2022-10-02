// @By Mehmet Doğan Date 2022-09-28 //
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LevelContainer))]
public class LevelContainerEditor : Editor
{
    private const string SCENE_MODE = "SCENE_MODE";
    private SerializedProperty m_LevelListProperty;
    private ReorderableList m_List;
    private LevelContainer m_Base;

    private void OnEnable()
    {
        m_LevelListProperty = serializedObject.FindProperty("Levels");
        m_List = new ReorderableList(serializedObject, m_LevelListProperty, true, true, true, true)
        {
            drawHeaderCallback = DrawListHeader,
            drawElementCallback = DrawListElement
        };

        m_Base = target as LevelContainer;
    }

    private void DrawListHeader(Rect rect) => GUI.Label(rect, "Levels");

    private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = m_LevelListProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item, new GUIContent($"Level Index {index.ToString()}"));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
        {
            DrawLabel();
            DrawLine();
            DrawLevelModeToggle();
            DrawLine();
            DrawProperties();
            DrawLine();
            DrawReorderableList();
#if SCENE_MODE
            DrawBuildSettingsButton();
#endif
        }
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLevelModeToggle()
    {
        GUI.backgroundColor = Color.gray;
#if SCENE_MODE
        if (GUILayout.Button("Switch To Prefab Mode", GUILayout.Height(30)))
            GlobalDefinitions.RemoveDefinition(BuildTargetGroup.Standalone, SCENE_MODE);
#else
        if (GUILayout.Button("Switch To Scene Mode", GUILayout.Height(30)))
            GlobalDefinitions.AddDefinition(BuildTargetGroup.Standalone, SCENE_MODE);
#endif
        GUI.backgroundColor = Color.white;
    }

    private void DrawLabel()
    {
        EditorGUILayout.LabelField("Level Container", EditorStyles.boldLabel);
    }

    private void DrawProperties()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LoopMode"));
#if SCENE_MODE
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MainScene"));
        if (m_Base.IsMainMenuEnabled)
            if (GUILayout.Button("Switch To Prefab Mode", GUILayout.Height(30)))
                GlobalDefinitions.RemoveDefinition(BuildTargetGroup.Standalone, SCENE_MODE);
#endif
    }

    private void DrawLine()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DrawReorderableList()
    {
        EditorGUILayout.Space();
        m_List.DoLayoutList();
    }

#if SCENE_MODE
    private void DrawBuildSettingsButton()
    {
        if (GUILayout.Button("Set Build Settings", GUILayout.Height(30)))
            m_Base.BuildSettingsSetter();

        if (EditorBuildSettings.scenes.Length == 0)
            EditorGUILayout.HelpBox("Build Settings is empty...\n Please Set build settings!", MessageType.Error);
    }
#endif
}