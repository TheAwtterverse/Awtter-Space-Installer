using System;
using AwtterSDK.Editor.Models.API;

namespace AwtterSDK.Editor.Models
{
    public class MarketplaceView
    {
        private Version _currentVersion;

        private Version _newVersion;
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string InstalledVersion { get; set; }
        public string DownloadUrl { get; set; }
        public bool IsMarketplace { get; set; }
        public CategoryModel Category { get; set; }

        public bool IsInstalled => AwtterSdkInstaller.InstalledPackages != null &&
                                   AwtterSdkInstaller.InstalledPackages.Marketplace.ContainsKey(Id);

        public bool IsOutdated
        {
            get
            {
                if (_newVersion == null && !System.Version.TryParse(Version, out _newVersion))
                    return false;

                if (_currentVersion == null && !System.Version.TryParse(InstalledVersion, out _currentVersion))
                    return false;

                return _currentVersion.CompareTo(_newVersion) > 0;
            }
        }

        public bool Install { get; set; }
    }
}