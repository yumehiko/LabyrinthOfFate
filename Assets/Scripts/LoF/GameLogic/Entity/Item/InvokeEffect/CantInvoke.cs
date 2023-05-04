using LoF.GameLogic.Entity.Actor.Model;

namespace LoF.GameLogic.Entity.Item.InvokeEffect
{
    public class CantInvoke : IInvokeEffect
    {
        public InvokeType Type => InvokeType.CantInvoke;

        public string Info =>
            "Invoke: You Can't Invoke this Card.";

        public void Invoke(IActorModel user, IItemModel parent)
        {
        }
    }
}