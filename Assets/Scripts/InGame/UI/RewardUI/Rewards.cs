using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;
using yumehiko.LOF.View;
using yumehiko.LOF.Model;
using System;
using UniRx;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// レベルクリア時のリワード処理。
    /// </summary>
    public class Rewards : MonoBehaviour
    {
        private RewardsUI ui;
        [SerializeField] private List<CardModel> candiateCards;

        private List<CardModel> currentCandiates = new List<CardModel>();

        [Inject]
        public void Construct(RewardsUI ui)
        {
            this.ui = ui;
        }

        public async UniTask WaitUntilePickReward(Inventory inventory, CancellationToken token)
        {
            currentCandiates.Clear();
            for (int i = 0; i < 3; i++)
            {
                currentCandiates.Add(PickRandomCard());
            }
            SetupUI();
            int pickID = -1;
            var disposable = ui.OnPick.Subscribe(id => pickID = id).AddTo(this);

            await ui.EnableUI(token);
            await UniTask.WaitUntil(() => pickID > -1);
            var pick = currentCandiates[pickID];
            Debug.Log($"追加できるかどうかを確認しておく。");
            inventory.AddItem(pick);
            await ui.DisableUI(token);
            disposable.Dispose();
        }

        private void SetupUI()
        {
            var views = new List<IItemView>();
            for (int i = 0; i < 3; i++)
            {
                var view = Inventory.MakeView(currentCandiates[i]);
                views.Add(view);
            }
            ui.SetRewardsInfo(views);
        }

        private CardModel PickRandomCard()
        {
            var id = UnityEngine.Random.Range(0, candiateCards.Count);
            return candiateCards[id];
        }
    }
}