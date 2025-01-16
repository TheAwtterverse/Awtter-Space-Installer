using System;

namespace AwtterSDK.Editor.Models.API
{
    public class AwtterSdkResponseModel
    {
        private Version _version;

        public Version Version2
        {
            get
            {
                if (_version == null)
                    System.Version.TryParse(Version, out _version);

                return _version;
            }
        }

        public string Version { get; set; }
        public string Path { get; set; }
    }
}