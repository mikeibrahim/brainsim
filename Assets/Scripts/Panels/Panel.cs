using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using System.Runtime.InteropServices;

public class Panel : EditorWindow
{
    public virtual void StartPlayMode() {}

    public virtual void EndPlayMode() {}

    // Handle play mode state changes
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode) StartPlayMode();
        else if (state == PlayModeStateChange.ExitingPlayMode) EndPlayMode();
    }
    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
}