using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapLayer
{
    [CreateAssetMenu(fileName = "TilePlayerSpawn", menuName = "Tile/Player Spawn")]
    public class TilePlayerSpawn:TileBase
    {
        [SerializeField] private Color _validAreaColor = new Color(0, 1, 0, 0.3f);
        [SerializeField] private Sprite _areaSprite;
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            tilemap.RefreshTile(position);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _areaSprite;
            tileData.color = _validAreaColor ;
            tileData.flags = TileFlags.LockColor;
            tileData.colliderType = Tile.ColliderType.None; 
        }
    }
}