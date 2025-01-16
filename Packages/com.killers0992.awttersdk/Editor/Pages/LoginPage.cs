using System.Collections;
using System.Collections.Generic;
using AwtterSDK.Editor.Enums;
using AwtterSDK.Editor.Interfaces;
using AwtterSDK.Editor.Models.API;
using Newtonsoft.Json;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace AwtterSDK.Editor.Pages
{
    public class LoginPage : IPage
    {
        private AwtterSdkInstaller _main;

        public string ErrorBox;
        public string Password = string.Empty;
        public bool TryLogin = true;

        public string Username = string.Empty;

        public void Load(AwtterSdkInstaller main)
        {
            _main = main;
        }

        public void DrawGUI(Rect pos)
        {
            if (TryLogin)
            {
                EditorCoroutineUtility.StartCoroutine(Login(true), this);
                TryLogin = false;
            }

            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("Account", "window", GUILayout.Height(120));

            Username = EditorGUILayout.TextField("Email or Username", Username);
            Password = EditorGUILayout.PasswordField("Password", Password);

            if (ErrorBox != null)
                EditorGUILayout.HelpBox(ErrorBox, MessageType.Error);

            if (Username.Length == 0 || Password.Length == 0)
                GUI.enabled = false;

            if (GUILayout.Button("Login"))
                EditorCoroutineUtility.StartCoroutine(Login(), this);

            GUI.enabled = true;

            if (GUILayout.Button("Register"))
                Application.OpenURL("https://shadedoes3d.com/accounts/register/");

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        public void Reset()
        {
            TryLogin = true;

            Username = string.Empty;
            Password = string.Empty;

            ErrorBox = null;
        }

        public IEnumerator Login(bool first = false)
        {
            yield return AwtterApi.GetProducts();

            if (AwtterSdkInstaller.LoggedIn)
                yield break;

            if (first) yield break;

            using (var www = UnityWebRequest.Post("https://shadedoes3d.com/api/auth", new Dictionary<string, string>
                   {
                       { "username", Username },
                       { "password", Password }
                   }))
            {
                yield return www.SendWebRequest();

                switch (www.responseCode)
                {
                    // Success response.
                    case 200:
                        var authResponse = JsonConvert.DeserializeObject<AuthOkResponseModel>(www.downloadHandler.text);

                        if (authResponse.Status != StatusType.Success)
                        {
                            ErrorBox = $"Response code {authResponse.Status}";
                            Debug.LogError($"[<color=orange>Awtter SDK</color>] {ErrorBox}!");
                            yield break;
                        }

                        TokenCache.Token = authResponse.Data.Token;

                        Debug.Log("[<color=orange>Awtter SDK</color>] Logged in!");
                        yield return AwtterApi.GetProducts(false);
                        AwtterSdkInstaller.LoggedIn = true;
                        break;
                    // Bad response.
                    case 404:
                        var badResponse = JsonConvert.DeserializeObject<AuthBadResponseModel>(www.downloadHandler.text);
                        ErrorBox = badResponse.Message;
                        Debug.LogError($"[<color=orange>Awtter SDK</color>] {ErrorBox}!");
                        break;
                }
            }
        }
    }
}