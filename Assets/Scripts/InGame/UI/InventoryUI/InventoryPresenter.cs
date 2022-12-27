using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using VContainer;
using System;

namespace yumehiko.LOF.Presenter
{
	public class Inventory
	{
		private readonly InventoryModel model;
		private readonly InventoryUI view;

		[Inject]
		public Inventory(InventoryModel model, InventoryUI view)
        {
			this.model = model;
			this.view = view;
        }

		public bool CanAddItem() => model.Count < 5;

		public void SetWeapon(IItemModel item)
        {
			model.SetWeapon(item);
			view.SetWeapon(MakeView(item));
        }

		public void SetArmor(IItemModel item)
        {
			model.SetArmor(item);
			view.SetWeapon(MakeView(item));
		}

		/// <summary>
		/// アイテムをインベントリに追加する。
		/// </summary>
		/// <param name="itemModel"></param>
		public void AddItem(IItemModel itemModel)
        {
			try
			{
				var itemView = MakeView(itemModel);
				int id = model.Add(itemModel);
				view.SetItem(itemView, id);
			}
			catch (Exception e) 
            {
				throw e;
            }
        }

		public static IItemView MakeView(IItemModel model)
        {
			var name = model.Name;
			var frame = model.Frame;
			var invokeEffect = model.InvokeEffect;
			var statsInfo = model.StatsInfo;
			var view = new ItemView(name, frame, invokeEffect, statsInfo);
			return view;
		}
	}
}