using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
public class UKS_Node
{
    public string label;
    public List<UKS_Edge> edges = new();

    public UKS_Node(string label) => this.label = label;
}
