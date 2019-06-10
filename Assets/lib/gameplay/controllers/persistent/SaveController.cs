using Ceras;
using Ceras.Formatters;
using Ceras.Resolvers;
using Sesim;
using Sesim.Models;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

namespace Sesim.Game.Controllers.Persistent
{
    public sealed class SaveController
    {
        private const string MetadataFilename = "meta.bin";
        private const string SavefileFilename = "save.bin";
        static readonly string SAVEFILE_DIR = "Saves/%ID%";

        private static SaveController instance;
        public static SaveController Instance
        {
            get
            {
                if (instance == null) instance = new SaveController();
                return instance;
            }
        }

        public static SaveController __DebugNewInstance() => new SaveController();

        public List<SaveMetadata> saveMetas = new List<SaveMetadata>();

        public SaveFile saveFile;

        public bool shouldCompanyLoadSavefile = false;
        public bool working = false;
        byte[] saveBuffer = null;

        public CerasSerializer ceras = new CerasSerializer();
        public SerializerConfig config = new SerializerConfig();

        public string savePosition;

        // Start is called before the first frame update
        private SaveController()
        {
            config.OnResolveFormatter.Add((s, t) =>
            {
                if (t == typeof(Ulid))
                    return new Sesim.Helpers.Ceras.UlidFormatter();
                else
                    return null;
            });
            config.VersionTolerance.Mode = VersionToleranceMode.Standard;

            // DontDestroyOnLoad(this.gameObject);
        }

        /*
            Save folders are structured like this:

            Saves/
                <Ulid of this save>/
                    meta.bin                    - savefile metadata
                    save.bin                    - actual savefile
                .../
         */

        string resolveSavePos(Ulid saveId)
        {
            var dataPos = Application.dataPath;
            return Directory
                .GetParent(dataPos)
                .CreateSubdirectory("Saves")
                .CreateSubdirectory(saveId.ToString())
                .FullName;
        }
        string SaveFolderPos
        {
            get
            {
                var dataPos = Application.dataPath;
                return Directory
                    .GetParent(dataPos)
                    .CreateSubdirectory("Saves")
                    .FullName;
            }
        }

        public List<SaveMetadata> loadSaveMetas()
        {
            var saveFolderPos = SaveFolderPos;
            var saveFolders = Directory.EnumerateDirectories(saveFolderPos);
            this.saveMetas.Clear();
            foreach (var folder in saveFolders)
            {
                try
                {
                    var metaFile = File.ReadAllBytes(Path.Combine(folder, MetadataFilename));
                    var metadata = ceras.Deserialize<SaveMetadata>(metaFile);
                    saveMetas.Add(metadata);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Metadata file \"meta.bin\" cannot be read in {folder}. Is this save folder corrupted?");
                    Debug.LogWarning(e);
                }
            }

            Debug.Log(String.Join("\n", this.saveMetas.ConvertAll<String>(m => m.ToString())));

            return saveMetas;
        }

        public async Task SaveAsync()
        {
            working = true;
            this.savePosition = resolveSavePos(this.saveFile.id);
            var saveSize = ceras.Serialize<SaveFile>(this.saveFile, ref saveBuffer);
            var saveFile = File.OpenWrite(Path.Combine(savePosition, SavefileFilename));
            await saveFile.WriteAsync(saveBuffer, 0, saveSize);
            saveFile.Close();

            SaveMetadata metadata = this.saveFile.Metadata;
            metadata.saveTime = DateTime.Now;
            var metaSize = ceras.Serialize<SaveMetadata>(metadata, ref saveBuffer);
            var metaFile = File.OpenWrite(Path.Combine(savePosition, MetadataFilename));
            await metaFile.WriteAsync(saveBuffer, 0, metaSize);
            metaFile.Close();

            Debug.Log($"Successfully saved SaveFile#{this.saveFile.id}. Save file size {saveSize}, metadata file size {metaSize}.");

            working = false;
        }

        public void Load(Ulid id)
        {
            working = true;
            resolveSavePos(id);
            var saveBuffer = File.ReadAllBytes(
                Path.Combine(resolveSavePos(id), SavefileFilename)
            );
            var saveFile = ceras.Deserialize<SaveFile>(saveBuffer);
            this.saveFile = saveFile;
            this.shouldCompanyLoadSavefile = true;

            Debug.Log($"Successfully loaded SaveFile#{this.saveFile.id}.");

            working = false;
        }
    }
}
