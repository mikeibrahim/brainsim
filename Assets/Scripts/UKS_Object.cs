using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// should this be initialized by the files or the editor
// i can store both the uks objs and the files 
public class UKS_Object : MonoBehaviour
{
    [SerializeField] private string label;
    private UKS_Data data;

    public void Awake() => data = new UKS_Data(label);

    public void AddEdge(UKS_Node sourceNode, UKS_Node targetNode, UKS_Node relationshipType)
    {
        UKS_Edge edge = new(sourceNode, targetNode, relationshipType);
        data.nodes.Add(sourceNode);
        data.nodes.Add(targetNode);
        data.edges.Add(edge);
        sourceNode.edges.Add(edge);
        targetNode.edges.Add(edge);
    }

    // Getters
    public string GetLabel() => label;
    public UKS_Data GetData() => data;
    public List<UKS_Node> GetNodes() => data.nodes;
    public List<UKS_Edge> GetEdges() => data.edges;

    // Adders
    public void AddNode(UKS_Node node) => data.nodes.Add(node);
    public void AddEdge(UKS_Edge edge) => data.edges.Add(edge);

    // Setters
    public void SetData(UKS_Data data) => this.data = data;
}
