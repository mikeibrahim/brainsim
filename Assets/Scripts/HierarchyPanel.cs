using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;

public class HierarchyPanel : EditorWindow
{
    private UKS_Data selectedData;
    private UKS_Node selectedNode;
    private Dictionary<UKS_Node, float> changedNodes;
    private readonly float highlightTime = 1f;

    [MenuItem("Window/Hierarchy Panel")]
    public static void ShowWindow() => GetWindow<HierarchyPanel>("Hierarchy Panel");

    private void OnGUI()
    {
        // make a button to select the data from 
        // if (GUILayout.Button("Click Me"))
        // {
        //     UKS.AddNode(new UKS_Node("Node " + UKS.GetNodes().Count));
        //     Debug.Log("Number of nodes: " + UKS.GetNodes().Count);
        // }
        // EditorGUILayout.BeginVertical();
        // _expanded = EditorGUILayout.Foldout(_expanded, "Click to " + (_expanded ? "collapse" : "expand"), true);
        // if (_expanded) {
        //     EditorGUI.indentLevel++;
        //     EditorGUILayout.LabelField("The inner stuff");
        //     EditorGUI.indentLevel--;
        // }	   
        // EditorGUILayout.EndVertical();
    }

    private void StartPlaymode()
    {
    }

    private void EndPlaymode()
    {
    }

    // Handle play mode state changes
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode) StartPlaymode();
        else if (state == PlayModeStateChange.ExitingPlayMode) EndPlaymode();
    }
    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
}