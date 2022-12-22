using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using VContainer;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF.View
{
    /// <summary>
    /// ゲーム中のダンジョンのView。
    /// </summary>
	public class DungeonView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hider;
        [SerializeField] private Color hiderColor;
        [SerializeField] private TileViewSetter wallTileSetter;
        [SerializeField] private TileViewSetter borderWallTileSetter;
        [SerializeField] private Camera mainCamera;

        public void SetTiles(Dungeon dungeon)
        {
            foreach (var tile in dungeon)
            {
                SetTile(tile);
            }

            var cameraPosition = new Vector3(-dungeon.Bounds.position.x, -dungeon.Bounds.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = cameraPosition;
        }

        public Sequence Show()
        {
            Sequence sequence = DOTween.Sequence()
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
                default: throw new System.Exception("未定義の地形タイプ");
            }
        }
    }
}