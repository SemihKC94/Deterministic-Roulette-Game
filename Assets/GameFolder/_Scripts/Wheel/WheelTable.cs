using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.DeterministicRoulette.Helpers;
using SKC.DeterministicRoulette.Ball;
using SKC.DeterministicRoulette.Bet;

namespace SKC.DeterministicRoulette.Wheel
{
    public class WheelTable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BallController _ball;
        [SerializeField] private BetManager _betManager;
        [SerializeField] private GameObject[] _checkerPots;

        [Space, Header("Settings")]
        [SerializeField] private float _speed = 0.3f;
        [Tooltip("Just For Info")][SerializeField] private bool _spinning = true;

        // Privates
        private readonly Vector3 _pivot = Vector3.up; // Cache pivot

        // Access
        public GameObject[] CheckerPots { get { return _checkerPots; } }

        void FixedUpdate()
        {
            if (_spinning)
                transform.Rotate(_pivot * _speed);
        }

        public virtual void Spin()
        {
            if (_betManager.UnconfirmedChip <= 0) return;

            //BetSpace.EnableBets(false);
            //SceneRoulette.GameStarted = true;
            _ball.StartSpin();
            StartCoroutine(SetResult());
        }

        private IEnumerator SetResult()
        {
            yield return Helper.GetWait(2.0f);
            _ball.SetWinnerNumber(Random.Range(0, 37), false);
        }
    }
}
