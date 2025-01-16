using System;

namespace AwtterSDK.Editor.Interfaces
{
    public interface ICheckInstallStatus
    {
        bool IsInstalled { get; }
        Version InstalledVersion { get; }
        void Check();
    }
}