using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RelationshipPanel : Panel
{
    private static string sourceLabel = "";
    private static string relationshipLabel = "";
    private static string targetLabel = "";
    private static UKS_Data data;

    [MenuItem("Window/Relationship Panel")]
    public static void ShowWindow() => GetWindow<RelationshipPanel>();

    private void OnGUI()
    {
        if (data == null) EditorGUILayout.LabelField("No UKS Data selected.");
        else
        {
            sourceLabel = EditorGUILayout.TextField("Source Node", sourceLabel);
            relationshipLabel = EditorGUILayout.TextField("Relationship Type", relationshipLabel);
            targetLabel = EditorGUILayout.TextField("Target Node", targetLabel);
            if (GUILayout.Button("Add Relationship")) AddRelationship(sourceLabel, relationshipLabel, targetLabel);
        }
    }
    private void AddRelationship(string sourceLabel, string relationshipLabel, string targetLabel)
    {
        if (sourceLabel == "" || targetLabel == "" || relationshipLabel == "") { Debug.LogWarning("Please fill in all fields."); return; }
        sourceLabel = LintString(sourceLabel);
        targetLabel = LintString(targetLabel);
        relationshipLabel = LintString(relationshipLabel);
        data.AddRelationship(sourceLabel, relationshipLabel, targetLabel);
        UKSPanel.SaveDataToFile(data); // Save the updated data to file
    }
    private string LintString(string str) => str.Trim().ToLower();
    public static void SetData(UKS_Data data) => RelationshipPanel.data = data;
    public static void UpdatePanel() => GetWindow<RelationshipPanel>().Repaint();
}