namespace LoF.UI.ResultMessage
{
    /// <summary>
    ///     stringだけのシンプルなリザルト。
    /// </summary>
    public class RawResult : IActResult
    {
        private readonly string message;

        public RawResult(string message)
        {
            this.message = message;
        }

        public string GetMessage()
        {
            return message;
        }
    }
}