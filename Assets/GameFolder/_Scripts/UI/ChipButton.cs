using UnityEngine;
using UnityEngine.UI;
using SKC.DeterministicRoulette.Core;
using SKC.DeterministicRoulette.Save;
using SKC.DeterministicRoulette.Events;
using TMPro;
using SKC.DeterministicRoulette.Bet;
using SKC.DeterministicRoulette.Pool;

namespace SKC.DeterministicRoulette.UI
{
    public class ChipButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ChipType _chipType;
        [SerializeField] private ChipPool _chipPooled;

        [Space, Header("References")]
        [SerializeField] private Button _chipButton;
        [SerializeField] private TextMeshProUGUI _chipVisualText;
        [SerializeField] private GameObject _disabledButton;
        [SerializeField] private BetManager _betManager;

        // Privates
        private bool _interactable = true;
        private int _chipValue = 0;

        private void Start()
        {
            _chipButton.onClick.AddListener(() => BetChip());

            SetChipValue();            
        }

        private void OnEnable()
        {
            BasicEvents.RefreshMoney += RefreshChip;
        }

        private void OnDisable()
        {
            BasicEvents.RefreshMoney -= RefreshChip;
        }

        public void RefreshChip()
        {
            if(_chipValue == 0)
            {
                SetChipValue();
            }

            if(SaveManager.SaveData.SoftCurrency < _chipValue)
            {
                _interactable = false;
                _chipButton.interactable = _interactable;
                _disabledButton.gameObject.SetActive(!_interactable);

                return;
            }

            _interactable = true;
            _chipButton.interactable = _interactable;
            _disabledButton.gameObject.SetActive(!_interactable);
        }

        private void BetChip()
        {
            var obj = _chipPooled.TakeFromPool();
            _betManager.ChipVisual(obj,_chipPooled);
        }

        private void SetChipValue()
        {
            switch (_chipType)
            {
                case ChipType.None:
                    _chipValue = -1;
                    _chipVisualText.SetText(string.Empty);

                    break;
                case ChipType.Chip10:
                    _chipValue = 10;

                    break;
                case ChipType.Chip50:
                    _chipValue = 50;

                    break;
                case ChipType.Chip100:
                    _chipValue = 100;

                    break;
                case ChipType.Chip250:
                    _chipValue = 250;

                    break;
                case ChipType.Chip500:
                    _chipValue = 500;

                    break;
            }

            _chipVisualText.SetText("$" + _chipValue.ToString());
            RefreshChip();
        }
    }
}
