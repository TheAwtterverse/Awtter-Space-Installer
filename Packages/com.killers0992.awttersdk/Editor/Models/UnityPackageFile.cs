using AwtterSDK.Editor.Interfaces;

namespace AwtterSDK.Editor.Models
{
    public class UnityPackageFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public bool RequiresAuth { get; set; }
        public ICheckInstallStatus InstallStatus { get; set; }
    }
}