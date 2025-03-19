using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MNISTPanel : EditorWindow
{
    private static Texture2D canvas;
    private static Vector2Int canvasSize = new Vector2Int(28, 28);
    private static int size = 10;
    private static Vector2 scrollPos;
    private static Dictionary<string, Texture2D> savedImages = new Dictionary<string, Texture2D>();
    private static int carouselIndex = 0;
    private static int carouselSize = 5;
    private static string labelInput = "";
    private static readonly string savePath = "Assets/MNIST_Images/";

    [MenuItem("Window/MNIST Panel")]
    public static void ShowWindow() => GetWindow<MNISTPanel>("Pixel Canvas");

    private void OnEnable()
    {
        canvas = new(canvasSize.x, canvasSize.y) { filterMode = FilterMode.Point };
        ClearCanvas();
        UpdateSavedImages();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Pixel Canvas", EditorStyles.boldLabel);
        Rect canvasRect = GUILayoutUtility.GetRect(canvasSize.x * size, canvasSize.y * size);
        GUI.DrawTexture(canvasRect, canvas, ScaleMode.ScaleToFit);

        Event e = Event.current;
        if (e.type == EventType.MouseDrag || e.type == EventType.MouseDown)
        {
            if (canvasRect.Contains(e.mousePosition))
            {
                float squareSize = Mathf.Min(canvasRect.size.x, canvasRect.size.y);
                Vector2 squareTopLeft = new((canvasRect.size.x - squareSize) / 2, (canvasRect.size.y - squareSize) / 2);
                Vector2 relativeMousePos = e.mousePosition - squareTopLeft - canvasRect.position;
                Vector2 normalizedPos = relativeMousePos / squareSize;
                int x = Mathf.FloorToInt(normalizedPos.x * canvasSize.x);
                int y = Mathf.FloorToInt((1 - normalizedPos.y) * canvasSize.y);
                if (x >= 0 && x < canvasSize.x && y >= 0 && y < canvasSize.y) canvas.SetPixel(x, y, Color.black);
                canvas.Apply();
                Repaint();
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Canvas")) ClearCanvas();
        GUI.enabled = !string.IsNullOrEmpty(labelInput);
        if (GUILayout.Button("Save Image")) SaveImage();
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Image Label (required):", GUILayout.Width(100));
        labelInput = EditorGUILayout.TextField(labelInput);
        EditorGUILayout.EndHorizontal();

        GUI.enabled = savedImages.ContainsKey(labelInput);
        if (GUILayout.Button("Delete Image")) DeleteImage(labelInput);
        GUI.enabled = true;

        if (GUILayout.Button("Send to UKS_Data")) SendToUKSData();
        if (savedImages.Count > 0)
        {
            GUILayout.Label("Saved Images", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("◄", GUILayout.Width(30)))
                carouselIndex = (carouselIndex - 1 + savedImages.Count) % savedImages.Count;

            int startIndex = carouselIndex % savedImages.Count; // Ensure startIndex wraps around
            int displayedCount = 0;
            var imageKeys = savedImages.Keys.ToList(); // Get list of keys for consistent indexing

            while (displayedCount < carouselSize && displayedCount < savedImages.Count)
            {
                int currentIndex = (startIndex + displayedCount) % savedImages.Count;
                string key = imageKeys[currentIndex];
                Texture2D texture = savedImages[key];

                Rect imageRect = GUILayoutUtility.GetRect(50, 50);
                if (GUI.Button(imageRect, texture, GUI.skin.box))
                {
                    SetCanvasToImage(texture);
                    labelInput = key;
                }
                GUI.Label(new Rect(imageRect.x, imageRect.y + 50, 50, 20), key);

                displayedCount++;
            }

            if (GUILayout.Button("►", GUILayout.Width(30)))
                carouselIndex = (carouselIndex + 1) % savedImages.Count;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    void ClearCanvas()
    {
        for (int x = 0; x < canvasSize.x; x++) for (int y = 0; y < canvasSize.y; y++) canvas.SetPixel(x, y, Color.white);
        canvas.Apply();
    }

    void SaveImage()
    {
        if (string.IsNullOrEmpty(labelInput)) return;

        Texture2D savedCanvas = new Texture2D(canvasSize.x, canvasSize.y);
        savedCanvas.SetPixels(canvas.GetPixels());
        savedCanvas.filterMode = FilterMode.Point;
        savedCanvas.Apply();

        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
        string filePath = $"{savePath}{labelInput}.png";
        File.WriteAllBytes(filePath, savedCanvas.EncodeToPNG());
        AssetDatabase.Refresh();

        savedImages[labelInput] = savedCanvas;
        labelInput = "";
        UpdatePanel();
    }

    void DeleteImage(string label)
    {
        if (savedImages.ContainsKey(label))
        {
            string filePath = $"{savePath}{label}.png";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                string metaFile = filePath + ".meta";
                if (File.Exists(metaFile)) File.Delete(metaFile);
            }
            savedImages.Remove(label);
            UpdatePanel();
        }
    }

    void SetCanvasToImage(Texture2D image)
    {
        canvas.SetPixels(image.GetPixels());
        canvas.Apply();
        Repaint();
    }

    void SendToUKSData()
    {
        Debug.Log("Sending image to UKS_Data...");
    }

    private static void UpdateSavedImages()
    {
        savedImages = new Dictionary<string, Texture2D>();
        if (!Directory.Exists(savePath)) return;

        foreach (string file in Directory.GetFiles(savePath, "*.png"))
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(canvasSize.x, canvasSize.y);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Point;
            savedImages[fileName] = texture;
        }
    }

    public static void UpdatePanel()
    {
        UpdateSavedImages();
        GetWindow<MNISTPanel>().Repaint();
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() => UpdatePanel();
}