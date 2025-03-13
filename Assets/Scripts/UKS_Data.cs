using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
public class UKS_Data
{
    public string label = "";
    public List<UKS_Node> nodes = new();
    public List<UKS_Edge> edges = new();

    public UKS_Data(string label) => this.label = label;

    public UKS_Data(string label, List<UKS_Node> nodes, List<UKS_Edge> edges)
    {
        this.label = label;
        this.nodes = nodes;
        this.edges = edges;
    }
}
