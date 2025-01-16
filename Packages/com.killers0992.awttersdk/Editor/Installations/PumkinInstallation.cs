using System;
using System.IO;
using AwtterSDK.Editor.Interfaces;
using AwtterSDK.Editor.Models;
using Newtonsoft.Json;

namespace AwtterSDK.Editor.Installations
{
    public class PumkinInstallation : ICheckInstallStatus
    {
        private Version _version;

        public bool IsInstalled { get; private set; }

        public Version InstalledVersion => _version;

        public void Check()
        {
            IsInstalled = Directory.Exists("Assets/PumkinsAvatarTools");

            if (IsInstalled)
            {
                var targetFile = "Assets/PumkinsAvatarTools/thry_module_manifest.json";

                if (File.Exists(targetFile))
                {
                    var manifest = JsonConvert.DeserializeObject<PackageManifest>(File.ReadAllText(targetFile));

                    Version.TryParse(manifest.Version, out _version);
                }
            }
        }
    }
}