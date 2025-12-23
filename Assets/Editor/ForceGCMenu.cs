using UnityEditor;
using UnityEngine;
using System;

public static class ForceGCMenu
{
    [MenuItem("Tools/Memory/Force GC")]
    private static void ForceGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}