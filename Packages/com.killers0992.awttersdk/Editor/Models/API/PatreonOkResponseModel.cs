using System.Collections.Generic;

namespace AwtterSDK.Editor.Models.API
{
    public class PatreonOkResponseModel
    {
        public bool Active { get; set; }
        public string Tier { get; set; }
        public List<ProductModel> Benefits { get; set; }
    }
}