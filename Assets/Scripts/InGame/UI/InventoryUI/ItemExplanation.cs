using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.View
{
    public class ItemExplanation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private Image frame;
        [SerializeField] private TextMeshProUGUI stats;
        [SerializeField] private TextMeshProUGUI invokeEffect;
        [Space(10)]
        [SerializeField] private CommandButton invokeButton;
        [SerializeField] private CommandButton equipWeaponButton;
        [SerializeField] private CommandButton equipArmorButton;
        [SerializeField] private CommandButton cancelButton;

        private readonly Subject<InventoryCommandType> onCommand = new Subject<InventoryCommandType>();

        private void Awake()
        {
            _ = invokeButton.OnClick
                .Subscribe(_ => onCommand.OnNext(InventoryCommandType.Invoke))
                .AddTo(this);
            _ = equipWeaponButton.OnClick
                .Subscribe(_ => onCommand.OnNext(InventoryCommandType.EquipAsWeapon))
                .AddTo(this);
            _ = equipArmorButton.OnClick
                .Subscribe(_ => onCommand.OnNext(InventoryCommandType.EquipAsArmor))
                .AddTo(this);
            _ = cancelButton.OnClick
                .Subscribe(_ => onCommand.OnNext(InventoryCommandType.Cancel))
                .AddTo(this);
        }

        /// <summary>
        /// 説明画面を開く。操作が終わるまで待機する。
        /// </summary>
        public async UniTask<InventoryCommandType> Open(InventorySlotView slot, CancellationToken token)
        {
            try
            {
                SetItem(slot);
                Open();
                var command = await onCommand.ToUniTask(true, token);
                return command;
            }
            finally
            {
                Close();
            }
        }

        private void Open()
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            group.alpha = 1.0f;
        }

        private void Close()
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            group.alpha = 0.0f;
        }

        private void SetItem(InventorySlotView slot)
        {
            var view = slot.ItemView;
            cardName.text = view.Name;
            frame.sprite = view.Frame;
            stats.text = view.StatsInfo;
            invokeEffect.text = view.InvokeEffectInfo;

            bool canInvoke = slot.Type == SlotType.Inventory && view.CanInvoke;
            invokeButton.SetEnable(canInvoke);
        }
    }
}