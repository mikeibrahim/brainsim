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
    public string label;
    public Dictionary<string, HierarchyNode> children;

    public HierarchyNode(string label)
    {
        this.label = label;
        children = new Dictionary<string, HierarchyNode>();
    }
}