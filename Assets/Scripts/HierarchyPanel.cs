using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;

public class HierarchyPanel : Panel
{
    private UKS_Data selectedData;
    private UKS_Node selectedNode;
    private Dictionary<UKS_Node, float> changedNodes;
    private readonly float highlightTime = 1f;
    private UKSPanel uksPanel;

    [MenuItem("Window/Hierarchy Panel")]
    public static void ShowWindow() => GetWindow<HierarchyPanel>("Hierarchy Panel");

    private void OnGUI()
    {
        if (uksPanel == null) uksPanel = GetWindow<UKSPanel>("UKS Panel");

        List<string> options = new List<string>();
        foreach (UKS_Object obj in uksPanel.GetUKSObjects()) options.Add(obj.GetData().label);
        foreach (UKS_Data data in uksPanel.GetSavedUKSData()) options.Add(data.label);

        if (options.Count == 0)
        {
            EditorGUILayout.LabelField("No UKS Data found.");
            return;
        }
        EditorGUILayout.LabelField("Select UKS Data:");
        int selectedIndex = selectedData != null ? options.IndexOf(selectedData.label) : 0;
        selectedIndex = EditorGUILayout.Popup(selectedIndex, options.ToArray());

        if (selectedIndex >= 0 && selectedIndex < uksPanel.GetUKSObjects().Count)
            selectedData = uksPanel.GetUKSObjects()[selectedIndex].GetData();
        else if (selectedIndex >= uksPanel.GetUKSObjects().Count && selectedIndex < options.Count)
            selectedData = uksPanel.GetSavedUKSData()[selectedIndex - uksPanel.GetUKSObjects().Count];

        Debug.Log("Selected data: " + selectedData.label);
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

    public override void StartPlayMode()
    {
        Debug.Log("Starting Hierarchy Panel playmode");
    }

    public override void EndPlayMode()
    {
        Debug.Log("Ending Hierarchy Panel playmode");
    }
}