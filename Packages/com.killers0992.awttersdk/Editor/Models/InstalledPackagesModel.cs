using System.Collections.Generic;
using AwtterSDK.Editor.Installations;
using AwtterSDK.Editor.Interfaces;
using Newtonsoft.Json;

namespace AwtterSDK.Editor.Models
{
    public class InstalledPackagesModel
    {
        public static Dictionary<string, ICheckInstallStatus> CheckInstallStatuses = new()
        {
            { "poiyomi", new PoyomiInstallation() },
            { "awttermerger", new MergerInstallation() },
            { "pumkintool", new PumkinInstallation() }
        };

        [JsonIgnore] public Dictionary<string, InstalledPackageModel> Tools = new();

        public InstalledPackageModel BaseModel { get; set; }
        public Dictionary<int, InstalledPackageModel> Dlcs { get; set; } = new();
        public Dictionary<int, InstalledPackageModel> Marketplace { get; set; } = new();

        public void CheckTools()
        {
            Tools.Clear();
            foreach (var tool in CheckInstallStatuses)
            {
                tool.Value.Check();
                if (tool.Value.IsInstalled)
                    Tools.Add(tool.Key, new InstalledPackageModel
                    {
                        Version = tool.Value.InstalledVersion == null
                            ? string.Empty
                            : tool.Value.InstalledVersion.ToString()
                    });
            }
        }
    }
}