using UnityEngine;

namespace AwtterSDK.Editor.Interfaces
{
    public interface IPage
    {
        void Load(AwtterSdkInstaller main);
        void DrawGUI(Rect pos);
        void Reset();
    }
}