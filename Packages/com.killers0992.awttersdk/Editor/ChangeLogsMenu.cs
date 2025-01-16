using System.IO;
using AwtterSDK.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Awtter_SDK.Editor
{
    public class ChangeLogsMenu : EditorWindow
    {
        private static ChangeLogsMenu _window;

        public string[] Changelogs;

        public Vector2 scroll = Vector2.zero;

        public string ChangeLogsPath => Path.Combine(Paths.MainPath, "Editor", "Textures", "changelogs.txt");

        public static void ShowChangelogs()
        {
            _window = (ChangeLogsMenu)GetWindow(typeof(ChangeLogsMenu), false, "Awtter SDK | Changelogs");
            _window.minSize = new Vector2(600f, 600f);

            var position = _window.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            _window.position = position;

            _window.Show();
        }

        private void OnGUI()
        {
            if (Changelogs == null) Changelogs = File.ReadAllLines(ChangeLogsPath);

            scroll = GUILayout.BeginScrollView(scroll, false, true);
            foreach (var line in Changelogs) GUILayout.Label(line);
            GUILayout.EndScrollView();

            if (GUILayout.Button("CLOSE", GUILayout.MinWidth(50), GUILayout.MinHeight(32))) Close();
        }
    }
}