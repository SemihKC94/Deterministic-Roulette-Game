using SKC.DeterministicRoulette.Bet;
using SKC.DeterministicRoulette.Core;
using SKC.DeterministicRoulette.Helpers;
using SKC.DeterministicRoulette.Wheel;
using SKC.DeterministicRoulette.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SKC.DeterministicRoulette.Save;
using SKC.DeterministicRoulette.Ball;
using SKC.DeterministicRoulette.Sound;

namespace SKC.DeterministicRoulette.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WheelTable _wheelTable;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private BetManager _betManager;
        [SerializeField] private BallController _ballController;

        [Space, Header("Panels")]
        [SerializeField] private CanvasGroup _moneyCG;
        [SerializeField] private CanvasGroup _betCG = null;
        [SerializeField] private CanvasGroup _lossPanel = null;
        [SerializeField] private CanvasGroup _winPanel = null;
        [SerializeField] private CanvasGroup _cheatPanel = null;

        [Space, Header("Cheat Features")]
        [SerializeField] private Button _cheatOpen = null;
        [SerializeField] private Button _cheatClose = null;
        [SerializeField] private Button _moneyCheat = null;
        [SerializeField] private TMP_Dropdown _dropDownMenu = null;

        [Space, Header("History")]
        [SerializeField] private RectTransform _historyPanel = null;
        [SerializeField] private Button _historyButton = null;
        [SerializeField] private GameObject _historyContainer = null;
        [SerializeField] private TextMeshProUGUI _historyText = null;

        [Space, Header("Buttons")]
        [SerializeField] private Button _spinButton = null;
        [SerializeField] private Button _clearButton = null;
        [SerializeField] private Button _lossAgainButton = null;
        [SerializeField] private Button _winAgainButton = null;

        [Space, Header("Chip Buttons")]
        [SerializeField] private ChipButton[] _chips;
        [SerializeField] private Button _leftButton = null;
        [SerializeField] private Button _rightButton = null;

        [Space, Header("Texts")]
        [SerializeField] private TextMeshProUGUI _moneyText = null;
        [SerializeField] private TextMeshProUGUI _bettedChips = null;
        [SerializeField] private TextMeshProUGUI _lossInfoText = null;
        [SerializeField] private TextMeshProUGUI _winInfoText = null;


        // Private
        private int _winnerNumber = -1;
        private int _indexChip = 0;
        private const int HISTORY_INDEX = 0;
        private bool _historyToggle = false;

        private void Start()
        {
            _clearButton.onClick.AddListener(() => ClearBet(true));
            _spinButton.onClick.AddListener(() => Spin());
            _lossAgainButton.onClick.AddListener(() => Again(false));
            _winAgainButton.onClick.AddListener(() => Again(true));
            _leftButton.onClick.AddListener(() => ChipChange(0));
            _rightButton.onClick.AddListener(() => ChipChange(1));
            _historyButton.onClick.AddListener(() => HistoryToggle());
            _cheatOpen.onClick.AddListener(() => CheatPanel());
            _cheatClose.onClick.AddListener(() => CheatPanel());
            _moneyCheat.onClick.AddListener(() => CheatMoney());
            _dropDownMenu.onValueChanged.AddListener(delegate { DropdownValueChanged(_dropDownMenu); });

            RefreshMoney();
            ChipChange(2);
        }

        private void OnEnable()
        {
            BasicEvents.WinnerNumber += SetWinnerNumber;
            BasicEvents.RefreshMoney += RefreshMoney;
        }

        private void OnDisable()
        {
            BasicEvents.WinnerNumber -= SetWinnerNumber;
            BasicEvents.RefreshMoney -= RefreshMoney;
        }

        private void Spin()
        {
            if (_betManager.UnconfirmedChip <= 0) return;

            _cameraManager.CameraChange(1);
            _betManager.ClearVisualChip();
            CanvasGroupEditor(_betCG);
            StartCoroutine(DelayedAction());

            SoundManager.Instance.PlayClickSound();

            IEnumerator DelayedAction()
            {
                yield return Helper.GetWait(1.0f);
                _wheelTable.Spin();
            }
        }

        private void Again(bool win)
        {
            SoundManager.Instance.PlayClickSound();
            _betManager.ResetBets();
            BettedChips(0);
            ClearBet(false);

            if (win)
            {
                CanvasGroupEditor(_winPanel);
                CanvasGroupEditor(_betCG);
                _cameraManager.CameraChange(0);
                return;
            }

            CanvasGroupEditor(_lossPanel);
            CanvasGroupEditor(_betCG);
            _cameraManager.CameraChange(0);
        }

        private void ClearBet(bool fromButton)
        {
            SoundManager.Instance.PlayClickSound();
            _betManager.ClearBet(fromButton);
            BettedChips(0);
            _betManager.ResetBets();
        }

        public void BettedChips(int bettedChips)
        {
            _bettedChips.SetText($"BET <color=green>$</color>{bettedChips}");
        }

        private void SetWinnerNumber(int winnerNumber, NumberColor _winnerColor)
        {
            _winnerNumber = winnerNumber;
        }

        public void Loss()
        {
            string temp = $"<b><color=green>{_winnerNumber}</color></b>  SORRY, NO WINNER THERE.";
            _lossInfoText.SetText(temp);
            CanvasGroupEditor(_lossPanel);
        }

        public void Win(int gainedChips)
        {
            string temp = $"<b><color=green>{_winnerNumber}</color></b>  CONGRATULATIONS \n" +
                $"YOU HAVE WON ${gainedChips}";
            _winInfoText.SetText(temp);
            CanvasGroupEditor(_winPanel);
        }

        private void ChipChange(int _which) // 0 = left, 1 = right
        {
            SoundManager.Instance.PlayClickSound();

            if (_which == 0) _indexChip -= 1;
            else if(_which == 1) _indexChip += 1;

            if (_indexChip < 0) _indexChip = 4;
            if (_indexChip > 4) _indexChip = 0;

            foreach (ChipButton item in _chips)
            {
                item.gameObject.SetActive(false);
            }

            _chips[_indexChip].gameObject.SetActive(true);
            _chips[_indexChip].RefreshChip();
            _betManager.ClearVisualChip();

        }

        private void HistoryToggle()
        {
            SoundManager.Instance.PlayClickSound();
            _historyToggle = !_historyToggle;

            if(_historyToggle)
            {
                string temp = string.Empty;
                temp = "Last Win ";
                if(SaveManager.SaveData.LatestWinnerNumber.Count <= 5)
                {
                    foreach (int item in SaveManager.SaveData.LatestWinnerNumber)
                    {
                        temp += item.ToString() + ", ";
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        temp += SaveManager.SaveData.LatestWinnerNumber[i].ToString() + ", ";
                    }
                }

                temp += "\n" + $"Spin Count = <color=yellow>{SaveManager.SaveData.SpinCount}</color>" + "\n" +
                    $"Win Count = <color=green>{SaveManager.SaveData.WinCount}</color>" + "\n" +
                    $"Loss Count = <color=red>{SaveManager.SaveData.LossCount}</color>" + "\n" +
                    $"Total Gained = <color=green>{SaveManager.SaveData.TotalGainChip}</color>" + "\n" +
                    $"Total Loss = <color=red>{SaveManager.SaveData.TotalLossChip}</color>";

                _historyText.SetText(temp);

                _historyPanel.sizeDelta = new Vector2(300, 600);
                _historyContainer.gameObject.SetActive(true);
                return;
            }

            _historyPanel.sizeDelta = new Vector2(300, 250);
            _historyContainer.gameObject.SetActive(false);
        }

        private void CheatPanel()
        {
            SoundManager.Instance.PlayClickSound();
            CanvasGroupEditor(_cheatPanel);
        }

        public void DropdownValueChanged(TMP_Dropdown dropDown)
        {
            SoundManager.Instance.PlayClickSound();
            _ballController.Cheat(dropDown.value - 1);
        }

        private void CheatMoney()
        {
            SoundManager.Instance.PlayClickSound();
            SaveManager.SaveData.SoftCurrency += 1000;
            BasicEvents.InvokeSaveGame();
        }

        // Refresh money ui
        private void RefreshMoney()
        {
            _moneyText.SetText($"<color=green>$</color> {SaveManager.SaveData.SoftCurrency}");
        }

        private void CanvasGroupEditor(CanvasGroup cg)
        {
            StartCoroutine(DoFade(cg));
        }

        // Fade canvas groups
        private IEnumerator DoFade(CanvasGroup cg)
        {
            if (cg.alpha >= 1.0f)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;

                while (cg.alpha > 0)
                {
                    cg.alpha -= 0.05f;
                    yield return Helper.GetWait(0.01f);
                }

                cg.alpha = 0.0f; 
            }
            else
            {
                while (cg.alpha < 1.0f)
                {
                    cg.alpha += 0.05f;
                    yield return Helper.GetWait(0.01f);
                }

                cg.interactable = true;
                cg.blocksRaycasts = true;
                cg.alpha = 1.0f;
            }
        }
    }
}
