using System.Collections.Generic;

namespace AwtterSDK.Editor.Models.API
{
    public class VrcPackage
    {
        public Dictionary<string, VrcFile> Versions { get; set; }
    }
}