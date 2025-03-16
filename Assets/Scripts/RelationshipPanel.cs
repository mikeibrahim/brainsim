using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;

/*
I want to have an inputfield for the target relationship type and source
and then a button to add the relationship to the current UKS data specified in the hierarchy panel
*/
public class RelationshipPanel : Panel
{
    private string sourceNode;
    private string targetNode;
    private string relationshipType;
    private HierarchyPanel hierarchyPanel;

    [MenuItem("Window/Relationship Panel")]
    public static void ShowWindow() => GetWindow<RelationshipPanel>("Relationship Panel");

    private void OnGUI()
    {
        // if (hierarchyPanel == null) hierarchyPanel = GetWindow<HierarchyPanel>("Hierarchy Panel");
        // EditorGUILayout.LabelField("Relationship Panel");
        // sourceNode = EditorGUILayout.TextField("Source Node", sourceNode);
        // relationshipType = EditorGUILayout.TextField("Relationship Type", relationshipType);
        // targetNode = EditorGUILayout.TextField("Target Node", targetNode);
        // if (GUILayout.Button("Add Relationship")) AddRelationship(sourceNode, targetNode, relationshipType);
    }

    private void AddRelationship(string sourceNode, string targetNode, string relationshipType)
    {
        UKS_Data data = hierarchyPanel.GetSelectedData();
        // if (data.active)
        // {
        //     // if the data belongs to an agent, get the agent
        //     // Agent agent = hierarchyPanel.GetSelectedAgent();
        // }
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