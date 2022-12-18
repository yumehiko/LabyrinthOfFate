using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;
using yumehiko.LOF.View;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.Presenter
{
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private Text targetName;
        [SerializeField] private Text targetHP;

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

            targetName.text = actor.Name;
            targetHP.text = $"HP:{actor.Status.HP.Value}/{actor.Status.MaxHP.Value}";
        }

        private void CheckActorAt(Vector2Int position)
        {
            var actor = level.Actors.GetActorAt(position);
            SetInfo(actor);
        }
    }
}