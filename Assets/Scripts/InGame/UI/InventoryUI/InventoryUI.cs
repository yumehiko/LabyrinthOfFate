using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace yumehiko.LOF.View
{
	public class InventoryUI : MonoBehaviour
	{
		[SerializeField] private InventorySlot weaponSlot;
		[SerializeField] private InventorySlot armorSlot;
		[SerializeField] private List<InventorySlot> items;

		public void SetWeapon(IItemView view)
        {
			weaponSlot.SetView(view);
        }

		public void SetArmor(IItemView view)
        {
			armorSlot.SetView(view);
        }

		public void SetItem(IItemView view, int id)
        {
			items[id].SetView(view);
        }
	}
}