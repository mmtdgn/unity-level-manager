using UnityEngine;
using UnityEditor;

namespace SimpleUnityObserver
{
    public class SimpleObserverWindow : EditorWindow
    {
        private readonly Color COLOR_RED = new Color(212 / 255f, 43f / 255f, 14f / 255f, 1f);
        private readonly Color COLOR_GREEN = new Color(212 / 255f, 43f / 255f, 14f / 255f, 1f);

        private GUIStyle GetLabelStyle(Color color)
        {
            return new GUIStyle()
            {
                normal =
                {
                    textColor = color
                },
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
            };
        }

        [MenuItem("MD/SimpleObserver")]
        private static void ShowWindow()
        {
            var window = GetWindow<SimpleObserverWindow>();
            window.titleContent = new GUIContent("SimpleObserver");
            window.Show();
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                DrawLabel(COLOR_RED, "Editor is not running!");
                DrawBox();
            }
            else
            {
                DrawBox();
                DrawLabel(Color.green);
                DrawCallBackButtons();
            }
        }

        private void DrawBox()
        {
            Rect _boxRect = new Rect(10, 40, position.width - 20, position.height - 50);
            GUI.backgroundColor = Color.gray;
            GUI.Box(_boxRect, "", EditorStyles.objectField);
            GUI.backgroundColor = Color.white;
        }

        private void DrawLabel(Color color, string Label = "Callbacks")
        {
            Rect _labelRect = new Rect(10, 10, position.width - 20, 20);
            GUI.Box(_labelRect, string.Empty, EditorStyles.objectField);
            GUI.Label(_labelRect, Label, GetLabelStyle(color));
        }

        private void DrawCallBackButtons()
        {
            for (int i = 0; i < SimpleObserver.GetCallBackCount; i++)
            {
                CallBack _callback = SimpleObserver.GetCallback(i, out string methodName);
                Rect _buttonRect = new Rect(15, 45 + i * 25, position.width - 30, 20);
                if (GUI.Button(_buttonRect, methodName))
                    _callback?.Invoke();
            }
        }
    }
}
