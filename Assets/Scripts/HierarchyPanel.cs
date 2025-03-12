using UnityEngine;
using UnityEditor;

public class HierarchyPanel : EditorWindow
{
    private string _myText = "Hello, Editor Panel!";

    [MenuItem("Window/Hierarchy Panel")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyPanel>();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField(_myText);

        if (GUILayout.Button("Change Text"))
        {
            _myText = "Text Changed!";
        }
    }
}