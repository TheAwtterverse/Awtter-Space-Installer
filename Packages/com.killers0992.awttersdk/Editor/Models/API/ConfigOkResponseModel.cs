using AwtterSDK.Editor.Enums;

namespace AwtterSDK.Editor.Models.API
{
    public class ConfigOkResponseModel
    {
        public StatusType Status { get; set; }
        public ConfigResponseModel Data { get; set; }
    }
}