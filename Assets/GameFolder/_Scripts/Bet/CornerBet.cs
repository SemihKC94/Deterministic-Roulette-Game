using SKC.DeterministicRoulette.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Bet
{
    public class CornerBet : BetNumber
    {
        [Header("Settings")]
        [SerializeField] private BetNumber[] _cornerBetNumbers;
        public override void Start()
        {
            base.Start();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void CheckWinner(int _winnerNumber, NumberColor _numberColor)
        {
            base.CheckWinner(_winnerNumber,_numberColor);

            if(base.BetStyle == BetStyle.Corner)
            {
                bool cornerWin = false;

                foreach (var item in _cornerBetNumbers)
                {
                    if (item.MyNumber == _winnerNumber)
                        cornerWin = true;
                }

                if (cornerWin)
                {
                    int _gainedMoney = base.TotalBetChips + (base.WinPower * base.TotalBetChips);
                    base.BetManager.CheckCondition(_gainedMoney);
                }
            }
        }

        public override void OnMouseDown()
        {
            base.OnMouseDown();
        }

        public override void OnMouseOver()
        {
            foreach (var item in _cornerBetNumbers)
            {
                item.HighLight();
            }
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            foreach (var item in _cornerBetNumbers)
            {
                item.HighLight();
            }
        }

        public override void HighLight()
        {
            base.HighLight();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void References()
        {
            base.References();
        }
    }
}
