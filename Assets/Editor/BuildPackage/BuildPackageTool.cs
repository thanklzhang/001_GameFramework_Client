using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildArg
{
    public bool isBuildPackage;
    public bool isBuildAB;
    public string productName;
    public string version;
    public BuildTarget buildTarget;
    public string channel;
    public string resVersion = "";
}

public class BuildPackageTool
{

    static BuildArg buildArgs;
    public static List<string> commandArgs = new List<string>()
    {
        "is_build_package",
        "is_build_ab",
        "product_name",
        "version",
        "platform",
        "channel",
        "res_version"
    };

    [MenuItem("Build/Build Package")]
    public static void BuildPackage()
    {
        // 解析命令行参数
        buildArgs = new BuildArg();

        buildArgs.isBuildPackage = false;
        buildArgs.isBuildAB = false;
        buildArgs.buildTarget = BuildTarget.StandaloneWindows64;

        string[] args = System.Environment.GetCommandLineArgs();
        foreach (var s in args)
        {
            Debug.Log("argsargs : " + s);
            if (s.Contains("--") && s.Contains(":"))
            {
                var ss = s.Split(':');
                string value = ss[1];
                var currCmd = ss[0].Split("--")[1].Trim();

                Debug.Log("arg : cmd : " + currCmd + " , value : " + value);
                HandleCommand(currCmd, value);
            }
            //foreach (var currCmd in commandArgs)
            //{
            //    if (s.Contains("--" + currCmd + ":"))
            //    {
            //        string value = s.Split(':')[1];
            //        Debug.Log("arg : cmd : " + currCmd + " , value : " + value);
            //        HandleCommand(currCmd, value);
            //        break;
            //    }
            //}
        }

        //buldArgs 已经填充好
        StartBuild();
    }

    static void StartBuild()
    {
        Logx.Log("Build 包版本 : " + buildArgs.version);
        Logx.Log("Build 平台 : " + buildArgs.buildTarget);

        BuildPlayerOptions opt = new BuildPlayerOptions();
        opt.scenes = new string[] { "Assets/Scenes/Startup.unity" };
        //opt.locationPathName = Application.dataPath + "/../Bin/test.apk";

        if (buildArgs.buildTarget == BuildTarget.StandaloneWindows64)
        {
            opt.locationPathName = "E:\\E_Place_For_Use\\GamePacageOutPath\\Windows\\test001.exe";
        }
        else if (buildArgs.buildTarget == BuildTarget.Android)
        {
            opt.locationPathName = "E:\\E_Place_For_Use\\GamePacageOutPath\\Android\\test001.apk";
        }
        else if (buildArgs.buildTarget == BuildTarget.iOS)
        {
            //opt.locationPathName = "E:\\E_Place_For_Use\\Apk\\test001.apk";
        }


        opt.target = buildArgs.buildTarget;
        opt.options = BuildOptions.None;

        PlayerSettings.productName = buildArgs.productName;
        PlayerSettings.bundleVersion = buildArgs.version;

        if (buildArgs.isBuildAB)
        {
            Logx.Log("开始 build AB");
            ABPackageTool.BuildAssetResource(buildArgs.buildTarget, buildArgs.resVersion);
        }

        if (buildArgs.isBuildPackage)
        {
            Logx.Log("开始 build 包");
            BuildPipeline.BuildPlayer(opt);
        }

        Debug.Log("Build All Done!");
    }

    public static void HandleCommand(string cmd, string value)
    {

        if (cmd == "is_build_package")
        {
            buildArgs.isBuildPackage = bool.Parse(value);
        }
        else if (cmd == "is_build_ab")
        {
            buildArgs.isBuildAB = bool.Parse(value);
        }
        else if (cmd == "product_name")
        {
            buildArgs.productName = value;
        }
        else if (cmd == "version")
        {
            //PlayerSettings.bundleVersion = value;
            buildArgs.version = value;
        }
        else if (cmd == "platform")
        {
            if (value == "Windows")
            {
                buildArgs.buildTarget = BuildTarget.StandaloneWindows64;
            }
            else if (value == "Android")
            {
                buildArgs.buildTarget = BuildTarget.Android;
            }
            else if (value == "IOS")
            {
                buildArgs.buildTarget = BuildTarget.iOS;
            }
        }
        else if (cmd == "channel")
        {
            buildArgs.channel = value;
        }
        else if (cmd == "res_version")
        {
            buildArgs.resVersion = value;
        }
    }

}
