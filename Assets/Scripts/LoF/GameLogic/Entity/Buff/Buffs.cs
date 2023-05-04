using System.Collections.Generic;

namespace LoF.GameLogic.Entity.Buff
{
    /// <summary>
    ///     バフのコレクション。あるActorが持つ全てのバフ。
    /// </summary>
    public class Buffs
    {
        private readonly List<IBuff> list = new();

        //------------------------
        //-----  Properties  -----
        //------------------------

        public int AddibleEnergy
        {
            get
            {
                var addibleEnergy = 0;
                foreach (var buff in list)
                    if (buff.Type == BuffType.Acceleration)
                        addibleEnergy++;
                return addibleEnergy;
            }
        }

        public void Add(IBuff buff)
        {
            list.Add(buff);
        }

        /// <summary>
        ///     寿命を数え、0以下になればバフを消す（単位：Level）
        /// </summary>
        public void CountLevelLifetime()
        {
            var tempList = new List<IBuff>(list);
            foreach (var buff in tempList)
            {
                if (buff.LifetimeType == BuffLifetimeType.Level) buff.CountLifetime();
                if (buff.Lifetime <= 0) list.Remove(buff);
            }
        }
    }
}