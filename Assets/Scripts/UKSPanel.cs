using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

public class UKSPanel : Panel
{
    private readonly string savePath = "Assets/UKS_Data/";
    private List<UKS_Object> uksObjects = new();
    private List<UKS_Data> savedUKSData = new();

    [MenuItem("Window/UKS Panel")]
    public static void ShowWindow() => GetWindow<UKSPanel>("UKS Panel");

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Active UKS Objects:");
        foreach (UKS_Object uksObject in uksObjects)
        {
            UKS_Data data = uksObject.GetData();
            EditorGUILayout.LabelField("Label: " + data.label);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Nodes: " + data.nodes.Count);
            EditorGUILayout.LabelField("Edges: " + data.edges.Count);
            if (GUILayout.Button("Load Data")) uksObject.SetData(GetUKSData(data));
            if (GUILayout.Button("Save Data")) SetUKSData(data);
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        EditorGUILayout.LabelField("Saved UKS Data:");
        foreach (UKS_Data data in savedUKSData)
        {
            EditorGUILayout.LabelField("Label: " + data.label);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Nodes: " + (data?.nodes?.Count ?? 0));
            EditorGUILayout.LabelField("Edges: " + (data?.edges?.Count ?? 0));
            if (GUILayout.Button("Delete Data")) DeleteData(data);
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        if (GUILayout.Button("Refresh")) RefreshSavedData();
    }

    // Pull all data from the UKS_Data folder
    private void DeleteData(UKS_Data data)
    {
        string destination = savePath + data.label + ".json";
        if (File.Exists(destination)) File.Delete(destination);
        RefreshSavedData();
    }

    private void RefreshSavedData()
    {
        savedUKSData = new();
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        foreach (string file in Directory.GetFiles(savePath, "*.json"))
        {
            string json = File.ReadAllText(file);
            Debug.Log("Loaded data from " + file);
            Debug.Log(json);
            savedUKSData.Add(Deserialize(json));
        }
    }

    // Add data to the UKS_Data folder
    private void SetUKSData(UKS_Data data)
    {
        Debug.Log("Saved data for " + data.label);
        string destination = savePath + data.label + ".json";
        // data.nodes ??= new List<UKS_Node>();
        // data.edges ??= new List<UKS_Edge>();
        string json = Serialize(data);
        File.WriteAllText(destination, json);
        RefreshSavedData();
    }

    // Get data from the UKS_Data folder
    private UKS_Data GetUKSData(UKS_Data data)
    {
        Debug.Log("Loaded data for " + data.label);
        string destination = savePath + data.label + ".json";
        if (!File.Exists(destination))
        {
            Debug.LogWarning("File not found");
            return null;
        }
        string json = File.ReadAllText(destination);
        return Deserialize(json);
    }

    public override void StartPlayMode() => uksObjects = new(FindObjectsOfType<UKS_Object>());
    public override void EndPlayMode() => uksObjects = new();
    private string Serialize(UKS_Data data) => JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
    private UKS_Data Deserialize(string json) => JsonConvert.DeserializeObject<UKS_Data>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

    // Getters
    public List<UKS_Object> GetUKSObjects() => uksObjects;
    public List<UKS_Data> GetSavedUKSData() => savedUKSData;
}