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
        public bool IsValidBattleArea(Vector3 worldPosition)
        {
            Vector3Int baseCoords= _tilemap.WorldToCell(worldPosition);
            TileBase tile = _tilemap.GetTile(baseCoords);
            return tile is TileBattleArea;
        }

        public void ShowMoveTile(UnitModel unitModel)
        {
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
                        // dapatkan pusat tile dengan GetCellCenterWorld
                        Vector3 worldPos = _tilemap.GetCellCenterWorld(hitCoords);
                        Instantiate(_highlight, worldPos, Quaternion.identity, _highlightContainer.transform);
                    }
                }
            }

        }

        public void HideMoveTile()
        {
            foreach (Transform child in _highlightContainer.transform)
                Destroy(child.gameObject);
        }
    }
}