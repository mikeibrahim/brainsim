using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class QueryPanel : Panel
{
    private static string sourceLabel = "";
    private static string relationshipLabel = "";
    private static string targetLabel = "";
    private static string queryResult = "";
    private static UKS_Data data;

    [MenuItem("Window/Query Panel")]
    public static void ShowWindow() => GetWindow<QueryPanel>();

    private void OnGUI()
    {
        if (data == null) EditorGUILayout.LabelField("No UKS Data selected.");
        else
        {
            sourceLabel = EditorGUILayout.TextField("Source Node", sourceLabel);
            relationshipLabel = EditorGUILayout.TextField("Relationship Type", relationshipLabel);
            targetLabel = EditorGUILayout.TextField("Target Node", targetLabel);
            if (GUILayout.Button("Query Relationship")) QueryRelationship(sourceLabel, relationshipLabel, targetLabel);
            EditorGUILayout.LabelField("Query Result:");
            EditorGUILayout.TextArea(queryResult, GUILayout.Height(100), GUILayout.ExpandHeight(true));
        }
    }
    private void QueryRelationship(string sourceLabel, string relationshipLabel, string targetLabel)
    {
        if (sourceLabel == "" && relationshipLabel == "" && targetLabel == "") { Debug.LogWarning("Please fill in at least one field."); return; }
        sourceLabel = LintString(sourceLabel);
        targetLabel = LintString(targetLabel);
        relationshipLabel = LintString(relationshipLabel);
        queryResult = ""; // Clear previous results
        foreach (KeyValuePair<string, UKS_Node> node in data.nodes)
        {
            if (sourceLabel != "" && node.Key != sourceLabel) continue; // Filter by sourceLabel
            if (relationshipLabel != "" && !node.Value.relationships.ContainsKey(relationshipLabel)) continue; // Filter by relationshipLabel

            // If we reach here, it means the node matches the query
            foreach (KeyValuePair<string, string> relationship in node.Value.relationships)
            {
                if (targetLabel != "" && relationship.Value != targetLabel) continue; // Filter by targetLabel
                if (relationshipLabel == "" || relationship.Key == relationshipLabel)
                {
                    queryResult += $"{node.Key} -> {relationship.Key} -> {relationship.Value}\n";
                }
            }
        }
    }
    private string LintString(string str) => str.Trim().ToLower();
    public static void UpdatePanel() => GetWindow<QueryPanel>().Repaint();
    public static void SetData(UKS_Data data) => QueryPanel.data = data;
}