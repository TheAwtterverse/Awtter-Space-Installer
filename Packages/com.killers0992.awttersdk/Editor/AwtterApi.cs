using System.Collections;
using AwtterSDK.Editor.Enums;
using AwtterSDK.Editor.Models.API;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace AwtterSDK.Editor
{
    public class AwtterApi
    {
        public static IEnumerator GetProducts(bool isFirst = true)
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/products"))
            {
                www.SetRequestHeader("Authorization", $"Token {TokenCache.Token}");

                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                AwtterSdkInstaller.Products = JsonConvert.DeserializeObject<ProductsModel>(www.downloadHandler.text);

                if (!AwtterSdkInstaller.LoggedIn && isFirst)
                {
                    Debug.Log("[<color=orange>Awtter SDK</color>] Logged in using cache!");
                    AwtterSdkInstaller.LoggedIn = true;
                }
            }
        }

        public static IEnumerator GetConfig()
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/config"))
            {
                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                var okResponse = JsonConvert.DeserializeObject<ConfigOkResponseModel>(www.downloadHandler.text);

                if (okResponse.Status == StatusType.Success)
                    AwtterSdkInstaller.RemoteConfig = okResponse.Data;
            }
        }

        public static IEnumerator GetCurrentUser()
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/users/me"))
            {
                www.SetRequestHeader("Authorization", $"Token {TokenCache.Token}");

                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                var model = JsonConvert.DeserializeObject<CurrentUserResponseModel>(www.downloadHandler.text);

                if (model.Status != StatusType.Success) yield break;

                AwtterSdkInstaller.LoggedInUser = model.Data;
            }
        }

        public static IEnumerator GetToolbox()
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/products/toolbox"))
            {
                www.SetRequestHeader("Authorization", $"Token {TokenCache.Token}");

                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                var model = JsonConvert.DeserializeObject<ToolboxResponseModel>(www.downloadHandler.text);

                if (model.Status != StatusType.Success) yield break;

                foreach (var tool in model.Data.Files) tool.IsTool = true;

                model.Data.Files.RemoveAll(x => x.Name == "7zip");

                AwtterSdkInstaller.Toolbox = model.Data;
            }
        }

        public static IEnumerator GetPatreon()
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/patreon/me"))
            {
                www.SetRequestHeader("Authorization", $"Token {TokenCache.Token}");

                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                var model = JsonConvert.DeserializeObject<PatreonResponseModel>(www.downloadHandler.text);

                if (model.Status != StatusType.Success) yield break;

                AwtterSdkInstaller.Patreon = model.Data;
                AwtterSdkInstaller.RefreshAwtterPackages = true;
            }
        }

        public static IEnumerator GetMarketplace()
        {
            using (var www = UnityWebRequest.Get("https://awtterspace.com/api/v2/marketplace"))
            {
                www.SetRequestHeader("Authorization", $"Token {TokenCache.Token}");

                yield return www.SendWebRequest();

                if (www.responseCode != 200) yield break;

                var model = JsonConvert.DeserializeObject<MarketplaceResponseModel>(www.downloadHandler.text);

                if (model.Status != StatusType.Success) yield break;

                foreach (var p in model.Data)
                foreach (var f in p.Files)
                    f.IsMarketPlace = true;

                AwtterSdkInstaller.Marketplace = model.Data;
                AwtterSdkInstaller.RefreshAwtterPackages = true;
            }
        }
    }
}