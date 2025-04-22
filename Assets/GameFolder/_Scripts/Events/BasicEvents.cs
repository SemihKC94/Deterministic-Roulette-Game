using System;
using SKC.DeterministicRoulette.Core;
namespace SKC.DeterministicRoulette.Events
{
    public static class BasicEvents
    {
        public static Action SaveGame;
        public static void InvokeSaveGame()
        {
            SaveGame?.Invoke();
            RefreshMoney?.Invoke();
        }

        public static Action LevelSucces;
        public static void InvokeLevelSucces()
        {
            LevelSucces?.Invoke();
        }

        public static Action LevelFail;
        public static void InvokeLevelFail()
        {
            LevelFail?.Invoke();
        }

        public static Action LoadLevel;
        public static void InvokeLoadLevel()
        {
            LoadLevel?.Invoke();
        }

        public static Action RefreshMoney;
        public static void InvokeRefreshMoney()
        {
            RefreshMoney?.Invoke();
        }

        public static Action<int,NumberColor> WinnerNumber;
        public static void InvokeWinnerNumber(int _winnerNumber, NumberColor _numberColor)
        {
            WinnerNumber?.Invoke(_winnerNumber, _numberColor);
        }


    }
}
