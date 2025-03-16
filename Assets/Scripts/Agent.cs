using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private UKS_Data data;

    // public void Awake() => data = new UKS_Data(label, new(), new(), true);

    // public Agent(string label)
    // {
    //     data = new UKS_Data(label, new() { { UKS_Data.RootNodeLabel, new(UKS_Data.RootNodeLabel) } });
    //     // data = new UKS_Data(label, new() { { RootNodeLabel, new(RootNodeLabel) } }, new(), true);
    // }

    // public void AddRelationship(UKS_Node sourceNode, UKS_Node targetNode, UKS_Node relationshipNode)
    // {
    //     relationshipNode.relationships.Add(sourceNode.label, targetNode); // ex. is-a.relationships = { fido: dog }
    //     sourceNode.relationships.Add(relationshipNode.label, targetNode); // ex. fido.relationships = { is-a: dog }
    // }

    // Getters
    // public string GetLabel() => label;
    public UKS_Data GetData() => data;
    // public HashSet<UKS_Node> GetNodes() => data.nodes.Values;
    // public HashSet<UKS_Edge> GetEdges() => data.edges;

    // Adders
    // public void AddNode(UKS_Node node) => data.nodes.Add(node);
    // public void AddEdge(UKS_Edge edge) => data.edges.Add(edge);

    // Setters
    public void SetData(UKS_Data data) => this.data = data;

    // public override bool Equals(object obj)
    // {
    //     if (obj == null || GetType() != obj.GetType()) return false;
    //     Agent other = (Agent)obj;
    //     return data.Equals(other.data);
    // }

    // public override int GetHashCode() => data.GetHashCode();

    // public override string ToString() => label;

    
}
