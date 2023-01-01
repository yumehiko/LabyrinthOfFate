using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// このレベルにいる間、加速度+1。
    /// </summary>
    public class AccelerateBuff : IBuff
    {
        public BuffType Type => BuffType.Acceleration;
        public BuffLifetimeType LifetimeType { get; } = BuffLifetimeType.Level;
        public int Lifetime { get; private set; } = 1;
        public int AddEnergy { get; } = 1;

        public void CountLifetime()
        {
            Lifetime--;
        }
    }
}