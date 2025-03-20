using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcHoughDetector
{
    public static List<Feature> DetectFeatures(Texture2D texture, int threshold, double minLineLength, double maxLineGap)
    {
        // // Load image as Texture2D from Resources folder
        // Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        // if (texture == null)
        // {
        //     Debug.LogError($"Failed to load texture from Resources: {resourcePath}");
        //     return new List<Feature>();
        // }
        Debug.Log($"Texture loaded: {texture.width}x{texture.height}");
        using var image = OpenCvSharp.Unity.TextureToMat(texture); // Decode PNG byte array to Mat
        if (image == null || image.Empty())
        {
            Debug.LogError("Failed to decode texture to Mat");
            return new List<Feature>();
        }
        Debug.Log($"Mat created: {image.Width}x{image.Height}");

        // Preprocess image
        Cv2.GaussianBlur(image, image, new Size(5, 5), 1.5);
        Debug.Log("Image blurred: " + image.Width + "x" + image.Height);
        using Mat edges = new Mat();
        Cv2.Canny(image, edges, 100, 200);
        Debug.Log("Canny edges detected: " + edges.Width + "x" + edges.Height);

        // Detect lines (edges)
        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Math.PI / 180, threshold, minLineLength, maxLineGap);
        Debug.Log($"Hough lines detected: {lines.Length} lines");
        List<Feature> features = new List<Feature>();

        foreach (var line in lines)
        {
            features.Add(new EdgeStroke
            {
                StartX = line.P1.X,
                StartY = line.P1.Y,
                EndX = line.P2.X,
                EndY = line.P2.Y
            });
        }

        // Detect circles and extract arcs
        var circles = Cv2.HoughCircles(edges, HoughMethods.Gradient, 1, 20, 100, 30, 10, 100);
        foreach (var circle in circles)
        {
            List<float> angles = GetEdgeAngles(edges, circle);
            if (angles.Count < 10) continue; // Noise filter
            var arcSegments = FindArcSegments(angles, circle);
            features.AddRange(arcSegments);
        }

        Debug.Log($"Detected {features.Count} features");
        return features;
    }

    private static List<float> GetEdgeAngles(Mat edges, CircleSegment circle)
    {
        List<float> angles = new List<float>();
        byte[] edgeData = edges.ToBytes();

        float centerX = circle.Center.X;
        float centerY = circle.Center.Y;
        float radius = circle.Radius;
        float tolerance = radius * 0.1f;

        for (int y = 0; y < edges.Rows; y++)
        {
            for (int x = 0; x < edges.Cols; x++)
            {
                if (edgeData[y * edges.Cols + x] == 255)
                {
                    float dx = x - centerX;
                    float dy = y - centerY;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (Math.Abs(distance - radius) < tolerance)
                    {
                        float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
                        if (angle < 0) angle += 360;
                        angles.Add(angle);
                    }
                }
            }
        }
        return angles.OrderBy(a => a).ToList();
    }

    private static List<ArcStroke> FindArcSegments(List<float> angles, CircleSegment circle)
    {
        List<ArcStroke> segments = new List<ArcStroke>();
        if (angles.Count == 0) return segments;

        float maxGap = 10f;
        float startAngle = angles[0];
        float prevAngle = startAngle;

        for (int i = 1; i < angles.Count; i++)
        {
            float currentAngle = angles[i];
            float gap = Math.Min(
                Math.Abs(currentAngle - prevAngle),
                360 - Math.Abs(currentAngle - prevAngle)
            );

            if (gap > maxGap)
            {
                segments.Add(new ArcStroke
                {
                    CenterX = circle.Center.X,
                    CenterY = circle.Center.Y,
                    Radius = circle.Radius,
                    StartAngle = startAngle,
                    EndAngle = prevAngle
                });
                startAngle = currentAngle;
            }
            prevAngle = currentAngle;
        }

        segments.Add(new ArcStroke
        {
            CenterX = circle.Center.X,
            CenterY = circle.Center.Y,
            Radius = circle.Radius,
            StartAngle = startAngle,
            EndAngle = prevAngle
        });

        return segments;
    }

    public abstract class Feature { }
    public class EdgeStroke : Feature
    {
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float EndX { get; set; }
        public float EndY { get; set; }
    }
    public class ArcStroke : Feature
    {
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Radius { get; set; }
        public float StartAngle { get; set; } // Degrees
        public float EndAngle { get; set; }   // Degrees
    }
}