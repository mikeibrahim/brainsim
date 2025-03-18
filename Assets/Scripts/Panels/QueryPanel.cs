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
        // HashSet<UKS_Node> attributeNodes = new();
        // if (sourceLabel != "") attributeNodes.Add(data.nodes[sourceLabel]);
        // if (relationshipLabel != "") attributeNodes.Add(data.nodes[relationshipLabel]);
        // if (targetLabel != "") attributeNodes.Add(data.nodes[targetLabel]);

        /*
        scenario: sourceLabel = fido
            output: fido -> is-a -> dog
            output: fido -> has -> tail
            ...
        scenario: relationshipLabel = is-a
            output: fido -> is-a -> dog
            output: dog -> is-a -> Thing
        scenario: targetLabel = dog
            output: fido -> is-a -> dog
            output: rover -> is-a -> dog
        scenario: sourceLabel = fido, relationshipLabel = has
            output: fido -> has -> tail
            output: fido -> has -> brain
        scenario: relationshipLabel = is-a, targetLabel = dog
            output: fido -> is-a -> dog
            output: rover -> is-a -> dog
        scenario: sourceLabel = fido, targetLabel = dog
            output: fido -> is-a -> dog
        scenario: sourceLabel = fido, relationshipLabel = is-a, targetLabel = dog
            output: fido -> is-a -> dog
        */
        if (sourceLabel == "" && relationshipLabel == "" && targetLabel == "") { Debug.LogWarning("Please fill in at least one field."); return; }
        sourceLabel = LintString(sourceLabel);
        targetLabel = LintString(targetLabel);
        relationshipLabel = LintString(relationshipLabel);
        queryResult = ""; // Clear previous results
        foreach (KeyValuePair<string, UKS_Node> node in data.nodes)
        {
            if (sourceLabel != "" && node.Key != sourceLabel) continue; // Filter by sourceLabel
            if (relationshipLabel != "" && !node.Value.relationships.ContainsKey(relationshipLabel)) continue; // Filter by relationshipLabel
            // if (targetLabel != "" && !node.Value.relationships.Values.Contains(targetLabel)) continue; // Filter by targetLabel

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