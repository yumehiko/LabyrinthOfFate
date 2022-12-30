using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using System;

namespace yumehiko.LOF.View
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup uiGroup;
        [SerializeField] private RectTransform uiRect;
        [SerializeField] private InventorySlotView weaponSlot;
        [SerializeField] private InventorySlotView armorSlot;
        [SerializeField] private List<InventorySlotView> slots;
        [SerializeField] private ItemExplanation explanation;
        [Space(10)]
        [SerializeField] private Transform slotViewsParent;
        [SerializeField] private InventorySlotView slotViewPrefab;

        private readonly Subject<InventorySlotView> onClickSlot = new Subject<InventorySlotView>();
        private float initY;
        private Sequence openSequence;

        public void Initialize()
        {
            initY = uiRect.anchoredPosition.y;
            weaponSlot.Initialize(SlotType.Weapon);
            armorSlot.Initialize(SlotType.Armor);
            _ = weaponSlot.OnClick.Subscribe(_ => onClickSlot.OnNext(weaponSlot)).AddTo(this);
            _ = armorSlot.OnClick.Subscribe(_ => onClickSlot.OnNext(armorSlot)).AddTo(this);
        }

        public void SetWeapon(IItemView view)
        {
            weaponSlot.SetView(view);
        }

        public void SetArmor(IItemView view)
        {
            armorSlot.SetView(view);
        }

        public void Add(IItemView view)
        {
            var slot = Instantiate(slotViewPrefab, slotViewsParent);
            slots.Add(slot);
            slot.Initialize(SlotType.Inventory);
            _ = slot.OnClick.Subscribe(_ => onClickSlot.OnNext(slot)).AddTo(slot);
            slot.SetView(view);
            AlignViews();
        }

        public void RemoveAt(int index)
        {
            slots[index].DestroySelf();
            slots.RemoveAt(index);
            AlignViews();
        }

        /// <summary>
        /// カードViewを整列する。
        /// </summary>
        private void AlignViews()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].AlignPositionByID(i);
            }
        }

        public async UniTask<InventoryCommand> Open(CancellationToken token)
        {
            try
            {
                await OpenAnimation().ToUniTask(cancellationToken: token);
                InventorySlotView slot = null;
                InventoryCommandType type = InventoryCommandType.Cancel;
                while (!token.IsCancellationRequested)
                {
                    slot = await onClickSlot.ToUniTask(true, token);
                    type = await explanation.Open(slot.ItemView, token);
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
        Cancel,
    }

    public class InventoryCommand
    {
        public InventorySlotView Slot { get; }
        public InventoryCommandType Type { get; }

        public InventoryCommand(InventorySlotView slot, InventoryCommandType type)
        {
            Slot = slot;
            Type = type;
        }
    }
}