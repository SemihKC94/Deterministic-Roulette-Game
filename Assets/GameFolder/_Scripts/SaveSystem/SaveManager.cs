using SKC.DeterministicRoulette.Constant;
using SKC.DeterministicRoulette.Events;
using SKC.DeterministicRoulette.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Save
{
	public class SaveManager : MonoBehaviour
	{
        public static SaveData SaveData;
        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            LoadGame();
        }

        private void OnEnable()
        {
            BasicEvents.SaveGame += SaveGame;
        }

        private void OnDisable()
        {
            BasicEvents.SaveGame -= SaveGame;
        }

        private void SaveGame()
        {
            if (SaveData == null) return;

            DataManager.SaveData(SKContants.Save.SAVE_DATA, SaveData);
        }

        private void LoadGame()
        {
            var savedData = DataManager.LoadData<SaveData>(SKContants.Save.SAVE_DATA);

            if (savedData == null)
            {
                // New Save
                SaveData = new SaveData()
                {
                    UserName = "Player",
                    GameVersion = Application.version,
                    FirstTime = false,
                    SoftCurrency = 1000,
                    SpinCount = 0,
                    TotalGainChip = 0,
                    TotalLossChip = 0,
                    WinCount = 0,
                    LossCount = 0,
                    LatestWinnerNumber = new List<int>()
                };

                Helper.Debug($"<color=green> New Save Created</color>");

                DataManager.SaveData(SKContants.Save.SAVE_DATA, SaveData);
            }
            else
            {
                Helper.Debug($"<color=green>Load saved data</color>");
                SaveData = savedData;
            }
        }
    }
}
