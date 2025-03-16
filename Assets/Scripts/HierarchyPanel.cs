using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class HierarchyPanel : Panel
{
    private static string[] dataLabels = new string[] { };
    private static int dataIndex = -1;
    private static UKS_Data data;
    private static string nodeLabel = "";
    private static HierarchyNode rootHierarchyNode;
    // private readonly float highlightTime = 1f;
    // private Dictionary<UKS_Node, float> changedNodes = new();

    [MenuItem("Window/Hierarchy Panel")]
    public static void ShowWindow() => GetWindow<HierarchyPanel>();
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        // [Dropdown] UKS Data selection
        EditorGUI.BeginChangeCheck();
        if (dataLabels.Length == 0) EditorGUILayout.LabelField("No UKS Data available.");
        else
        {
            // EditorGUI.BeginChangeCheck();
            dataIndex = EditorGUILayout.Popup("Select UKS Data:", dataIndex, dataLabels);
            // if (EditorGUI.EndChangeCheck()) UpdateData();
        }
        // [Input Field]
        if (data == null) EditorGUILayout.LabelField("No UKS Data selected.");
        else
        {
            // Root hierarchy node selection
            // EditorGUI.BeginChangeCheck();
            nodeLabel = EditorGUILayout.TextField("Select Node:", nodeLabel);
            // if (EditorGUI.EndChangeCheck()) UpdateNode();
            // Hierarchy display
            if (rootHierarchyNode != null) DisplayHierarchyNode(rootHierarchyNode);
        }
        if (EditorGUI.EndChangeCheck()) UpdatePanel();
        EditorGUILayout.EndVertical();
    }
    private void DisplayHierarchyNode(HierarchyNode hierarchyNode)
    {
        hierarchyNode.expanded = EditorGUILayout.Foldout(hierarchyNode.expanded, hierarchyNode.node.label, true);
        if (!hierarchyNode.expanded) return; // If not expanded, do not display children
        foreach (KeyValuePair<string, HierarchyNode> child in hierarchyNode.children)
        {
            HierarchyNode childNode = child.Value;
            childNode.expanded = EditorGUILayout.Foldout(childNode.expanded, childNode.node.label, true);
            EditorGUI.indentLevel++;
            if (childNode.expanded) DisplayHierarchyNode(childNode);
            EditorGUI.indentLevel--;
        }
    }
    private static void UpdateData()
    {
        // if (dataLabels.Length) dataIndex = System.Array.IndexOf(dataLabels, dataLabel);
        if (dataLabels.Length > 0 && dataIndex >= 0 && dataIndex < dataLabels.Length) data = UKSPanel.GetData(dataLabels[dataIndex]);
        else
        {
            dataIndex = -1;
            data = null;
            nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = null;
        }
        // data = UKSPanel.GetData(dataLabel);
    }
    private static void UpdateNode()
    {
        if (data != null)
        {
            if (!data.nodes.ContainsKey(nodeLabel)) nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = new HierarchyNode(data.nodes[nodeLabel], true);
        }
        else
        {
            nodeLabel = UKS_Data.RootNodeLabel;
            rootHierarchyNode = null;
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
    public static void DisplayStats()
    {
        Debug.Log($"Selected Data Index: {dataIndex}");
        Debug.Log($"Selected Data: {data?.label ?? "None"}");
        Debug.Log($"Selected Node: {nodeLabel}");
        Debug.Log($"Root Hierarchy Node: {rootHierarchyNode?.node.label ?? "None"}");
    }
    // Getters
    public UKS_Data GetSelectedData() => data;
    // Setters
    public static void SetDataLabels(string[] dataLabels)
    {
        HierarchyPanel.dataLabels = dataLabels;
    }
}