using System;
using System.Diagnostics;
using System.IO;
using AwtterSDK.Editor.Interfaces;

namespace AwtterSDK.Editor.Installations
{
    public class MergerInstallation : ICheckInstallStatus
    {
        private Version _version;
        public bool IsInstalled { get; private set; }

        public Version InstalledVersion => _version;

        public void Check()
        {
            IsInstalled = Directory.Exists("Assets/AwboiMerger");

            if (IsInstalled)
            {
                var targetFile = "Assets/AwboiMerger/AWBOI_MERGER.dll";

                if (File.Exists(targetFile))
                {
                    var ver = FileVersionInfo.GetVersionInfo(targetFile);
                    Version.TryParse(ver.FileVersion, out _version);
                }
            }
        }
    }
}