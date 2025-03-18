using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyPanel : Panel
{
    private static string[] dataLabels = new string[] { };
    private static int dataIndex = -1;
    private static UKS_Data data;
    private static string nodeLabel = UKS_Data.RootNodeLabel;
    private static HierarchyNode rootHierarchyNode;
    private static Dictionary<string, bool> expandedNodes = new();
    // private readonly float highlightTime = 1f;
    // private Dictionary<UKS_Node, float> changedNodes = new();

    [MenuItem("Window/Hierarchy Panel")]
    public static void ShowWindow() => GetWindow<HierarchyPanel>();
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        if (dataLabels.Length == 0) EditorGUILayout.LabelField("No UKS Data available.");
        else
        {
            dataIndex = EditorGUILayout.Popup("Select UKS Data:", dataIndex, dataLabels);
            if (data == null) EditorGUILayout.LabelField("No UKS Data selected.");
            else
            {
                nodeLabel = EditorGUILayout.TextField("Select Node:", nodeLabel);
                if (rootHierarchyNode != null) DisplayHierarchyNode(rootHierarchyNode);
            }
        }
        if (EditorGUI.EndChangeCheck()) UpdatePanel();
        EditorGUILayout.EndVertical();
    }
    private void DisplayHierarchyNode(HierarchyNode hierarchyNode)
    {
        expandedNodes[hierarchyNode.label] = EditorGUILayout.Foldout(expandedNodes[hierarchyNode.label], hierarchyNode.label, true);
        if (!expandedNodes[hierarchyNode.label]) return;
        EditorGUI.indentLevel++;
        foreach (KeyValuePair<string, string> relationship in data.nodes[hierarchyNode.label].relationships) EditorGUILayout.LabelField($"{relationship.Key} -> {relationship.Value}");
        foreach (KeyValuePair<string, HierarchyNode> child in hierarchyNode.children)
        {
            HierarchyNode childNode = child.Value;
            if (!expandedNodes.ContainsKey(childNode.label)) expandedNodes[childNode.label] = false;
            DisplayHierarchyNode(childNode);
        }
        EditorGUI.indentLevel--;
    }
    private static void UpdateData()
    {
        if (dataLabels != null && dataLabels.Length > 0 && dataIndex >= 0 && dataIndex < dataLabels.Length)
        {
            UKS_Data newData = UKSPanel.GetData(dataLabels[dataIndex]);
            if (newData != data)
            {
                rootHierarchyNode = null;
                // Debug.Log($"Data changed: {data?.label} -> {newData?.label}");
                expandedNodes.Clear();
            }
            data = UKSPanel.GetData(dataLabels[dataIndex]);
        }
        else
        {
            dataIndex = -1;
            data = null;
            nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = null;
            // Debug.Log($"Data cleared: {data?.label}");
            expandedNodes.Clear();
        }
        RelationshipPanel.SetData(data);
        RelationshipPanel.UpdatePanel();
        QueryPanel.SetData(data);
        QueryPanel.UpdatePanel();
    }
    private static void UpdateNode()
    {
        if (data != null)
        {
            if (!data.nodes.ContainsKey(nodeLabel)) nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = new HierarchyNode(nodeLabel);
            expandedNodes[rootHierarchyNode.label] = true;
            // Debug.Log($"Creating hierarchy node for: {nodeLabel}");
            CreateHierarchyNode(rootHierarchyNode, data.nodes[nodeLabel]); // Thing, Thing
        }
        else
        {
            nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = null;
        }
    }
    private static void CreateHierarchyNode(HierarchyNode hierarchyNode, UKS_Node node)
    {
        foreach (KeyValuePair<string, string> relationship in data.nodes[UKS_Data.InheritanceLabel].relationships)
        {
            if (relationship.Value == node.label)
            {
                HierarchyNode childNode = new(relationship.Key);
                hierarchyNode.children.Add(relationship.Key, childNode);
                CreateHierarchyNode(childNode, data.nodes[relationship.Key]);
            }
        }
    }
    public static void UpdatePanel()
    {
        UpdateData();
        UpdateNode();
        GetWindow<HierarchyPanel>().Repaint();
    }
    public override void StartPlayMode() { }
    public override void EndPlayMode() { }
    // Getters
    public UKS_Data GetSelectedData() => data;
    // Setters
    public static void SetDataLabels(string[] dataLabels) => HierarchyPanel.dataLabels = dataLabels;
}