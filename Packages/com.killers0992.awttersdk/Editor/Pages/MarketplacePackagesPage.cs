using System.IO;
using System.Linq;
using AwtterSDK.Editor.Interfaces;
using UnityEditor;
using UnityEngine;

namespace AwtterSDK.Editor.Pages
{
    public class MarketplacePackagesPage : IPage
    {
        private Texture2D _awtterInboxImage;
        private AwtterSdkInstaller _main;

        public GUIStyle CustomButton;
        public GUIStyle CustomButton2;

        public Vector2 Scroll = Vector2.zero;

        public Texture2D AwtterInboxImage
        {
            get
            {
                if (_awtterInboxImage == null)
                    _awtterInboxImage =
                        AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(Paths.MainPath, "Editor", "Textures",
                            "inbox.png"));

                return _awtterInboxImage;
            }
        }

        public void Load(AwtterSdkInstaller main)
        {
            _main = main;

            CustomButton = new GUIStyle(GUI.skin.button);
            CustomButton.fontSize = 15;
            CustomButton.alignment = TextAnchor.MiddleCenter;

            CustomButton2 = new GUIStyle(GUI.skin.button);
            CustomButton2.fontSize = 15;
            CustomButton2.alignment = TextAnchor.MiddleCenter;
            CustomButton2.normal.textColor = Color.green;
        }

        public void DrawGUI(Rect pos)
        {
            GUILayout.Space(15);
            Utils.CreateBox("Marketplace items");
            GUILayout.Space(15);

            Scroll = GUILayout.BeginScrollView(Scroll);
            if (AwtterSdkInstaller.AvaliableBases.Any(x => x.IsMarketplace))
            {
                Utils.CreateBox("BASES");
                GUILayout.Space(5);
                foreach (var baseModel in AwtterSdkInstaller.AvaliableBases.Where(x => x.IsMarketplace))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Box(TextureCache.GetTextureOrDownload(baseModel.Icon), GUILayout.Height(32),
                        GUILayout.Width(32));
                    if (GUILayout.Button(baseModel.Name,
                            AwtterSdkInstaller.CurrentBase?.Id == baseModel.Id ? CustomButton2 : CustomButton,
                            GUILayout.Height(32)))
                    {
                        AwtterSdkInstaller.CurrentBase =
                            AwtterSdkInstaller.CurrentBase?.Id == baseModel.Id ? null : baseModel;
                        _main.UpdateAwtterPackages();
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
            }

            if (AwtterSdkInstaller.AvaliableMarketplaceItems.Any())
            {
                string currentCategory = null;

                foreach (var dlc in AwtterSdkInstaller.AvaliableMarketplaceItems.OrderBy(y => y.Category.Priority)
                             .ThenBy(z => z.Category.Name))
                {
                    if (dlc.Category.Name != currentCategory)
                    {
                        currentCategory = dlc.Category.Name;

                        Utils.CreateBox(currentCategory.ToUpper());
                        GUILayout.Space(5);
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Box(TextureCache.GetTextureOrDownload(dlc.Icon), GUILayout.Height(32),
                        GUILayout.Width(32));
                    GUI.color = dlc.Install ? Color.green : Color.white;
                    if (GUILayout.Button(dlc.Name, CustomButton, GUILayout.Height(32))) dlc.Install = !dlc.Install;
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
            }

            GUILayout.EndScrollView();
            GUILayout.Space(15);
            End();
        }


        public void Reset()
        {
        }

        private void End()
        {
            GUILayout.BeginHorizontal();
            GUI.color = Color.red;
            if (GUILayout.Button("Go back", _main.Shared.WindowCustomButton3, GUILayout.MinHeight(27)))
                AwtterSdkInstaller.ViewMarketplacePackages = false;
            GUI.color = Color.white;

            GUILayout.Space(10);
            GUI.color = Color.green;
            if (GUILayout.Button("Run SDK Installer", _main.Shared.WindowCustomButton3, GUILayout.MinHeight(27)))
                AwtterSdkInstaller.IsInstalling = true;
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }
    }
}