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

public class SaveController
{
    static readonly string SAVEFILE_DIR = "Saves/%ID%";


    SaveFile saveData;
    byte[] saveBuffer = null;

    CerasSerializer ceras = new CerasSerializer();
    SerializerConfig config = new SerializerConfig();

    string savePosition;

    // Start is called before the first frame update
    void Start()
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
                save.bin                    - actrual savefile
            .../
     */

    void resolveSavePos(Ulid saveId)
    {
        var dataPos = Application.dataPath;
        savePosition = Directory
            .GetParent(dataPos)
            .CreateSubdirectory("Saves")
            .CreateSubdirectory(saveId.ToString())
            .FullName;
    }

    async void SaveAsync()
    {
        resolveSavePos(saveData.id);
        var saveSize = ceras.Serialize<SaveFile>(saveData, ref saveBuffer);
        var saveFile = File.OpenWrite(savePosition + "/save.bin");
        await saveFile.WriteAsync(saveBuffer, 0, saveSize);
        saveFile.Close();

        var metaSize = ceras.Serialize<SaveMetadata>(saveData.Metadata, ref saveBuffer);
        var metaFile = File.OpenWrite(savePosition + "/meta.bin");
        await metaFile.WriteAsync(saveBuffer, 0, metaSize);
        metaFile.Close();
    }
}
