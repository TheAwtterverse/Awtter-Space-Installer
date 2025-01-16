using System.Collections.Generic;
using AwtterSDK.Editor.Enums;

namespace AwtterSDK.Editor.Models.API
{
    public class MarketplaceResponseModel
    {
        public StatusType Status { get; set; }
        public List<ProductModel> Data { get; set; }
    }
}