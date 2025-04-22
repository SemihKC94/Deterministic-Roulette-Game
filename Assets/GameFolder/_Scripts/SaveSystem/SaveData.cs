using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Save
{
    [System.Serializable]
    public class SaveData
    {
        public string UserName { get; set; }
        public string GameVersion { get; set; }
        public bool FirstTime { get; set; }
        public int SoftCurrency { get; set; }
        public int SpinCount { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public int TotalGainChip { get; set; }
        public int TotalLossChip { get; set; }
        public List<int> LatestWinnerNumber { get; set; }
    }
}
