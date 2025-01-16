using AwtterSDK.Editor.Enums;

namespace AwtterSDK.Editor.Models.API
{
    public class PatreonResponseModel
    {
        public StatusType Status { get; set; }
        public PatreonOkResponseModel Data { get; set; }
    }
}