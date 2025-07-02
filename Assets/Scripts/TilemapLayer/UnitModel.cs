using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapLayer
{
    public class UnitModel
    {
        public Tilemap ParentTilemap { get; private set; }
        public UnitData UnitData { get; private set; }
        public GameObject GameObject { get; private set; }
        public Vector3Int Coordinates { get; private set; }

        public UnitModel(Tilemap parentTilemap, UnitData unitData, GameObject prefab, Vector3Int coords)
        {
            ParentTilemap = parentTilemap;
            UnitData = unitData;
            GameObject = prefab;
            Coordinates = coords;
        }

    }
}