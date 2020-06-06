using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;

public class SoundDataCreatorWindow : EditorWindow
{
    private AudioClip[] audios;
    private List<string> enumElements = new List<string>();

    private string enumFileSavePath = "";
    private string assetDataSavePath = "";

    [MenuItem("Window/SoundDataCreator")]
    private static void Open()
    {
        GetWindow<SoundDataCreatorWindow>("SoundDataCreator");
    }

    private void OnGUI()
    {
        using (new EditorGUILayout.VerticalScope())
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("サウンドデータを読み込み"))
                {
                    audios = Resources.LoadAll<AudioClip>("");
                }
            }

            if (audios == null) return;
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Enum名");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("サウンドデータ");
                }

                    foreach (var audio in audios)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (audio != null) { GUILayout.Label(audio.name); }
                            else { GUI.color = Color.red; GUILayout.Label("Missing"); }
                            GUILayout.FlexibleSpace();
                            using (new EditorGUI.DisabledGroupScope(true))
                            {
                                EditorGUILayout.ObjectField(audio, typeof(AudioClip));
                            }
                    }
                }
            }

            GUI.color = Color.white;

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                GUILayout.Label("Enumクラスの出力先");
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(enumFileSavePath);
                    if (GUILayout.Button("変更", GUILayout.Width(50)))
                    {
                        string path = EditorUtility.SaveFilePanel("Enumクラスの出力先", "Assets", "SoundEnum", "cs");
                        if (path.Length != 0) { enumFileSavePath = ToRelativePath(path); }
                    }
                }
            }

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                GUILayout.Label("アセットデータの出力先");
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(assetDataSavePath);
                    if (GUILayout.Button("変更", GUILayout.Width(50)))
                    {
                        string path = EditorUtility.SaveFilePanel("アセットデータの出力先", "Assets", "SoundData", "asset");
                        if (path.Length != 0) { assetDataSavePath = ToRelativePath(path); }
                    }
                }
            }

            if (enumFileSavePath.Length != 0 && assetDataSavePath.Length != 0)
            {
                if (GUILayout.Button("データ作成"))
                {
                    enumElements.Clear();

                    foreach(var audio in audios)
                    {
                        enumElements.Add(audio.name);
                    }

                    string[] split = enumFileSavePath.Split('/');
                    string enumName = split[split.Length - 1].Substring(0, split[split.Length - 1].IndexOf(".cs"));
                    EnumCreator.Create(enumElements.ToArray(), enumFileSavePath, enumName);

                    AssetDatabase.CreateAsset(new SoundAssetsScript(audios), assetDataSavePath);

                    AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
                }
            }
        }
    }

    private string ToRelativePath(string absolutePath)
    {
        string[] relativePath = Regex.Split(absolutePath, "/Assets/");
        if (relativePath.Length >= 2)
        {
            return "Assets/" + relativePath[1];
        }
        else
        {
            return absolutePath;
        }
    }
}