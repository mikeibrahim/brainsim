using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Serializable]
public class UKS_Node
{
    public readonly string label;
    public readonly Dictionary<string, string> relationships = new();

    public UKS_Node(string label) => this.label = label;
    public void AddRelationship(string key, string value)
    {
        relationships.Add(key, value);
        Debug.Log($"Added relationship: {label} -> {key} -> {value}");
    }
    public void SetRelationship(string key, string value)
    {
        relationships[key] = value;
        Debug.Log($"Set relationship: {label} -> {key} -> {value}");
    }

    public void PrintRelationships()
    {
        Debug.Log($"Node: {label}");
        foreach (KeyValuePair<string, string> relationship in relationships)
        {
            Debug.Log($"  {relationship.Key} -> {relationship.Value}");
        }
    }
}
