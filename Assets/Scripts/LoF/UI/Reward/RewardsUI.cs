using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Item;
using LoF.GameLogic.Entity.Item.Card;
using LoF.Input;
using LoF.UI.Inventory;
using UniRx;
using UnityEngine;
using VContainer;

namespace LoF.UI.Reward
{
    /// <summary>
    ///     レベルクリア時のリワード処理。
    ///     TODO:これはMonoBehaviourでなくてよいので、カード候補を別のクラスに切り分けておく。
    ///     RewardGiverとかに名前を変える。
    /// </summary>
    public class RewardsUI : MonoBehaviour
    {
        [SerializeField] private List<CardProfile> candiateCards;

        private readonly List<CardModel> currentCandiates = new();
        private RewardsUIView uiView;

        [Inject]
        public void Construct(RewardsUIView rewardUIView)
        {
            uiView = rewardUIView;
        }

        public async UniTask WaitUntilePickReward(InventoryUI inventory, CancellationToken token)
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
                inventory.Model.Add(pick);
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
        ///     報酬が選ばれるまで待機する。
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask<int> WaitUntilPick(InventoryUI inventory, CancellationToken token)
        {
            var pickID = -1;
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
            for (var i = 0; i < 3; i++)
            {
                var card = new CardModel(PickRandomCard());
                currentCandiates.Add(card);
            }
        }

        private void SetupUI()
        {
            var views = new List<IItemView>();
            for (var i = 0; i < 3; i++)
            {
                var view = InventoryUI.MakeView(currentCandiates[i]);
                views.Add(view);
            }

            uiView.SetRewardsInfo(views);
        }

        private CardProfile PickRandomCard()
        {
            var id = Random.Range(0, candiateCards.Count);
            return candiateCards[id];
        }
    }
}