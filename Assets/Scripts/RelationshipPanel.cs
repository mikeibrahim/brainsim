using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;

public class RelationshipPanel : Panel
{
    [MenuItem("Window/Relationship Panel")]
    public static void ShowWindow() => GetWindow<RelationshipPanel>("Relationship Panel");

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Relationship Panel");
    }

    public override void StartPlayMode()
    {
        Debug.Log("Starting Relationship Panel playmode");
    }

    public override void EndPlayMode()
    {
        Debug.Log("Ending Relationship Panel playmode");
    }
}