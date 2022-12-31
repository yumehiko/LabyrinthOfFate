using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.Presenter;

namespace yumehiko.LOF.Invoke
{
    /// <summary>
    /// stringだけのシンプルなリザルト。
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