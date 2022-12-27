using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace yumehiko.LOF.View
{
	public class InventorySlot : MonoBehaviour
	{
		[SerializeField] private CanvasGroup group;
		[SerializeField] private TextMeshProUGUI cardName;
		[SerializeField] private Image frame;

		private IItemView currentInfo;

		public void EnableView()
        {
			group.alpha = 1.0f;
			group.interactable = true;
			group.blocksRaycasts = true;
        }

		public void DisableView()
        {
			group.alpha = 0.0f;
			group.interactable = false;
			group.blocksRaycasts = false;
        }

		public void SetView(IItemView info)
        {
			currentInfo = info;
			this.cardName.text = info.Name;
			this.frame.sprite = info.Frame;
        }
	}
}