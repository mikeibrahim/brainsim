using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class HierarchyNode
{
    public UKS_Node node;
    public Dictionary<string, HierarchyNode> children;
    public bool expanded;

    public HierarchyNode(UKS_Node node, bool expanded)
    {
        this.node = node;
        children = new Dictionary<string, HierarchyNode>();
        this.expanded = expanded;
    }
}