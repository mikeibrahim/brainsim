using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

public class UKSPanel : EditorWindow
{
    private readonly string savePath = "Assets/UKS_Data/";
    private List<UKS_Object> uksObjects = new();
    private List<UKS_Data> savedUKSData = new();

    [MenuItem("Window/UKS Panel")]
    public static void ShowWindow() => GetWindow<UKSPanel>("UKS Panel");

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh")) RefreshSavedData();
        EditorGUILayout.LabelField("Active UKS Objects:");
        foreach (UKS_Object uksObject in uksObjects)
        {
            EditorGUILayout.LabelField("Label: " + uksObject.GetLabel());
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Nodes: " + uksObject.GetNodes().Count);
            EditorGUILayout.LabelField("Edges: " + uksObject.GetEdges().Count);
            if (GUILayout.Button("Load Data")) LoadUKSData(uksObject);
            if (GUILayout.Button("Save Data")) SaveUKSData(uksObject);
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
            if (GUILayout.Button("Delete Data"))
            {
                string destination = savePath + data.label + ".json";
                if (File.Exists(destination)) File.Delete(destination);
                RefreshSavedData();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }

    // Pull data from the UKS_Data folder
    private void RefreshSavedData()
    {
        savedUKSData = new();
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        foreach (string file in Directory.GetFiles(savePath))
        {
            string json = File.ReadAllText(file);
            savedUKSData.Add(JsonUtility.FromJson<UKS_Data>(json));
        }
    }

    // Add data to the UKS_Data folder
    private void SaveUKSData(UKS_Object uksObject)
    {
        Debug.Log("Saved data for " + uksObject.GetLabel());
        string destination = savePath + uksObject.GetLabel() + ".json";
        UKS_Data data = uksObject.GetData();
        data.nodes ??= new List<UKS_Node>();
        data.edges ??= new List<UKS_Edge>();
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(destination, json);
        RefreshSavedData();
    }

    // Get data from the UKS_Data folder
    private void LoadUKSData(UKS_Object uksObject)
    {
        Debug.Log("Loaded data for " + uksObject.GetLabel());
        string destination = savePath + uksObject.GetLabel() + ".json";
        if (!File.Exists(destination))
        {
            Debug.LogWarning("File not found");
            return;
        }
        string json = File.ReadAllText(destination);
        uksObject.SetData(JsonUtility.FromJson<UKS_Data>(json));
    }

    private void StartPlayMode() => uksObjects = new(FindObjectsOfType<UKS_Object>());
    private void EndPlayMode() => uksObjects = new();

    // Handle play mode state changes
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode) StartPlayMode();
        else if (state == PlayModeStateChange.ExitingPlayMode) EndPlayMode();
    }
    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
}