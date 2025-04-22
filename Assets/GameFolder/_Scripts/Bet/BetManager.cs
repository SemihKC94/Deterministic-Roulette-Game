using SKC.DeterministicRoulette.Events;
using SKC.DeterministicRoulette.Save;
using SKC.DeterministicRoulette.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.DeterministicRoulette.Helpers;
using SKC.DeterministicRoulette.UI;
using SKC.DeterministicRoulette.Core;

namespace SKC.DeterministicRoulette.Bet
{
    public class BetManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _mainCamera = null;
        [SerializeField] private UIManager _uiManager = null;
        [SerializeField] private ParticleSystem _winConffeties = null;

        [Space, Header("Bets")]
        [SerializeField] private BetNumber[] _straightBets;
        [SerializeField] private BetNumber[] _splitBets;
        [SerializeField] private BetNumber[] _streetBets;
        [SerializeField] private BetNumber[] _sixLineBets;
        [SerializeField] private BetNumber[] _cornerBets;
        [SerializeField] private BetNumber[] _redBets;
        [SerializeField] private BetNumber[] _blackBets;
        [SerializeField] private BetNumber[] _evenBets;
        [SerializeField] private BetNumber[] _oddBets;
        [SerializeField] private BetNumber[] _lowBets;
        [SerializeField] private BetNumber[] _highBets;
        [SerializeField] private BetNumber[] _dozensBets;
        [SerializeField] private BetNumber[] _columnsBets;
        [SerializeField] private BetNumber[] _allBets;

        // privates
        private PoolObject _tempChip = null;
        private ChipPool _chipPool = null;
        private bool _enableBet = true;
        private bool _chipReady = false;
        private int _unconfirmedChips = 0;
        private int _currentBetChips = 0;
        private int _totalBetChips = 0;
        private int _gainedChips = 0;
        private Vector3 _mousePosition;
        private Vector3 _worldPosition;

        // Access
        public bool EnableBet 
        { 
            get 
            {
                if (SaveManager.SaveData.SoftCurrency >= _currentBetChips && SaveManager.SaveData.SoftCurrency > 0) return _enableBet;
                else return false;
            } 
        }

        public int CurrentBetChip { get { return _currentBetChips; } }
        public int UnconfirmedChip { get { return _unconfirmedChips; } }
        public PoolObject TempChip { get { return _tempChip; } }
        public ChipPool ChipPool { get { return _chipPool; } }

        private void Start()
        {
            _totalBetChips = SaveManager.SaveData.SoftCurrency;
        }

        private void OnEnable()
        {
            BasicEvents.WinnerNumber += SetResult;
        }

        private void OnDisable()
        {
            BasicEvents.WinnerNumber -= SetResult;
        }

        private void FixedUpdate()
        {
            if (!_chipReady) return;

            _mousePosition = Input.mousePosition;

            Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _worldPosition = hitInfo.point;
            }

            _tempChip.transform.position = _worldPosition;
        }

        private void SetResult(int temp, NumberColor tempWheelColor)
        {
            StartCoroutine(CalculateGain());
        }

        private IEnumerator CalculateGain()
        {
            yield return Helper.GetWait(1.0f);
            if (_gainedChips > 0)
            {
                SaveManager.SaveData.SoftCurrency += _gainedChips;
                _uiManager.Win(_gainedChips);
                _winConffeties.Play();
                SaveManager.SaveData.TotalGainChip += _gainedChips;
                SaveManager.SaveData.WinCount += 1;
            }
            else
            {
                SaveManager.SaveData.TotalLossChip += _unconfirmedChips;
                SaveManager.SaveData.LossCount += 1;
                _uiManager.Loss();
            }

            BasicEvents.InvokeSaveGame();
        }

        public void ChipVisual(PoolObject _tempChip, ChipPool _chipPool)
        {
            this._tempChip = _tempChip;
            this._chipPool = _chipPool;
            
            if(this._tempChip == null)
            {
                _chipReady = false;
                return;
            }

            _chipReady = true;
            _currentBetChips = (int)_tempChip.ChipType;
        }

        public void ClearVisualChip()
        {
            _chipReady = false;
            if (_tempChip != null)
                _tempChip.gameObject.SetActive(false);
            _tempChip = null;
        }

        public void AddChips(int _tempChipValue)
        {
            _unconfirmedChips += _tempChipValue;
            _currentBetChips = _tempChipValue;
            _totalBetChips -= _currentBetChips;
            SaveManager.SaveData.SoftCurrency -= _currentBetChips;
            BasicEvents.InvokeRefreshMoney();

            _uiManager.BettedChips(_unconfirmedChips);
        }

        public void ConfirmBet()
        {
            _chipReady = false;
            _enableBet = false;
            _tempChip = null;
            SaveManager.SaveData.SoftCurrency -= _totalBetChips;
            _totalBetChips = SaveManager.SaveData.SoftCurrency;
            // Unconfirmed chip
            BasicEvents.InvokeSaveGame();
        }

        public void CheckCondition(int gain)
        {
            _gainedChips += gain;
        }

        public void ResetBets()
        {
            foreach (BetNumber item in _allBets)
            {
                item.Reset();
            }
        }

        public void ClearBet()
        {
            if(_tempChip != null)
                _tempChip.gameObject.SetActive(false);

            _tempChip = null;
            SaveManager.SaveData.SoftCurrency += _unconfirmedChips;
            _unconfirmedChips = 0;
            _totalBetChips = 0;
            _currentBetChips = 0;
            _gainedChips = 0;
            _chipReady = false;
            _enableBet = true;
            _tempChip = null;
            BasicEvents.InvokeSaveGame();
        }
    }

}
