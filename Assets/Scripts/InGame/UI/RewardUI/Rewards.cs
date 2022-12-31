using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;
using yumehiko.LOF.View;
using yumehiko.LOF.Model;
using yumehiko.Input;
using System;
using UniRx;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// レベルクリア時のリワード処理。
    /// TODO:これはmonobehaviourでなくてよいので、カード候補を別のクラスに切り分けておく。
    /// RewardGiverとかに名前を変える。
    /// </summary>
    public class Rewards : MonoBehaviour
    {
        private RewardsUI uiView;
        [SerializeField] private List<CardProfile> candiateCards;

        private readonly List<CardModel> currentCandiates = new List<CardModel>();

        [Inject]
        public void Construct(RewardsUI uiView)
        {
            this.uiView = uiView;
        }

        public async UniTask WaitUntilePickReward(Inventory inventory, CancellationToken token)
        {
            SetRewardCandiates();
            SetupUI();

            //インベントリの開閉
            var inventoryDisposable = ReactiveInput.OnInventory
                .Where(isTrue => isTrue)
                .Subscribe(_ => inventory.SwitchOpen())
                .AddTo(this);

            //Rewardを無視する処理。pickCancellationがCancel()されるとRewardの取得を中断し、finallyに入る。
            var pickCancellation = new CancellationTokenSource();
            var ignoreDisposable = uiView.OnIgnore.Subscribe(_ => pickCancellation.Cancel());

            try
            {
                await uiView.EnableUI(token);
                var pickID = await WaitUntilPick(inventory, pickCancellation.Token);
                var pick = currentCandiates[pickID];
                inventory.AddItem(pick);
            }
            finally
            {
                await uiView.DisableUI(token);
                inventoryDisposable.Dispose();
                pickCancellation.Dispose();
                ignoreDisposable.Dispose();
            }
        }

        /// <summary>
        /// 報酬が選ばれるまで待機する。
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask<int> WaitUntilPick(Inventory inventory, CancellationToken token)
        {
            int pickID = -1;
            var pickDisposable = uiView.OnPick.Subscribe(id => pickID = id);
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => pickID > -1, cancellationToken: token);
                if (inventory.IsFull)
                {
                    Debug.Log("インベントリがフル（赤点滅+テキストで示す）");
                    pickID = -1;
                    continue;
                }
                if (pickID != -1) break;
            }
            pickDisposable.Dispose();
            return pickID;
        }

        private void SetRewardCandiates()
        {
            currentCandiates.Clear();
            for (int i = 0; i < 3; i++)
            {
                var card = new CardModel(PickRandomCard());
                currentCandiates.Add(card);
            }
        }

        private void SetupUI()
        {
            var views = new List<IItemView>();
            for (int i = 0; i < 3; i++)
            {
                var view = Inventory.MakeView(currentCandiates[i]);
                views.Add(view);
            }
            uiView.SetRewardsInfo(views);
        }

        private CardProfile PickRandomCard()
        {
            var id = UnityEngine.Random.Range(0, candiateCards.Count);
            return candiateCards[id];
        }
    }
}