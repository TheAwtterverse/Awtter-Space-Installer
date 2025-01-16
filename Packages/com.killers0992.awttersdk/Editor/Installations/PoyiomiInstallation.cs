using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AwtterSDK.Editor.Interfaces;

namespace AwtterSDK.Editor.Installations
{
    public class PoyomiInstallation : ICheckInstallStatus
    {
        private Version _version;

        public bool IsInstalled { get; private set; }

        public Version InstalledVersion => _version;

        public void Check()
        {
            IsInstalled = Directory.Exists("Assets/_PoiyomiShaders");

            if (IsInstalled)
                if (Directory.Exists("Assets/_PoiyomiShaders/Shaders"))
                    foreach (var dir in Directory.GetDirectories("Assets/_PoiyomiShaders/Shaders"))
                    foreach (var file in Directory.GetFiles(dir, "*.shader"))
                    {
                        var content = File.ReadAllLines(file);
                        var versionLine = content.FirstOrDefault(p => p.Contains("shader_master_label"));
                        if (versionLine != null)
                        {
                            var line = versionLine.Split('(', ')')[1];
                            var line2 = line.Split('"')[1];
                            var line3 = line2.Split(' ')[1];

                            var finalVersion = Regex.Replace(line3, @"<[^>]*>", string.Empty);

                            Version.TryParse(finalVersion, out _version);
                            return;
                        }
                    }
        }
    }
}