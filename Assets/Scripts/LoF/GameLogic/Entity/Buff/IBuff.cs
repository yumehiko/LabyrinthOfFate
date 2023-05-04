namespace LoF.GameLogic.Entity.Buff
{
	/// <summary>
	///     バフ。一時的な強化または弱化効果。
	/// </summary>
	public interface IBuff
    {
        BuffType Type { get; }
        BuffLifetimeType LifetimeType { get; }
        int Lifetime { get; }
        void CountLifetime();
    }

    public enum BuffType
    {
        None,
        Acceleration
    }

    /// <summary>
    ///     バフの寿命のタイプ。
    /// </summary>
    public enum BuffLifetimeType
    {
        Level,
        Turn
    }
}