using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace yumehiko.LOF.View
{
	public class ItemView : MonoBehaviour
	{
		[SerializeField] private CanvasGroup group;
		[SerializeField] private TextMeshProUGUI cardName;
		[SerializeField] private Image frame;

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

		public void SetView(string cardName, Sprite frameSprite)
        {
			this.cardName.text = cardName;
			this.frame.sprite = frameSprite;
        }
	}
}