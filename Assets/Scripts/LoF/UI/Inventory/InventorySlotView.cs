using System;
using LoF.GameLogic.Entity.Item;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace LoF.UI.Inventory
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private Image frame;
        [SerializeField] private Button button;

        public SlotType Type { get; private set; }
        public int ID { get; private set; }
        public IItemView ItemView { get; private set; }
        public IObservable<Unit> OnClick => button.OnClickAsObservable();

        public void Initialize(SlotType slotType, int id)
        {
            Type = slotType;
            ID = id;

            _ = button.OnPointerEnterAsObservable()
                .Subscribe(_ => Select())
                .AddTo(this);

            _ = button.OnPointerExitAsObservable()
                .Subscribe(_ => Deselect())
                .AddTo(this);

            _ = button.OnClickAsObservable()
                .Subscribe(_ => Click())
                .AddTo(this);
        }

        public void SetView(IItemView itemView)
        {
            cardName.text = itemView.Name;
            frame.sprite = itemView.Frame;
            ItemView = itemView;
            EnableView();
        }

        public void DisableView()
        {
            group.alpha = 0.0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        private void Select()
        {
            group.alpha = 1.0f;
        }

        private void Deselect()
        {
            group.alpha = 0.75f;
        }

        private void Click()
        {
            //ここでアニメーションする。
        }

        private void EnableView()
        {
            group.alpha = 0.75f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }

    public enum SlotType
    {
        Inventory,
        Weapon,
        Armor
    }
}