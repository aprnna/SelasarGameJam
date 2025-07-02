using System.Collections.Generic;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapLayer
{
    public class BattleAreaTilemap:TilemapLayer
    {
        [SerializeField] private GameObject _highlightContainer;
        [SerializeField] private GameObject _highlight;
        
        private HashSet<Vector3Int> _validMoveCells = new();
        public bool IsValidBattleArea(Vector3 worldPosition)
        {
            Vector3Int baseCoords= _tilemap.WorldToCell(worldPosition);
            TileBase tile = _tilemap.GetTile(baseCoords);
            return tile is TileBattleArea;
        }

        public void ShowMoveTile(UnitModel unitModel)
        {
            _validMoveCells.Clear();
            Debug.Log(unitModel.UnitData.Name);
            Vector3Int baseCoords = unitModel.Coordinates;
            int moveRange = unitModel.UnitData.MoveRange;

            foreach (Transform child in _highlightContainer.transform)
                Destroy(child.gameObject);

            for (int dx = -moveRange; dx <= moveRange; dx++)
            {
                for (int dy = -moveRange; dy <= moveRange; dy++)
                {
                    // hanya tile dengan |dx|+|dy| <= moveRange
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) > moveRange)
                        continue;

                    Vector3Int hitCoords = new Vector3Int(baseCoords.x + dx, baseCoords.y + dy, baseCoords.z);
                    TileBase tile = _tilemap.GetTile(hitCoords);
                    if (tile is TileBattleArea)
                    {
                        _validMoveCells.Add(hitCoords);
                        // dapatkan pusat tile dengan GetCellCenterWorld
                        Vector3 worldPos = _tilemap.GetCellCenterWorld(hitCoords);
                        Instantiate(_highlight, worldPos, Quaternion.identity, _highlightContainer.transform);
                    }
                }
            }

        }
        public bool IsValidMoveCell(Vector3 worldPosition)
        {
            Vector3Int cell = _tilemap.WorldToCell(worldPosition);
            return _validMoveCells.Contains(cell);
        }
        public void HideMoveTile()
        {
            _validMoveCells.Clear();
            foreach (Transform child in _highlightContainer.transform)
                Destroy(child.gameObject);
        }
    }
}