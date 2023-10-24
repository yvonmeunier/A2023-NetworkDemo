using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Editor
{
    public static class Pipeline
    {
        private static string _lastBuildFilenameWithPath;
        
        private enum NetworkMode
        {
            Server,
            Client,
            Host
        }
        
        [MenuItem("Pipeline/Build For/MacOS")]
        private static void BuildMacOs()
        {
            BuildGameFor(BuildTarget.StandaloneOSX);
        }
        
        [MenuItem("Pipeline/Build For/Windows")]
        private static void BuildWindows()
        {
            Assert.Fail("I'm sorry, is this some sort of peasant joke that I'm too rich to understand?");
        }
        
        [MenuItem("Pipeline/Run As/Host and Client")]
        private static void RunGameAsHostAndClient()
        {
            RunGameAs(NetworkMode.Host);
            RunGameAs(NetworkMode.Client);
        }
        
        [MenuItem("Pipeline/Run As/Host")]
        private static void RunGameAsHost()
        {
            RunGameAs(NetworkMode.Host);
        }
        
        [MenuItem("Pipeline/Run As/Client")]
        private static void RunGameAsClient()
        {
            RunGameAs(NetworkMode.Client);
        }
        
        [MenuItem("Pipeline/Run As/Server")]
        private static void RunGameAsServer()
        {
            RunGameAs(NetworkMode.Server);
        }

        [MenuItem("Pipeline/Build and Run As/Host and Client")]
        private static void BuildAndRunGameAsHostAndClient()
        {
            BuildGameFor(BuildTarget.StandaloneOSX);
            RunGameAs(NetworkMode.Host);
            RunGameAs(NetworkMode.Client);
        }

        private static void BuildGameFor(BuildTarget target, BuildOptions options = BuildOptions.None)
        {
            var scenesPath = Path.Combine(Application.dataPath, "_Project/Scenes");
            var scenes = Directory.GetFiles(scenesPath, "*.unity", SearchOption.AllDirectories)
                .Select(file => $"Assets/_Project/Scenes/{Path.GetFileName(file)}")
                .ToArray();
            var savePath = EditorUtility.SaveFolderPanel("Choose folder location for build", "Build", "");
            var filenameWithPath = savePath + "/Prototype.app";
            
            BuildPipeline.BuildPlayer(
                scenes, 
                filenameWithPath, 
                target,
                options);

            _lastBuildFilenameWithPath = $"{filenameWithPath}/Contents/MacOS/{Application.productName}";
        }
        
        private static void RunGameAs(NetworkMode networkMode)
        {
            Assert.IsNotNull(_lastBuildFilenameWithPath, "You must build the game first before you can run it.");
            
            var process = new Process();
            process.StartInfo.FileName = _lastBuildFilenameWithPath;
            process.StartInfo.Arguments = $"-mlapi {networkMode.ToString().ToLower()}";
            process.Start();
        }
    }
}
