﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public enum OpDirection {Front, Back, Omni}

[CreateAssetMenu(fileName = "OpData", menuName = "ScriptableObjects/OperatorData", order = 1)]
public class OperatorDataset : ScriptableObject
{

    public List<OperatorData> Dataset;
    [ContextMenu("Save To File...")]
    public void SaveToFile() {
        using (StreamWriter SW = new StreamWriter(Path.Combine(System.Environment.CurrentDirectory,"OperatorDataset.json"))) {
            
            var js = JsonSerializer.Create();
            js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            js.ContractResolver.ResolveContract(typeof(Sprite));
            js.Serialize(SW,Dataset);
        }
    }
    [ContextMenu("Load From File...")]
    public void LoadFromFile() {
        using (StreamReader SW = new StreamReader(Path.Combine(System.Environment.CurrentDirectory, "OperatorDataset.json"))) {
            Dataset = JsonConvert.DeserializeObject<List<OperatorData>>(SW.ReadToEnd());

        }
    }
}
[System.Serializable]
public struct OperatorData {
    public string Name;
    [TextArea(5, 10)]
    public string Description;
    public OpDirection Direction;
    public List<Command> Commands;
    public List<string> Variants;
    public int Value1;
    public int Value2;
    public Sprite Icon;
    public int Time;
}