using LoF.GameLogic.Entity.Actor.Model;

namespace LoF.GameLogic.Entity.Item.InvokeEffect
{
	/// <summary>
	///     アイテムの発動効果を表す。
	/// </summary>
	public interface IInvokeEffect
    {
        InvokeType Type { get; }
        string Info { get; }
        void Invoke(IActorModel user, IItemModel parent);
    }
}