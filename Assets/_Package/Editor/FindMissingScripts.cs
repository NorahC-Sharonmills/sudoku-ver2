using UnityEngine;
using UnityEditor;
using System.Reflection;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Window/_Package Tool")]
    public static void ShowWindow()
    {
        var _windows = EditorWindow.GetWindow(typeof(FindMissingScripts), true, "_Package Tool");
        _windows.minSize = new Vector2(400, 800);
        _windows.maxSize = new Vector2(400, 800);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Remove Missing Script", GUILayout.Height(50)))
        {
            ClearLog();
            RemoveAllMissingScript();
        }
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Active Firebase", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            var _defineArray = _defineAndroid.Split(';');
            _defineArray = _defineArray.Add("FIREBASE_ENABLE");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
#endif
        }
        if (GUILayout.Button("Remove Firebase", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (_defineAndroid.Contains("FIREBASE_ENABLE"))
            {
                _defineAndroid = _defineAndroid.Replace("FIREBASE_ENABLE", "");
                var _defineArray = _defineAndroid.Split(';');
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
                Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
            }
#endif
            //var _defineIOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Active Applovin", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            var _defineArray = _defineAndroid.Split(';');
            _defineArray = _defineArray.Add("APPLOVIN_ENABLE");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
#endif
        }
        if (GUILayout.Button("Remove Applovin", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (_defineAndroid.Contains("APPLOVIN_ENABLE"))
            {
                _defineAndroid = _defineAndroid.Replace("APPLOVIN_ENABLE", "");
                var _defineArray = _defineAndroid.Split(';');
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
                Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
            }
#endif
            //var _defineIOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Active IAP", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            var _defineArray = _defineAndroid.Split(';');
            _defineArray = _defineArray.Add("IAP_ENABLE");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
#endif
        }
        if (GUILayout.Button("Remove IAP", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            var _defineAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (_defineAndroid.Contains("IAP_ENABLE"))
            {
                _defineAndroid = _defineAndroid.Replace("IAP_ENABLE", "");
                var _defineArray = _defineAndroid.Split(';');
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, _defineArray);
                Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
            }
#endif
            //var _defineIOS = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Get All Define", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
#endif
        }

        if (GUILayout.Button("Remove All Define", GUILayout.Height(30)))
        {
            ClearLog();
#if UNITY_ANDROID
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
#endif
        }
        GUILayout.EndHorizontal();
    }
    private static void RemoveAllMissingScript()
    {
        GameObject[] go = Selection.gameObjects;
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        foreach (GameObject g in go)
        {
            FindInGOAndRemove(g);
            //PrefabUtility.ApplyObjectOverride(g, "Assets/_Asset/Assets/_Enemys", InteractionMode.AutomatedAction);
            EditorUtility.SetDirty(g);
        }
        AssetDatabase.Refresh();
        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }    

    // change script
    private static void ChangeInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        foreach (GameObject g in go)
        {
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    DestroyImmediate(components[i]);
                    //g.AddComponent<ItemControl>();
                }
            }
        }
    }

    // find on selected
    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        int go_count = 0, components_count = 0, missing_count = 0;
        foreach (GameObject g in go)
        {
            go_count++;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                components_count++;
                if (components[i] == null)
                {
                    missing_count++;
                    string s = g.name;
                    Transform t = g.transform;
                    while (t.parent != null)
                    {
                        s = t.parent.name + "/" + s;
                        t = t.parent;
                    }
                    Debug.Log(s + " has an empty script attached in position: " + i, g);
                }
            }
        }

        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }

    // find on selected and child
    private static int go_count = 0, components_count = 0, missing_count = 0;
    private static void FindInSelectedAndChil()
    {
        GameObject[] go = Selection.gameObjects;
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        foreach (GameObject g in go)
        {
            FindInGO(g);
        }
        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }

    private static void FindInGO(GameObject g)
    {
        go_count++;

        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Debug.Log(s + " has an empty script attached in position: " + i, g);
            }
        }
        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {   
            FindInGO(childT.gameObject);
        }
    }

    private static void FindInGOAndRemove(GameObject g)
    {
        go_count++;

        int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
        if (count != 0)
            Debug.Log($"Removed {count} missing scripts");

        foreach (Transform childT in g.transform)
        {
            FindInGOAndRemove(childT.gameObject);
        }

        Debug.Log(g.name);
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}