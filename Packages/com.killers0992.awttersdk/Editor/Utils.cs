using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AwtterSDK.Editor
{
    public static class Utils
    {
        private static readonly string[] suffixes =
            { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static Texture2D CreateTexture(int width, int height, Color col)
        {
            var pix = new Color[width * height];

            for (var i = 0; i < pix.Length; i++)
                pix[i] = col;

            var result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        public static long DirectorySize(DirectoryInfo d)
        {
            long size = 0;
            var fis = d.GetFiles();
            foreach (var fi in fis) size += fi.Length;

            var dis = d.GetDirectories();
            foreach (var di in dis) size += DirectorySize(di);
            return size;
        }


        public static string FormatSize(long bytes)
        {
            var counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }

            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public static void CreateBox(string text, bool flexible = true)
        {
            EditorGUILayout.BeginHorizontal("box");
            if (flexible) GUILayout.FlexibleSpace();
            GUILayout.Label(text, EditorStyles.boldLabel);
            if (flexible) GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        public static void CreateBox(string text, bool flexible = true, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal("box");
            if (flexible) GUILayout.FlexibleSpace();
            GUILayout.Label(text, EditorStyles.boldLabel, options);
            if (flexible) GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}