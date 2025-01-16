using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;

namespace AwtterSDK.Editor
{
    internal static class TokenCache
    {
        private static string _token;

        internal static string Token
        {
            get
            {
                if (_token == null)
                    ReadToken();

                return _token;
            }
            set
            {
                if (_token == value) return;

                _token = value;
                SaveToken(value);
            }
        }

        private static void SaveToken(string rawToken)
        {
            if (string.IsNullOrEmpty(rawToken))
            {
                EditorPrefs.SetString("AwSdkKey", string.Empty);
                EditorPrefs.SetString("AwSdkToken", string.Empty);
                return;
            }

            var crypto = new AesCryptoServiceProvider();
            crypto.KeySize = 128;
            crypto.BlockSize = 128;
            crypto.GenerateKey();

            EditorPrefs.SetString("AwSdkKey", Convert.ToBase64String(crypto.Key));
            EditorPrefs.SetString("AwSdkToken", AesOperation.EncryptString(crypto.Key, rawToken));
        }

        private static void ReadToken()
        {
            var key = EditorPrefs.GetString("AwSdkKey");
            var token = EditorPrefs.GetString("AwSdkToken");

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(token))
                _token = AesOperation.DecryptString(Convert.FromBase64String(key), token);
        }
    }

    internal static class AesOperation
    {
        public static string EncryptString(byte[] key, string plainText)
        {
            var iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(byte[] key, string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return string.Empty;

            var iv = new byte[16];
            byte[] buffer = null;
            try
            {
                buffer = Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                return string.Empty;
            }

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}