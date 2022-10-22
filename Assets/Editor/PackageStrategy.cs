﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public enum StrategyType
{
    //目录下所有的文件 包括所有层子文件夹 每个文件一个包
    OneByOneFile = 1,
    //该目录所有文件打成一个包
    AllInOneByFolder = 2,
    //该目录所有一级文件夹打成一个包
    AllInOneBySubFolder = 3,
}

public class StrategyInfo
{
    public string path;
    public StrategyType type;
}

public class PackageStrategy
{
    List<StrategyInfo> strategyList = new List<StrategyInfo>();
    public List<AssetBundleBuild> StartParse()
    {

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Models",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Prefabs/UI",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Scene",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "SourceRes/Models/Role",
            type = StrategyType.AllInOneBySubFolder
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Prefabs/Effects",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Textures/BG",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/Textures/Item",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "SourceRes/Models/Build/Model",
            type = StrategyType.AllInOneByFolder
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/TableData",
            type = StrategyType.OneByOneFile
        });

        strategyList.Add(new StrategyInfo()
        {
            path = Const.projectRootPath + "/" + "BuildRes/BattleTriggerConfig",
            type = StrategyType.OneByOneFile
        });


        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        for (int i = 0; i < strategyList.Count; i++)
        {
            var strategyInfo = strategyList[i];
            var currBuildList = this.Parse(strategyInfo.path, strategyInfo.type);
            buildList.AddRange(currBuildList);
        }
        return buildList;
    }

    public List<AssetBundleBuild> Parse(string path, StrategyType type)
    {
        var buildList = new List<AssetBundleBuild>();
        if (type == StrategyType.OneByOneFile)
        {
            buildList = HandleOneByOneFile(path, type);
        }
        if (type == StrategyType.AllInOneByFolder)
        {
            buildList = HandleAllInOneByFolder(path, type);
        }
        if (type == StrategyType.AllInOneBySubFolder)
        {
            buildList = AllInOneBySubFolder(path, type);
        }

        return buildList;
    }
    //目录下所有的文件 包括所有层子文件夹 每个文件一个包
    public List<AssetBundleBuild> HandleOneByOneFile(string path, StrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var ext = Path.GetExtension(file);
            if (!IgnorePackageExt.IsCanPackageAsset(ext))
            {
                continue;
            }

            AssetBundleBuild build = new AssetBundleBuild();

            var abName = file.Replace(ext, Const.ExtName).Replace("\\", "/");

            build.assetBundleName = abName;
            var assetName = file.Replace("\\", "/");
            build.assetNames = new string[] { assetName };
            buildList.Add(build);
        }

        return buildList;
    }

    //该目录所有文件打成一个包
    public List<AssetBundleBuild> HandleAllInOneByFolder(string path, StrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        //var abName = path.Replace(Const.projectRootPath + "/", "") + Const.ExtName;
        AssetBundleBuild build = new AssetBundleBuild();
        var bundleName = path.Replace("\\", "/");
        List<string> assetNameList = new List<string>();
        build.assetBundleName = bundleName + Const.ExtName;

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var ext = Path.GetExtension(file);

            if (!IgnorePackageExt.IsCanPackageAsset(ext))
            {
                continue;
            }

            var assetName = file.Replace("\\", "/");
            assetNameList.Add(assetName);
            //build.assetNames = new string[] { assetName };
            //buildList.Add(build);
        }

        build.assetNames = assetNameList.ToArray();
        buildList.Add(build);

        return buildList;
    }

    //该目录所有一级文件夹 每个文件夹打成一个包
    public List<AssetBundleBuild> AllInOneBySubFolder(string path, StrategyType type)
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

        var dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i];
            var buildRange = HandleAllInOneByFolder(dir, StrategyType.AllInOneByFolder);
            buildList.AddRange(buildRange);
        }

        return buildList;
    }
}

