using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

// Source: https://fargesportfolio.com/unity-generic-auto-build/
public class BuildCommand : MonoBehaviour
{
    const string androidKeystorePass = "";
    const string androidKeyaliasName = "";
    const string androidKeyaliasPass = "";

    static BuildTarget[] targetToBuildAll =
    {
        BuildTarget.Android,
        BuildTarget.StandaloneWindows64
    };

    private static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(Application.dataPath, "../Build/");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            return path;
        }
    }

    static int AndroidLastBuildVersionCode
    {
        get
        {
            return PlayerPrefs.GetInt("LastVersionCode", -1);
        }
        set
        {
            PlayerPrefs.SetInt("LastVersionCode", value);
        }
    }
    static string GetExtension(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";
            case BuildTarget.Android:
                return ".apk";
            default:
                return "";
        }
    }

    static BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        // Get build scenes
        List<string> listScenes = new List<string>();
        foreach (var s in EditorBuildSettings.scenes)
        {
            if (s.enabled)
                listScenes.Add(s.path);
        }

        buildPlayerOptions.scenes = listScenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;
        return buildPlayerOptions;
    }

    static void DefaultBuild(BuildTarget buildTarget)
    {
        BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

        string path = Path.Combine(BuildPathRoot, buildTarget.ToString());
        string name = PlayerSettings.productName + GetExtension(buildTarget);

        string defineSymbole = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole + ";BUILD");

        PlayerSettings.Android.keystorePass = androidKeystorePass;
        PlayerSettings.Android.keyaliasName = androidKeyaliasName;
        PlayerSettings.Android.keyaliasPass = androidKeyaliasPass;

        BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

        buildPlayerOptions.locationPathName = Path.Combine(path, name);
        buildPlayerOptions.target = buildTarget;

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole);

        if (buildTarget == BuildTarget.Android)
            AndroidLastBuildVersionCode = PlayerSettings.Android.bundleVersionCode;



        BuildPipeline.BuildPlayer(buildPlayerOptions);

    }

    [MenuItem("Build/Build Specific/Build Android")]
    static void BuildAndroid()
    {
        DefaultBuild(BuildTarget.Android);
    }

    [MenuItem("Build/Build Specific/Build Win32")]
    static void BuildWin32()
    {
        DefaultBuild(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Build/Build Specific/Build Win64")]
    static void BuildWin64()
    {
        DefaultBuild(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Build/Get Build Number")]
    static void BuildNumber()
    {
        Debug.Log("Current/Last: " + PlayerSettings.Android.bundleVersionCode + "/" + AndroidLastBuildVersionCode);
    }

    [MenuItem("Build/Build Number/Up Build Number")]
    static void BuildNumberUp()
    {
        PlayerSettings.Android.bundleVersionCode++;
        BuildNumber();
    }

    [MenuItem("Build/Build Number/Down Build Number")]
    static void BuildNumberDown()
    {
        PlayerSettings.Android.bundleVersionCode--;
        BuildNumber();
    }

    [MenuItem("Build/Build All")]
    static void BuildAll()
    {
        List<BuildTarget> buildTargetLeft = new List<BuildTarget>(targetToBuildAll);

        if (buildTargetLeft.Contains(EditorUserBuildSettings.activeBuildTarget))
        {
            DefaultBuild(EditorUserBuildSettings.activeBuildTarget);
            buildTargetLeft.Remove(EditorUserBuildSettings.activeBuildTarget);
        }

        foreach (var b in buildTargetLeft)
        {
            DefaultBuild(b);
        }
    }
}
