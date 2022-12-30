using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System;

namespace yumehiko.LOF.View
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

        public void Initialize(SlotType slotType)
        {
            Type = slotType;

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
            this.cardName.text = itemView.Name;
            this.frame.sprite = itemView.Frame;
            this.ItemView = itemView;
            EnableView();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void AlignPositionByID(int id)
        {
            ID = id;
            const float widthMargin = 60.0f;
            rectTransform.anchoredPosition = new Vector2(widthMargin * id, rectTransform.anchoredPosition.y);
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
        Armor,
    }
}