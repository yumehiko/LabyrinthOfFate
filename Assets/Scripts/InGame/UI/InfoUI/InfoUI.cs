using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;
using System;
using yumehiko.LOF.View;
using yumehiko.LOF.Model;
using TMPro;

namespace yumehiko.LOF.Presenter
{
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private UICursor cursor;
        [SerializeField] private TextMeshProUGUI nameUI;
        [SerializeField] private TextMeshProUGUI hpUI;
        [SerializeField] private TextMeshProUGUI weaponUI;
        [SerializeField] private TextMeshProUGUI armorUI;

        private Level level;
        private Player player;
        private CompositeDisposable levelDisposables;

        public void Initialize(Player player)
        {
            this.player = player;
            _ = player.Model.Status.HP
                .Subscribe(_ => SetInfo(player.Model))
                .AddTo(this);

            _ = player.Inventory.IsOpen
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

        private void OnDestroy()
        {
            levelDisposables?.Dispose();
        }

        /// <summary>
        /// 表示情報を指定したActorのステータスにする。
        /// </summary>
        /// <param name="actor"></param>
        public void SetInfo(IActor actor)
        {
            if (actor == null)
            {
                return;
            }

            const int nameToHPMargin = -2;
            const int HPtoWeaponMargin = -3;
            const int x = 0;

            nameUI.text = actor.Name;
            int sizeX = (int)nameUI.rectTransform.sizeDelta.x;
            nameUI.rectTransform.sizeDelta = new Vector2Int(sizeX, (int)nameUI.preferredHeight);

            hpUI.text = $"♥ {actor.Status.HP.Value}／{actor.Status.MaxHP.Value}";
            int y = nameToHPMargin - (int)nameUI.rectTransform.sizeDelta.y;
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