using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class UKSPanel : Panel
{
    [SerializeField] private Agent agentPrefab;
    private static readonly string savePath = "Assets/UKS_Data/";
    private static Dictionary<string, Agent> agents = new();
    private static Dictionary<string, UKS_Data> savedData = new();
    private static Vector2 scrollPosition;

    [MenuItem("Window/UKS Panel")]
    public static void ShowWindow() => GetWindow<UKSPanel>("UKS Panel");
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        // [Button] Create agent
        if (Application.isPlaying && GUILayout.Button("Create New Agent"))
        {
            string label = GetUniqueDataName();
            Agent newAgent = new GameObject(label).AddComponent<Agent>();
            agents.Add(label, newAgent);
            newAgent.SetData(new(label));
        }
        // [List] Active agents
        if (agents.Count == 0) EditorGUILayout.LabelField("No active agents.");
        else
        {
            EditorGUILayout.LabelField("Agents:");
            foreach (Agent agent in agents.Values)
            {
                UKS_Data data = agent.GetData();
                EditorGUILayout.LabelField("Label: " + data.label);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Nodes: " + data.nodes.Count);
                if (GUILayout.Button("Save Agent to File")) SaveDataToFile(data);
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        // [Button] Create saved data
        if (GUILayout.Button("Create New Saved Data")) SaveDataToFile(new(GetUniqueDataName()));
        // [List] Saved data
        if (savedData.Count == 0) EditorGUILayout.LabelField("No Saved UKS Data.");
        else
        {
            EditorGUILayout.LabelField("Saved UKS Data:");
            foreach (UKS_Data data in savedData.Values.ToList())
            {
                if (agents.ContainsKey(data.label)) continue; // Skip if the data is already active in an agent
                EditorGUILayout.LabelField("Label: " + data.label);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Nodes: " + data.nodes.Count);
                if (Application.isPlaying && GUILayout.Button("Load Data to Agent")) LoadAgent(data);
                if (GUILayout.Button("Delete Data")) DeleteDataFile(data);
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        // [Button] Refresh
        if (GUILayout.Button("Refresh")) UpdatePanel();
        EditorGUILayout.EndScrollView();
    }
    private void DeleteDataFile(UKS_Data data)
    {
        string destination = savePath + data.label + ".json";
        if (File.Exists(destination))
        {
            File.Delete(destination);
            string metaFile = destination + ".meta";
            if (File.Exists(metaFile)) File.Delete(metaFile);
        }
        UpdatePanel();
    }
    private void SaveDataToFile(UKS_Data data)
    {
        if (agents.ContainsKey(data.label)) agents.Remove(data.label); // Remove from active agents if it exists
        string destination = savePath + data.label + ".json";
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        string json = Serialize(data);
        File.WriteAllText(destination, json);
        UpdatePanel();
    }
    private void LoadAgent(UKS_Data data)
    {
        Agent agent = Instantiate(agentPrefab);
        agent.SetData(data);
        agents.Add(data.label, agent);
    }
    private static string Serialize(UKS_Data data) => JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
    private static UKS_Data Deserialize(string json) => JsonConvert.DeserializeObject<UKS_Data>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
    private string GetUniqueDataName()
    {
        string baseName = "Agent_";
        int index = 0;
        string uniqueName = baseName + index;
        while (agents.ContainsKey(uniqueName) || savedData.ContainsKey(uniqueName))
        {
            index++;
            uniqueName = baseName + index;
        }
        return uniqueName;
    }
    // private void UpdateHierarchyPanel()
    // {
    //     string[] dataLabels = savedData.Keys.ToArray();
    //     GetWindow<HierarchyPanel>("Hierarchy Panel").SetDataLabels(dataLabels);
    //     // TODO: update all the variables on change
    // }
    public override void StartPlayMode() { }
    public override void EndPlayMode() => SaveAllAgents();
    private void SaveAllAgents()
    {
        foreach (Agent agent in agents.Values) SaveDataToFile(agent.GetData());
        agents.Clear();
    }
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        DisplayStats();
        HierarchyPanel.DisplayStats();
        UpdatePanel();
    }
    public static void UpdatePanel()
    {
        savedData = new();
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        foreach (string file in Directory.GetFiles(savePath, "*.json"))
        {
            string json = File.ReadAllText(file);
            UKS_Data data = Deserialize(json);
            savedData.Add(data.label, data);
        }
        HierarchyPanel.SetDataLabels(savedData.Keys.ToArray());
        HierarchyPanel.UpdatePanel();
        GetWindow<UKSPanel>().Repaint();
    }
    public static void DisplayStats() => Debug.Log($"Active Agents: {agents.Count}, Saved Data: {savedData.Count}");
    // Getters
    public static UKS_Data GetData(string label)
    {
        if (agents.ContainsKey(label)) return agents[label].GetData();
        if (savedData.ContainsKey(label)) return savedData[label];
        return null;
    }
}