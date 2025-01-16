using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwtterSDK.Editor.Interfaces;
using AwtterSDK.Editor.Models;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AwtterSDK.Editor.Pages
{
    public class ScenesPage : IPage
    {
        public static bool DoRefreshScenes;
        private AwtterSdkInstaller _main;
        private List<SceneInfo> _scenes = new();
        private Vector2 _scenesScroll = Vector2.zero;

        public void Load(AwtterSdkInstaller main)
        {
            _main = main;
            RefreshScenes();
        }

        public void DrawGUI(Rect pos)
        {
            if (DoRefreshScenes)
                RefreshScenes();

            EditorGUILayout.Space(10);
            Utils.CreateBox("Select your scene!");
            _scenesScroll = EditorGUILayout.BeginScrollView(_scenesScroll, GUILayout.Height(240));

            foreach (var scene in _scenes)
            {
                GUI.color = SceneManager.GetActiveScene().name == scene.SceneName ? Color.green : Color.white;
                if (GUILayout.Button($"{scene.SceneName}", GUILayout.Height(30)))
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(scene.FullPath);
                GUI.color = Color.white;
            }

            EditorGUILayout.EndScrollView();

            if (AwtterSdkInstaller.CurrentBase.IsOutdated || AwtterSdkInstaller.AvaliableDlcs.Any(x => x.IsOutdated))
                EditorGUILayout.HelpBox(string.Concat(
                        AwtterSdkInstaller.CurrentBase.IsOutdated
                            ? "Your base is outdated!" + Environment.NewLine
                            : string.Empty,
                        AwtterSdkInstaller.AvaliableDlcs.Any(x => x.IsOutdated)
                            ? "Your DLCS are outdated!"
                            : string.Empty),
                    MessageType.Warning);
            else
                GUILayout.Space(5);

            EditorGUILayout.BeginVertical();
            GUI.color = ModelSelectionPage.OrangeColor;
            if (GUILayout.Button("Manage packages", _main.Shared.WindowCustomButton3))
                AwtterSdkInstaller.ViewManagePackages = !AwtterSdkInstaller.ViewManagePackages;
            GUI.color = Color.red;
            if (GUILayout.Button("Reset", _main.Shared.WindowCustomButton3)) AwtterSdkInstaller.ViewReset = true;
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
        }

        public void Reset()
        {
        }

        private void RefreshScenes()
        {
            _scenes = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories)
                .Select(x => new SceneInfo
                {
                    SceneName = Path.GetFileNameWithoutExtension(x),
                    FullPath = x
                }).ToList();
        }
    }
}