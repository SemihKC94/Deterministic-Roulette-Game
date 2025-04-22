using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.DeterministicRoulette.Helpers;
using SKC.DeterministicRoulette.Wheel;
using SKC.DeterministicRoulette.Events;
using SKC.DeterministicRoulette.Save;
using SKC.DeterministicRoulette.Sound;

namespace SKC.DeterministicRoulette.Ball
{
    public class BallController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody spinBall;
        [SerializeField] private Transform originPoint;
        [SerializeField] private WheelTable wheelControll;
        [SerializeField] private Transform pivotTransform;
        [SerializeField] private Transform pivotWheelTransform;

        [Space, Header("Settings")]
        [SerializeField] private bool spinning = false;
        [SerializeField] private int preDeterminedResult = 0;
        [SerializeField] private float jumpDuration = 1.0f;

        // Privates
        private Transform _target;

        private static readonly Vector3 _rotateAxis = Vector3.up;
        private Vector3 _deltaAngle = Vector3.zero;
        private float _angularSpeed = 5f;
        private int _estimatedResult = -1;
        
        private bool _stopping = false;
        private bool _animateBall = true;
        private bool _bouncing = false;


        void Start()
        {
            spinBall.isKinematic = true;
        }

        public void StartSpin()
        {
            spinBall.gameObject.SetActive(true);
            spinBall.isKinematic = true;
            spinBall.transform.SetParent(originPoint);
            spinBall.transform.localPosition = Vector3.zero;
            transform.SetParent(pivotTransform);
            transform.localRotation = Quaternion.identity;
            _angularSpeed = 5;
            spinning = true;
            _animateBall = true;
            SaveManager.SaveData.SpinCount++;
            SoundManager.Instance.PlayRollSound();
        }

        public void Cheat(int number)
        {
            preDeterminedResult = number;
        }

        public void SetWinnerNumber(int estimatedResult, bool isEuropean)
        {
            estimatedResult = estimatedResult == -1 && !isEuropean ? 37 : estimatedResult;
            if(preDeterminedResult >= 0)
                estimatedResult = preDeterminedResult;

            _estimatedResult = estimatedResult;
            _target = wheelControll.CheckerPots[_estimatedResult].transform;

            StartCoroutine(SmoothlyDecreaseAngularSpeed(_angularSpeed, 1.5f, 5f, () =>
            {
                _stopping = true;
            }));
        }

        private IEnumerator SmoothlyDecreaseAngularSpeed(float startValue, float endValue, float duration, System.Action onComplete)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                float normalizedTime = timeElapsed / duration;
                // Use Mathf.Lerp to smoothly interpolate the angular speed
                _angularSpeed = Mathf.Lerp(startValue, endValue, normalizedTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the final value is set
            _angularSpeed = endValue;
            onComplete?.Invoke();
        }

        public void PlaceToResult(float angleRatio)
        {
            spinBall.transform.SetParent(_target);
            _bouncing = true;
            StartCoroutine(PerformLocalJump(spinBall.transform));
            
            //stop ball spin sound
        }

        private IEnumerator PerformLocalJump(Transform ballTransform)
        {
            StartCoroutine(DropSound());

            float jumpPower = 0.1f;

            Vector3 startPosition = ballTransform.localPosition;
            Vector3 targetPosition = Vector3.zero;
            float jumpHeight = jumpPower;

            float timeElapsed = 0f;
            while (timeElapsed < jumpDuration)
            {
                float normalizedTime = timeElapsed / jumpDuration;
                // Linear ease for the horizontal movement
                float horizontalPosition = Mathf.Lerp(startPosition.x, targetPosition.x, normalizedTime);
                float horizontalPositionZ = Mathf.Lerp(startPosition.z, targetPosition.z, normalizedTime);

                // Parabolic curve for the jump height
                float verticalPosition = jumpHeight * Mathf.Sin(normalizedTime * Mathf.PI);

                ballTransform.localPosition = new Vector3(horizontalPosition, startPosition.y + verticalPosition, horizontalPositionZ);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            ballTransform.localPosition = targetPosition;
            _bouncing = false;
        }

        private IEnumerator DropSound()
        {
            while (_bouncing)
            {
                yield return Helper.GetWait(0.30f);
                SoundManager.Instance.PlayDropSound();
            }
        }

        private float CalculateAngleRatio(Vector3 angularCross)
        {
            _deltaAngle = angularCross - _deltaAngle;

            Vector3 targetVector = (_target.position - transform.position);
            Vector3 ballVector = (spinBall.position - transform.position);

            targetVector.y = ballVector.y = 0;

            return (Vector3.Angle(ballVector, targetVector) / 180f);
        }

        private void FixedUpdate()
        {
            if (!spinning)
                return;

            transform.Rotate(_rotateAxis, _angularSpeed);

            if (_stopping)
            {
                Vector3 angularCross = Vector3.Cross(transform.forward, (_target.position - transform.position).normalized);
                float angle = Vector3.SignedAngle(transform.forward, (_target.position - transform.position), transform.up);
                float angleRatio = CalculateAngleRatio(angularCross);
                if (_deltaAngle.y > 0f)
                {
                    if (angle < 35 && angle > 0)
                        _angularSpeed = angleRatio * 2f;

                    if (angleRatio <= 0.2f && _animateBall && angle > 5)
                    {
                        _animateBall = false;
                        PlaceToResult(angleRatio);
                    }
                    else if (angleRatio <= 0.01f && !_animateBall)
                    {
                        spinning = false;
                        transform.SetParent(pivotWheelTransform);
                        spinBall.isKinematic = true;
                        _stopping = false;
                        BasicEvents.InvokeWinnerNumber(_estimatedResult, wheelControll.CheckerPots[_estimatedResult].GetComponent<WheelNumber>().WheelNumberBase.Color);
                        SaveManager.SaveData.LatestWinnerNumber.Insert(0, _estimatedResult);
                    }
                }
            }
        }
    }
}
