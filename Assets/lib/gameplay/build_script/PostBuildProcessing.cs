#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Sesim.BuildScript
{
    class CustomBuildScripts
    {
        [MenuItem("Build/Build Windows and Pack")]
        public static void BuildWindowsPack()
        {
            BuildWindows(true);
        }

        [MenuItem("Build/Build Windows")]
        public static void BuildWindowsNoPack()
        {
            BuildWindows(false);
        }

        public static void BuildWindows(bool pack)
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

            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
                return;
            }

            try
            {
                ZipBuildArtifacts(summary.outputPath);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static void ZipBuildArtifacts(string outputExecutable)
        {
            var outputFolder = new DirectoryInfo(outputExecutable).Parent;
            var buildFolder = outputFolder.Parent;

            // Read git commit count
            int gitCommitCount = -1;
            try
            {
                var gitCountCommitInfo = new ProcessStartInfo("git", "rev-list --count HEAD")
                {
                    WorkingDirectory = outputFolder.FullName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                var gitProcess = Process.Start(gitCountCommitInfo);
                gitCommitCount = int.Parse(gitProcess.StandardOutput.ReadToEnd());
                gitProcess.WaitForExit();
                Debug.Assert(gitProcess.ExitCode == 0);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to start git.");
            }

            // Read git commit sha1
            String commitId = "";
            {
                var gitCommitIdInfo = new ProcessStartInfo("git", "rev-parse HEAD")
                {
                    WorkingDirectory = outputFolder.FullName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                gitCommitIdInfo.WorkingDirectory = outputFolder.FullName;
                var gitProcess = Process.Start(gitCommitIdInfo);
                commitId = gitProcess.StandardOutput.ReadToEnd();
                gitProcess.WaitForExit();
                Debug.Assert(gitProcess.ExitCode == 0);
            }

            var zipName = $"build_windows_x64_{Application.version}b{gitCommitCount}_{commitId.Substring(0, 8)}.zip";

            try
            {
                var _7zInfo = new ProcessStartInfo("7z", $"a ../{zipName} ./*")
                {
                    WorkingDirectory = outputFolder.FullName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                var _7zip = Process.Start(_7zInfo);
                _7zip.WaitForExit();
                Debug.Assert(_7zip.ExitCode == 0);
                _7zip.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to start 7zip.");
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
            catch (Exception e) { Debug.Log(e); }
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
