using LoF.GameLogic.Entity.Item.Card;
using UniRx;

namespace LoF.GameLogic.Entity.Item
{
    /// <summary>
    ///     武器用の装備スロット。
    /// </summary>
    public class EquipSlot
    {
        private readonly IntReactiveProperty additionalHP = new();

        public EquipSlot(ICardModel weapon, ICardModel armor)
        {
            EquipWeapon(weapon);
            EquipArmor(armor);
        }

        public ICardModel Weapon { get; private set; }

        public ICardModel Armor { get; private set; }

        public IReadOnlyReactiveProperty<int> AdditionalHP => additionalHP;

        public void EquipWeapon(ICardModel card)
        {
            Weapon = card;
        }

        public void EquipArmor(ICardModel card)
        {
            Armor = card;
            additionalHP.Value = Armor.DefenceStatus.HP;
        }

        public AttackStatus PickRandomAttackStatus()
        {
            var attack = Weapon.AttackStatuses.PickRandomAttack();
            return attack;
        }
    }
}