using SKC.DeterministicRoulette.Core;
using SKC.DeterministicRoulette.Helpers;
using UnityEngine;
using TMPro;
using System.Collections;
using SKC.DeterministicRoulette.Pool;
using SKC.DeterministicRoulette.Events;
using System.Collections.Generic;
using SKC.DeterministicRoulette.Sound;

namespace SKC.DeterministicRoulette.Bet
{
    public class BetNumber : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteBG;
        [SerializeField] private ParticleSystem _highlighEffect;
        [SerializeField] private TextMeshPro _numberText;
        [SerializeField] private Transform _chipContainer;

        [Space, Header("Settings")]
        [SerializeField] private BetStyle _betStyle;
        [SerializeField] private NumberBase _betNumberBase = null;
        [SerializeField] private int _winPower;

        // Access
        public NumberBase BetNumberBase { get { return _betNumberBase; } set { _betNumberBase = value; } }
        public BetManager BetManager { get { return _betManager; } }
        public ParticleSystem HighlighEffect {  get { return _highlighEffect; } }
        public BetStyle BetStyle { get { return _betStyle; } }
        public int TotalBetChips { get { return _totalBetChips; } }
        public int WinPower { get { return _winPower; } }
        public int MyNumber { get { return int.Parse(_betNumberBase.Number); } }

        // Privates
        private BetManager _betManager;
        private const string HEX_BLACK = "50504E";
        private HexCode _blackHexCode;
        private float _highlightTime;
        private bool _highlight = false;

        // Chips
        private int _addedChips = 0;
        private int _totalBetChips = 0;
        private int _myNumber;

        public virtual void Start()
        {
            References();
            SetValues();
        }

        public virtual void OnEnable()
        {
            BasicEvents.WinnerNumber += CheckWinner;
        }

        public virtual void OnDisable()
        {
            BasicEvents.WinnerNumber -= CheckWinner;
        }

        public virtual void References()
        {
            _betManager = GetComponentInParent<BetManager>();
        }

        [ContextMenu("Set")]
        public void SetValues()
        {
            _blackHexCode = Helper.ConvertHexToRBG(HEX_BLACK);

            switch (_betNumberBase.Color)
            {
                case NumberColor.Red:
                    _spriteBG.color = Color.red;

                    break;
                case NumberColor.Black:
                    _spriteBG.color = new Color(_blackHexCode.hexR / 255f, _blackHexCode.hexG / 255f, _blackHexCode.hexB / 255f);

                    break;
                case NumberColor.Green:
                    _spriteBG.color = Color.green;

                    break;
            }

            if(int.Parse(_betNumberBase.Number) >= 0)
                _numberText.SetText(_betNumberBase.Number.ToString());
        }

        public virtual void OnMouseEnter()
        {
            HighLight();
        }

        public virtual void OnMouseOver()
        {
            HighLight();
        }

        public virtual void OnMouseDown()
        {
            if(_betManager.EnableBet && _betManager.CurrentBetChip != 0 && _betManager.TempChip != null)
            {
                _betManager.AddChips((int)_betManager.TempChip.ChipType);
                _addedChips++;
                _totalBetChips += (int)_betManager.TempChip.ChipType;
                var obj = _betManager.ChipPool.TakeFromPool();
                obj.transform.SetParent(_chipContainer);
                obj.transform.localPosition = Vector3.zero + (Vector3.up * 0.01f * _addedChips);

                SoundManager.Instance.PlayChipSound();
            }
        }

        public virtual void CheckWinner(int _winnerNumber, NumberColor _numberColor)
        {
            int.TryParse(_betNumberBase.Number, out _myNumber);
            if(_betStyle == BetStyle.Straight)
            {
                if (_myNumber == _winnerNumber)
                {
                    // Win
                    int _gainedMoney = _totalBetChips + (_winPower * _totalBetChips);
                    _betManager.CheckCondition(_gainedMoney);
                }
            }
        }

        public virtual void Reset()
        {
            _highlightTime = 0;
            _highlight = false;
            _highlighEffect.Stop();
            _highlighEffect.Clear();
            _totalBetChips = 0;

            List<Transform> temp = new List<Transform>();
            foreach (Transform item in _chipContainer)
            {

                temp.Add(item);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].gameObject.SetActive(false);
                temp[i].transform.SetParent(null);
            }
            _addedChips = 0;

            StopAllCoroutines();
        }

        public virtual void HighLight()
        {
            _highlightTime = 0.5f;

            if (!_highlight) StartCoroutine(HighlightApply());
            _highlight = true;
        }

        private IEnumerator HighlightApply()
        {
            _highlighEffect.Play();

            while(_highlightTime > 0)
            {
                _highlightTime -= 0.1f;
                yield return Helper.GetWait(0.1f);
            }

            _highlighEffect.Stop();
            _highlighEffect.Clear();
            _highlight = false;
        }

        [ContextMenu("Give Name")] // Delete Later
        public void GiveName()
        {
            if (_betNumberBase.Number.Length <= 0) return;
            this.gameObject.name = _betNumberBase.Number.ToString() + " " + _betNumberBase.Color.ToString();
        }
    }
}
