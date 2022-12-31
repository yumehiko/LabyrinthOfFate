﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのコレクション。
    /// </summary>
    public class InventoryModel : IEnumerable<IItemModel>
    {
        public EquipSlot EquipSlot { get; }
        public IItemModel this[int index] => items[index];
        public int MaxCapacity = 5;
        public bool IsFull => items.Count == MaxCapacity;
        public IObservable<Unit> OnReflesh => onReflesh;

        private readonly Subject<Unit> onReflesh = new Subject<Unit>();
        private readonly List<IItemModel> items = new List<IItemModel>();

        public InventoryModel(IActorProfile profile)
        {
            var weapon = new CardModel(profile.Weapon);
            var armor = new CardModel(profile.Armor);
            EquipSlot = new EquipSlot(weapon, armor);
        }

        /// <summary>
        /// アイテムを追加する。
        /// 追加できた場合、インベントリ上の番号を返す。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(IItemModel item)
        {
            if (items.Count >= MaxCapacity)
            {
                throw new Exception("インベントリの容量を超えてしまう");
            }
            items.Add(item);
            onReflesh.OnNext(Unit.Default);
            return items.Count - 1;
        }

        public void EquipWeapon(ICardModel card)
        {
            EquipSlot.EquipWeapon(card);
            onReflesh.OnNext(Unit.Default);
        }

        public void EquipArmor(ICardModel card)
        {
            EquipSlot.EquipArmor(card);
            onReflesh.OnNext(Unit.Default);
        }

        public void Remove(IItemModel item)
        {
            items.Remove(item);
            onReflesh.OnNext(Unit.Default);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            onReflesh.OnNext(Unit.Default);
        }

        public void InvokeCard(int id, IActorModel user)
        {
            items[id].InvokeEffect.Invoke(user, items[id]);
        }

        /// <summary>
        /// インベントリから選んで武器を装備する。
        /// </summary>
        /// <param name="id"></param>
        public void EquipAsWeaponFromInventory(int id)
        {
            var originWeapon = EquipSlot.Weapon;
            var target = (ICardModel)items[id];
            items.RemoveAt(id);
            EquipSlot.EquipWeapon(target);
            items.Add(originWeapon);
            onReflesh.OnNext(Unit.Default);
        }

        /// <summary>
        /// インベントリから選んで防具を装備する。
        /// </summary>
        /// <param name="id"></param>
        public void EquipAsArmorFromInventory(int id)
        {
            var originArmor = EquipSlot.Armor;
            var target = (ICardModel)items[id];
            items.RemoveAt(id);
            EquipSlot.EquipArmor(target);
            items.Add(originArmor);
            onReflesh.OnNext(Unit.Default);
        }

        /// <summary>
        /// 武器と防具を交換する。
        /// </summary>
        public void SwitchWeaponArmor()
        {
            var originArmor = EquipSlot.Armor;
            var originWeapon = EquipSlot.Weapon;
            EquipSlot.EquipWeapon(originArmor);
            EquipSlot.EquipArmor(originWeapon);
            onReflesh.OnNext(Unit.Default);
        }

        public int IndexOf(IItemModel item) => items.IndexOf(item);
        IEnumerator<IItemModel> IEnumerable<IItemModel>.GetEnumerator() => items.GetEnumerator();
        public IEnumerator GetEnumerator() => items.GetEnumerator();
    }
}