using SKC.DeterministicRoulette.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Wheel
{
    public class WheelNumber : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private NumberBase _wheelNumberBase = null;

        // Private
        private float _latestAngle;

        // Access
        public NumberBase WheelNumberBase { get { return _wheelNumberBase; }}
        public float LatestAngle { get { return _latestAngle; } set { _latestAngle = value; } }

        [ContextMenu("Give Name")]
        public void GiveName()
        {
            if (_wheelNumberBase.Number.Length <= 0) return;
            this.gameObject.name = _wheelNumberBase.Number.ToString() + " " + _wheelNumberBase.Color.ToString();
        }

        [ContextMenu("Set Angle")]
        public void SetAngle()
        {
            this.gameObject.transform.localEulerAngles += Vector3.up * -180f;
            this.gameObject.transform.GetChild(0).localEulerAngles += Vector3.up * -180f;
        }
    }
}
