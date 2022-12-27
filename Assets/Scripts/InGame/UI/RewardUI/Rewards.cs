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
        [SerializeField] private List<Card> candiateCards;

        private List<Card> currentCandiates = new List<Card>();

        [Inject]
        public void Construct(RewardsUI ui)
        {
            this.ui = ui;
        }

        public async UniTask WaitUntilePickReward(Player player, CancellationToken token)
        {
            currentCandiates.Clear();
            for(int i = 0; i<3; i++)
            {
                currentCandiates.Add(PickRandomCard());
            }
            SetupUI();
            int pickID = -1;
            var disposable = ui.OnPick.Subscribe(id => pickID = id).AddTo(this);

            await ui.EnableUI(token);
            await UniTask.WaitUntil(() => pickID > -1);
            Debug.Log($"決めたアイテムをインベントリに追加する。");
            await ui.DisableUI(token);
            disposable.Dispose();
        }

        private void SetupUI()
        {
            var cardInfos = new List<CardInfo>();
            for (int i = 0; i < 3; i++)
            {
                var name = currentCandiates[i].CardName;
                var stats = currentCandiates[i].AttackStatuses.GetInfo();
                stats += Environment.NewLine;
                stats += currentCandiates[i].DefenceStatus.GetInfo();
                var invokeEffect = currentCandiates[i].InvokeEffect;
                var frame = currentCandiates[i].Frame;
                var cardInfo = new CardInfo(name, stats, invokeEffect, frame);
                cardInfos.Add(cardInfo);
            }
            ui.SetRewardsInfo(cardInfos);
        }

        private Card PickRandomCard()
        {
            var id = UnityEngine.Random.Range(0, candiateCards.Count);
            return candiateCards[id];
        }
    }
}