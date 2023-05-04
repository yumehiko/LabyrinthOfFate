using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LoF.GameLogic.Entity.Item;
using UniRx;
using UnityEngine;

namespace LoF.UI.Inventory
{
    public class InventoryUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup uiGroup;
        [SerializeField] private RectTransform uiRect;
        [SerializeField] private InventorySlotView weaponSlot;
        [SerializeField] private InventorySlotView armorSlot;
        [SerializeField] private List<InventorySlotView> slots;
        [SerializeField] private ItemExplanation explanation;

        [Space(10)] [SerializeField] private Transform slotViewsParent;

        [SerializeField] private InventorySlotView slotViewPrefab;

        private readonly Subject<InventorySlotView> onClickSlot = new();
        private float initY;
        private Sequence openSequence;

        public void Initialize()
        {
            initY = uiRect.anchoredPosition.y;
            weaponSlot.Initialize(SlotType.Weapon, 0);
            armorSlot.Initialize(SlotType.Armor, 0);
            _ = weaponSlot.OnClick.Subscribe(_ => onClickSlot.OnNext(weaponSlot)).AddTo(this);
            _ = armorSlot.OnClick.Subscribe(_ => onClickSlot.OnNext(armorSlot)).AddTo(this);
            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                slot.Initialize(SlotType.Inventory, i);
                _ = slot.OnClick.Subscribe(_ => onClickSlot.OnNext(slot)).AddTo(this);
            }
        }

        public void RefleshView(IItemView weapon, IItemView armor, IReadOnlyList<IItemView> items)
        {
            weaponSlot.SetView(weapon);
            armorSlot.SetView(armor);
            for (var i = 0; i < slots.Count; i++)
            {
                if (items.Count <= i)
                {
                    slots[i].DisableView();
                    continue;
                }

                slots[i].SetView(items[i]);
            }
        }

        public async UniTask<InventoryCommand> Open(CancellationToken token)
        {
            try
            {
                await OpenAnimation().ToUniTask(cancellationToken: token);
                InventorySlotView slot = null;
                var type = InventoryCommandType.Cancel;
                while (!token.IsCancellationRequested)
                {
                    slot = await onClickSlot.ToUniTask(true, token);
                    type = await explanation.Open(slot, token);
                    if (type != InventoryCommandType.Cancel) break;
                }

                var command = new InventoryCommand(slot, type);
                return command;
            }
            catch (OperationCanceledException e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
        }

        private void Close()
        {
            uiGroup.interactable = false;
            uiGroup.blocksRaycasts = false;
            openSequence?.Kill();
            openSequence = DOTween.Sequence()
                .Append(uiGroup.DOFade(0.5f, 0.5f))
                .Join(uiRect.DOAnchorPosY(initY, 0.5f))
                .SetLink(gameObject);
            openSequence.Play();
        }

        private Sequence OpenAnimation()
        {
            openSequence?.Kill();
            openSequence = DOTween.Sequence()
                .Append(uiGroup.DOFade(1.0f, 0.5f))
                .Join(uiRect.DOMoveY(0.0f, 0.5f))
                .AppendCallback(() => uiGroup.interactable = true)
                .AppendCallback(() => uiGroup.blocksRaycasts = true)
                .SetLink(gameObject);
            return openSequence.Play();
        }
    }

    public enum InventoryCommandType
    {
        None,
        Invoke,
        EquipAsWeapon,
        EquipAsArmor,
        Cancel
    }

    public class InventoryCommand
    {
        public InventoryCommand(InventorySlotView slot, InventoryCommandType type)
        {
            Slot = slot;
            Type = type;
        }

        public InventorySlotView Slot { get; }
        public InventoryCommandType Type { get; }
    }
}