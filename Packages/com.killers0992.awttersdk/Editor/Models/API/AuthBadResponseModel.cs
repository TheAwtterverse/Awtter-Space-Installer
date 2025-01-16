using AwtterSDK.Editor.Enums;

namespace AwtterSDK.Editor.Models.API
{
    public class AuthBadResponseModel
    {
        public StatusType Status { get; set; }
        public string Message { get; set; }
    }
}