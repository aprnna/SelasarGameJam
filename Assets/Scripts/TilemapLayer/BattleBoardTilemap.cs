using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapLayer
{
    public class BattleBoardTilemap:TilemapLayer
    {
        [SerializeField] private BattleAreaTilemap _battleAreaTilemap;
        [SerializeField] private ColliderTilemap _colliderTilemap;
        private Dictionary<Vector3Int,UnitModel> _unit = new();

        private void Start()
        {
            if (_tilemap != null)
            {
                _tilemap.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        public UnitModel Build(Vector3 worldCoords, GameObject prefab, UnitData unitData)
        {
            Vector3Int baseCoords = _tilemap.WorldToCell(worldCoords);
            if(!IsEmpty(baseCoords)) return null;
            _colliderTilemap.SetCollider(baseCoords);
            var position = _tilemap.CellToWorld(baseCoords) + new Vector3(1 / 2f, 1 / 2f);
            var player = Instantiate( prefab, position, Quaternion.identity);
            var unit = new UnitModel(_tilemap, unitData, player ,baseCoords, position);
            _unit.Add(baseCoords, unit);
            // var playerController = player.transform.GetComponent<UnitController>();
            // playerController.Initialize(unit);
            return unit;
        }
        
        public List<Vector3> GetPlayerLocWorld()
        {
            BoundsInt bounds = _tilemap.cellBounds;
            List<Vector3> playerLoc = new List<Vector3>();
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = _tilemap.GetTile(pos);
                if (tile is TilePlayerSpawn)
                {
                    Vector3 worldPos = _tilemap.CellToWorld(pos) ;
                    playerLoc.Add(worldPos);
                }
            }
            return playerLoc;
        }
        public List<Vector3> GetEnemyLocWorld()
        {
            BoundsInt bounds = _tilemap.cellBounds;
            List<Vector3> enemyLoc = new List<Vector3>();
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = _tilemap.GetTile(pos);
                if (tile is TileEnemySpawn)
                {
                    Vector3 worldPos = _tilemap.CellToWorld(pos);
                    enemyLoc.Add(worldPos);
                }
            }
            return enemyLoc;
        }
        public bool IsEmpty(Vector3 worldCoords)
        {
            Vector3Int baseCoords = _tilemap.WorldToCell(worldCoords);
            if (_unit.ContainsKey(baseCoords))
            {
                return false;
            }
            return true;

        }

        public UnitModel GetUnit(Vector3 worldCoords)
        {
            Vector3Int baseCoords = _tilemap.WorldToCell(worldCoords);
            _unit.TryGetValue(baseCoords, out var data);
            return data;

        }
        public void RemoveUnit(UnitModel buildable)
        {
            _colliderTilemap.DestroyCollider(buildable.Coordinates);
            _unit.Remove(buildable.Coordinates);
            if (buildable.GameObject != null)
                Destroy(buildable.GameObject);
        }

        public Vector3 CellToWorld(Vector3Int baseCoords)
        {
            return _tilemap.CellToWorld(baseCoords);
        }
    }
}