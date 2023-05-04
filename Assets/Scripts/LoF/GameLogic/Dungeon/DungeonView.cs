using System;
using DG.Tweening;
using LoF.GameLogic.Dungeon.Material;
using UnityEngine;

namespace LoF.GameLogic.Dungeon
{
    /// <summary>
    ///     ゲーム中のダンジョンのView。
    /// </summary>
    public class DungeonView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hider;
        [SerializeField] private Color hiderColor;
        [SerializeField] private TileViewSetter wallTileSetter;
        [SerializeField] private TileViewSetter borderWallTileSetter;
        [SerializeField] private Camera mainCamera;

        public void SetTiles(Terrain terrain)
        {
            foreach (var tile in terrain) SetTile(tile);
            //TODO:ちょっと左にずらしてるけどこのやり方はどっかで直す
            var transform1 = mainCamera.transform;
            var cameraPosition = new Vector3(-terrain.Bounds.position.x - 6.0f, -terrain.Bounds.position.y,
                transform1.position.z);
            transform1.position = cameraPosition;
        }

        public Sequence Show()
        {
            var sequence = DOTween.Sequence()
                .AppendCallback(() => hider.color = hiderColor)
                .AppendCallback(() => hider.enabled = true)
                .Append(hider.DOFade(0.0f, 1.0f))
                .AppendCallback(() => hider.enabled = false);
            return sequence.Play();
        }

        public Tweener Hide()
        {
            hider.enabled = true;
            return hider.DOFade(1.0f, 1.0f).SetLink(gameObject);
        }

        private void SetTile(DungeonTile tile)
        {
            switch (tile.Type)
            {
                case TileType.Empty: return;
                case TileType.Wall:
                    wallTileSetter.SetTile(tile);
                    return;
                case TileType.BorderWall:
                    borderWallTileSetter.SetTile(tile);
                    return;
                default: throw new Exception("未定義の地形タイプ");
            }
        }
    }
}