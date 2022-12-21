using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;
using yumehiko.LOF.View;
using yumehiko.LOF.Model;
using TMPro;

namespace yumehiko.LOF.Presenter
{
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameUI;
        [SerializeField] private TextMeshProUGUI hpUI;
        [SerializeField] private TextMeshProUGUI weaponUI;
        [SerializeField] private TextMeshProUGUI armorUI;


        private Level level;

        [Inject]
        public void Construct(Level level, Turn turn, UICursor cursor)
        {
            this.level = level;
            _ = turn.OnPlayerActEnd
                .Subscribe(_ => SetInfo(level.Actors.Player))
                .AddTo(this);

            _ = cursor.Position
                .Subscribe(position => CheckActorAt(position))
                .AddTo(this);

            level.Actors.Player.Status.HP
                .Subscribe(_ => SetInfo(level.Actors.Player))
                .AddTo(this);
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

            const int nameToHPMargin = -3;
            const int HPtoWeaponMargin = -4;
            const int x = 0;

            nameUI.text = actor.Name;
            int sizeX = (int)nameUI.rectTransform.sizeDelta.x;
            nameUI.rectTransform.sizeDelta = new Vector2Int(sizeX, (int)nameUI.preferredHeight);

            hpUI.text = $"♥ {actor.Status.HP.Value}／{actor.Status.MaxHP.Value}";
            int y = nameToHPMargin - (int)nameUI.rectTransform.sizeDelta.y;
            hpUI.rectTransform.anchoredPosition = new Vector2Int(x, y);

            y = HPtoWeaponMargin + (int)hpUI.rectTransform.anchoredPosition.y - (int)hpUI.rectTransform.sizeDelta.y;
            weaponUI.rectTransform.anchoredPosition = new Vector2Int(x, y);
            weaponUI.text = GetAttackInfo(actor.Status.Weapons);
        }

        private string GetAttackInfo(IReadOnlyList<AttackStatus> attacks)
        {
            const string diceChars = "\uE000\uE001\uE002\uE003\uE004\uE005";
            var info = "";
            for (int i = 0; i < 6; i++)
            {
                info += i != 0 ? System.Environment.NewLine : string.Empty; //1行目以外はまず改行を追加
                if (attacks[i].IsMiss())
                {
                    info += $"{diceChars[i]} MISS";
                    continue;
                }

                //属性攻撃とかあったらたす。
                info += $"{diceChars[i]} \uE006{attacks[i].AD}";
            }
            return info;
        }

        private void CheckActorAt(Vector2Int position)
        {
            var actor = level.Actors.GetActorAt(position);
            SetInfo(actor);
        }
    }
}