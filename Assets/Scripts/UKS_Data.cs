using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class UKS_Data
{
    public static readonly string RootNodeLabel = "Thing";
    public static readonly string RootRelationshipLabel = "RelationshipType";
    public static readonly string InheritanceLabel = "is-a";
    public readonly string label;
    public readonly Dictionary<string, UKS_Node> nodes;

    public UKS_Data(string label)
    {
        this.label = label;
        nodes = new Dictionary<string, UKS_Node>{ { RootNodeLabel, new UKS_Node(RootNodeLabel) } };
        AddRelationship(RootRelationshipLabel, InheritanceLabel, RootNodeLabel);
        AddRelationship(InheritanceLabel, InheritanceLabel, RootRelationshipLabel);
    }
    [JsonConstructor]
    public UKS_Data(string label, Dictionary<string, UKS_Node> nodes)
    {
        this.label = label;
        this.nodes = nodes;
    }
    public void AddNode(string label, string inheritLabel)
    {
        nodes.Add(label, new(label));
        AddRelationship(label, InheritanceLabel, inheritLabel); // Add inheritance relationship to the root node
    }
    public void AddRelationship(string sourceLabel, string relationshipLabel, string targetLabel)
    {
        if (!nodes.ContainsKey(sourceLabel)) AddNode(sourceLabel, RootNodeLabel);
        if (!nodes.ContainsKey(targetLabel)) AddNode(targetLabel, RootNodeLabel);
        if (!nodes.ContainsKey(relationshipLabel)) AddNode(relationshipLabel, RootRelationshipLabel);
        nodes[sourceLabel].SetRelationship(relationshipLabel, targetLabel); // fido -> is-a -> dog
        nodes[relationshipLabel].SetRelationship(sourceLabel, targetLabel); // is-a -> fido -> dog
    }
}
