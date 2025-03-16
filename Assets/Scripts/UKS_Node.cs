using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class UKS_Node
{
    public string label;
    public Dictionary<string, UKS_Node> relationships = new();

    public UKS_Node(string label) => this.label = label;

    // public override bool Equals(object obj)
    // {
    //     if (obj == null || GetType() != obj.GetType()) return false;
    //     UKS_Node other = (UKS_Node)obj;
    //     return label == other.label;
    // }

    // public override int GetHashCode() => label.GetHashCode();

    // public override string ToString() => label;
}
