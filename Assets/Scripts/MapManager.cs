﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
/// <summary>
/// Blocks generates space.
/// </summary>

public enum Block {
    nothing = 0,
    pipe = 0x0100,
    ///primarily, we could generate everything from those two.
}
/// <summary>
/// Tiles are generated by blocks.
/// </summary>
[Flags]
public enum Tile {
    nothing = 0,
    wall = 0x0100,
    hole = 0x0200,
    stationHole = 0x0201,
    station = 1,
    vendingMachine = 2
}
public struct MapBundle {
    //saves states of six direction(x+,x-,y+,y-,z+,z-) at every point.
    public MapBoxel[,,] v;
 
}
public struct MapBoxel {
    public Block b;
    public Tile[] t;
}
public class MapManager : MonoBehaviour {

    public event Action OnMapLoadComplete;
    public event Action OnMapCreateComplete;
    MapBundle m;
    PlayerController cMovement;
    [SerializeField] public TileDataset Dataset;
    [SerializeField] Text positionText;
    //public Vector3Int CurrentPosition;
    private void Awake() {

        cMovement = GetComponent<PlayerController>();




    }
    public int GetMapScale(int d) { return m.v.GetLength(d); }
    public void CreateNewMap() {
        m = new MapGenerator().CreateMap();
        OnMapCreateComplete?.Invoke();
    }
    public void LoadMap() {
        LoadMapData(); OnMapLoadComplete?.Invoke();
    }
    // Start is called before the first frame update
    void Start() {
        

    }
    public bool IsLineOfSight(Vector3Int v1, Vector3Int v2) {
        var v = v1 - v2;
        
        
        if (v.x * v.y != 0 || v.y * v.z != 0 || v.z * v.x != 0)
            return false;
        var m = v.magnitude;
        v.Clamp(Vector3Int.one * (-1), Vector3Int.one);     
        var f= Util.LHUnitVectorToFace(v);
        for (int i = 0; i < m; i++)
            if (GetTile(v2 + v * i, f) == Tile.wall)
                return false;
        return true;

    }
    public bool IsLineVisible(Vector3Int observer, Vector3Int line1, Vector3Int line2) {
        var v = line1 - line2;


        if (v.x * v.y != 0 || v.y * v.z != 0 || v.z * v.x != 0)
            return false;
        var m = v.magnitude;
        v.Clamp(Vector3Int.one * (-1), Vector3Int.one);
        var f = Util.LHUnitVectorToFace(v);
        for (int i = 0; i < m; i++)
            if (IsLineOfSight(observer,line2+v*i))
                return true;
        return false ;

    }
    public Vector3Int GetRandomAccessiblePoint() {
        List<Vector3Int> AccessibleBlocks = new List<Vector3Int>();
        for (int i = 0; i < m.v.GetLength(0); i++)
            for (int j = 0; j < m.v.GetLength(1); j++)
                for (int k = 0; k < m.v.GetLength(2); k++)
                    if (GetBlock(i, j, k) == Block.pipe)
                        AccessibleBlocks.Add(new Vector3Int(i, j, k));
        if (AccessibleBlocks.Count == 0) throw new ArgumentOutOfRangeException();
        return AccessibleBlocks[UnityEngine.Random.Range(0, AccessibleBlocks.Count)];
    }
    public Vector3Int GetPlayerSpawnPoint() {
        return GetRandomAccessiblePoint();
    }
    // Update is called once per frame

    public Block GetBlock(Vector3Int v) {
        if (IsOutofRange(m, v.x, v.y, v.z)) return Block.nothing;
        else
            return m.v[v.x, v.y, v.z].b;
    }
    public Block GetBlock(int i, int j, int k) {
        return GetBlock(new Vector3Int(i, j, k));
    }
    public Block GetBlockRelative(Vector3Int v, Entity e) {
        v += e.IdealPosition;
        if (IsOutofRange(m, v.x, v.y, v.z)) return Block.nothing;
        else
            return m.v[v.x, v.y, v.z].b;
    }

    public Block GetBlockRelative(int i, int j, int k, Entity e) {
        return GetBlockRelative(new Vector3Int(i, j, k), e);
    }

    public Tile GetTile(Vector3Int v, int f) {
        if (IsOutofRange(m, v)) return Tile.nothing;
        else
            return m.v[v.x, v.y, v.z].t[f];
    }
    public Tile GetTile(int i, int j, int k, int f) {
        if (IsOutofRange(m, i, j, k)) return Tile.nothing;
        else
            return m.v[i, j, k].t[f];
    }


    public Tile GetTileRelative(Vector3Int v, int f, Entity e) {
        v += e.IdealPosition;
        if (IsOutofRange(m, v)) return Tile.nothing;
        else
            return m.v[v.x, v.y, v.z].t[f];
    }
    public Tile GetTileRelative(int i, int j, int k, int f, Entity e) {
        return GetTileRelative(new Vector3Int(i, j, k), f, e);
    }
    public Tile GetTileRelative(Vector3Int v, int f, Vector3Int EntityPosition) {
        v += EntityPosition;
        if (IsOutofRange(m, v)) return Tile.nothing;
        else
            return m.v[v.x, v.y, v.z].t[f];
    }
    public Tile GetTileRelative(int i, int j, int k, int f, Vector3Int EntityPosition) {
        i += EntityPosition.x;
        j += EntityPosition.y;
        k += EntityPosition.z;
        if (IsOutofRange(m, i, j, k)) return Tile.nothing;
        else
            return m.v[i, j, k].t[f];
    }
    bool IsOutofRange(MapBundle m, int i, int j, int k) {
        try {
            var b = m.v[i, j, k];
        }
        catch (IndexOutOfRangeException e) {

            return true;
        }

        return false;

    }
    bool IsOutofRange(MapBundle m, Vector3Int v) {
        try {
            var b = m.v[v.x, v.y, v.z];
        }
        catch (IndexOutOfRangeException e) {

            return true;
        }

        return false;

    }
    public string SerializeMapData() {
        return JsonConvert.SerializeObject(m);
       
    }
    public void LoadMapData() {
        using (StreamReader SR = new StreamReader(Path.Combine(GameManager.Instance.AppdataPath, "Map.json"))) {
            string s = SR.ReadToEnd();
            m = JsonConvert.DeserializeObject<MapBundle>(s);
        }
        
        
    }
    
}
