
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class ColorizeMats : EditorWindow
{
    private Color color;

    [MenuItem("Tools/MachSavior/Colorize Mats")]
    public static void ShowWindow()
    {
        GetWindow<ColorizeMats>(true, "Colorize Mats");
    }

    private void OnGUI()
    {
        GUILayout.Label("Color the selected objects", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Apply color"))
        {
            Colorize();
        }
    }

    private void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<Renderer>() != null)
            {
                obj.GetComponent<Renderer>().sharedMaterial.color = color;
            }
        }
    }
}
#endif