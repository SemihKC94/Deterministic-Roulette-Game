using System;

namespace SKC.DeterministicRoulette.Core
{
    [Serializable]
    public class NumberBase
    {
        public string Number;
        public NumberColor Color;
        public BetStyle BetStyle;
        public bool IsOdd;
    }
}
