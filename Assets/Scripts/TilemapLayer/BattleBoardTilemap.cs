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

        public void Build(Vector3 worldCoords, GameObject prefab, UnitData unitData)
        {
            Vector3Int baseCoords = _tilemap.WorldToCell(worldCoords);
            if(!IsEmpty(baseCoords)) return;
            _colliderTilemap.SetCollider(baseCoords);
            var player = Instantiate( prefab, worldCoords, Quaternion.identity);
            var unit = new UnitModel(_tilemap, unitData, prefab ,baseCoords);
            _unit.Add(baseCoords, unit);
            var playerController = player.transform.GetComponent<UnitController>();
            playerController.Initialize(unit);
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
                    Vector3 worldPos = _tilemap.CellToWorld(pos) + new Vector3(1 / 2f, 1 / 2f);
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
                    Vector3 worldPos = _tilemap.CellToWorld(pos) + new Vector3(1 / 2f, 1 / 2f);
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
        private void RemoveUnit(UnitModel buildable)
        {
            _colliderTilemap.DestroyCollider(buildable.Coordinates);
            _unit.Remove(buildable.Coordinates);
            if (buildable.GameObject != null)
                Object.Destroy(buildable.GameObject);
        }
    }
}