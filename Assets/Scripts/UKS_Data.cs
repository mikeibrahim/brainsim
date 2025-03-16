using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class UKS_Data
{
    public static string RootNodeLabel = "UnknownObject";
    public string label;
    public Dictionary<string, UKS_Node> nodes;
    // public Dictionary<string, UKS_Edge> edges = new();
    // public bool active;

    public UKS_Data(string label) {
        this.label = label;
        nodes = new Dictionary<string, UKS_Node> { { RootNodeLabel, new UKS_Node(RootNodeLabel) } };
    }
    [JsonConstructor]
    public UKS_Data(string label, Dictionary<string, UKS_Node> nodes)
    // public UKS_Data(string label, Dictionary<string, UKS_Node> nodes, Dictionary<string, UKS_Edge> edges, bool active)
    {
        this.label = label;
        this.nodes = nodes;
        // this.edges = edges;
        // this.active = active;
    }

    // public override bool Equals(object obj)
    // {
    //     if (obj == null || GetType() != obj.GetType()) return false;
    //     UKS_Data other = (UKS_Data)obj;
    //     return label == other.label;
    // }

    // public override int GetHashCode() => label.GetHashCode();

    // public override string ToString() => label + (active ? " (active)" : " (saved)");
}
