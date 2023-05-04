using LoF.GameLogic;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.GameLogic.Entity.Actor.Model;
using TMPro;
using UniRx;
using UnityEngine;

namespace LoF.UI.Info
{
    //TODO:Viewと切り分ける
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private UICursor cursor;
        [SerializeField] private TextMeshProUGUI nameUI;
        [SerializeField] private TextMeshProUGUI hpUI;
        [SerializeField] private TextMeshProUGUI weaponUI;
        [SerializeField] private TextMeshProUGUI armorUI;

        private Level level;
        private CompositeDisposable levelDisposables;
        private Player player;

        private void OnDestroy()
        {
            levelDisposables?.Dispose();
        }

        public void Initialize(Player player)
        {
            this.player = player;
            _ = player.Model.Status.HP
                .Subscribe(_ => SetInfo(player.Model))
                .AddTo(this);

            _ = player.InventoryUI.IsOpen
                .Subscribe(isOpen => cursor.SetEnable(!isOpen))
                .AddTo(this);
        }

        public void SetLevel(Level level)
        {
            this.level = level;
            levelDisposables?.Dispose();
            levelDisposables = new CompositeDisposable();

            _ = level.Turn.OnPlayerActEnd
                .Subscribe(_ => SetInfo(player.Model))
                .AddTo(levelDisposables);

            _ = cursor.Position
                .Subscribe(position => CheckActorAt(position))
                .AddTo(levelDisposables);
        }

        /// <summary>
        ///     表示情報を指定したActorのステータスにする。
        /// </summary>
        /// <param name="actor"></param>
        public void SetInfo(IActorModel actor)
        {
            if (actor == null) return;

            const int nameToHPMargin = -2;
            const int HPtoWeaponMargin = -3;
            const int x = 0;

            nameUI.text = actor.Name;
            var sizeX = (int)nameUI.rectTransform.sizeDelta.x;
            nameUI.rectTransform.sizeDelta = new Vector2Int(sizeX, (int)nameUI.preferredHeight);

            hpUI.text = $"♥ {actor.Status.HP.Value}／{actor.Status.MaxHP.Value}";
            var y = nameToHPMargin - (int)nameUI.rectTransform.sizeDelta.y;
            hpUI.rectTransform.anchoredPosition = new Vector2Int(x, y);

            y = HPtoWeaponMargin + (int)hpUI.rectTransform.anchoredPosition.y - (int)hpUI.rectTransform.sizeDelta.y;
            weaponUI.rectTransform.anchoredPosition = new Vector2Int(x, y);
            weaponUI.text = actor.Status.AttackStatuses.GetInfo();
        }

        private void CheckActorAt(Vector2Int position)
        {
            var actor = level.Actors.GetActorAt(position);
            SetInfo(actor);
        }
    }
}