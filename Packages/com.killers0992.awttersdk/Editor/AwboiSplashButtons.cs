using System;
using System.Collections.Generic;
using UnityEngine;

namespace AWBOI.SplashScreen
{
    [Serializable]
    [CreateAssetMenu(fileName = "AwboiSplashButtons", menuName = "ScriptableObjects/AwboiSplashButtons", order = 1)]
    public class AwboiSplashButtons : ScriptableObject
    {
        public List<SplashButton> Buttons;
        public string credPath;

        public string SceneFolder;
    }

    [Serializable]
    public class SplashButton
    {
        public enum bType
        {
            WebLink,
            File
        }

        public string ButtonText;
        public bType ButtonType;

        [Tooltip("(Optional) Icon shown in the Splash Screen.")]
        public Texture2D Image;

        public string Link;

        public SplashButton(string text, string link)
        {
            ButtonText = text;
            Link = link;
        }
    }
}