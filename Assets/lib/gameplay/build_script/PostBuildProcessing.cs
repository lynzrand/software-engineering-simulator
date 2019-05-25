#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using System;

namespace Sesim.BuildScript
{
    class CustomBuildScripts
    {
        [MenuItem("Build/Build Windows")]
        public static void BuildWindows()
        {
            var options = new BuildPlayerOptions()
            {
                scenes = new[]{
                    "Assets/Scenes/SplashScreenScene.unity",
                    "Assets/Scenes/MainMenuScene.unity",
                    "Assets/Scenes/MainGameplayScene.unity",
                },
                locationPathName = "build/windows/SESim.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            };

            var report = BuildPipeline.BuildPlayer(options);

            // new PostBuildGameDataCopier().OnPostprocessBuild(report);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }

    class PostBuildGameDataCopier : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        // [PostProcessBuild(1)]
        public void OnPostprocessBuild(BuildReport report)
        {
            var gameData =
                new DirectoryInfo(Application.dataPath)
                .Parent
                .CreateSubdirectory("GameData");

            var outputGameData =
                new DirectoryInfo(report.summary.outputPath)
                .Parent
                .CreateSubdirectory("GameData");

            try
            {
                Directory.Delete(outputGameData.FullName, true);
            }
            catch (Exception e) { }
            Debug.Log($"Copying GameData files from {gameData} to {outputGameData}");

            CopyFilesRecursively(gameData, outputGameData);
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }
}
#endif
