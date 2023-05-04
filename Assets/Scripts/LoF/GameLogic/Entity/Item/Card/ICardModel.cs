namespace LoF.GameLogic.Entity.Item.Card
{
    public interface ICardModel : IItemModel
    {
        AttackStatuses AttackStatuses { get; }
        DefenceStatus DefenceStatus { get; }
    }
}