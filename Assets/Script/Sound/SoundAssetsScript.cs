using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundAssetsScript : ScriptableObject
{
    [SerializeField] private AudioClip[] soundDatas;

    public SoundAssetsScript(AudioClip[] soundData)
    {
        soundDatas = soundData;
    }

    public AudioClip GetAudioClip(Enum soundEnum)
    {
        return soundDatas[Convert.ToInt32(soundEnum)];
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SoundAssetsScript))]
    private class SoundAssetsScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                var soundAssets = target as SoundAssetsScript;

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Enum名");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("サウンドデータ");
                }

                foreach (var audio in soundAssets.soundDatas)
                {
                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField(audio.name);
                        using (new EditorGUI.DisabledGroupScope(true))
                        {
                            EditorGUILayout.ObjectField(audio, typeof(AudioClip));
                        }
                    }
                }
            }

            EditorUtility.SetDirty(this);
        }
    }
#endif
}
