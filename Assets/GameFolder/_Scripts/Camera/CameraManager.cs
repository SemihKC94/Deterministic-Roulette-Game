using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SKC.DeterministicRoulette.Core
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera _betCam;
        [SerializeField] private CinemachineVirtualCamera _wheelCam;

        public void CameraChange(int camNumber)
        {
            if(camNumber == 0)
            {
                _betCam.Priority = 20;
                _wheelCam.Priority = 10;
                return;
            }

            _betCam.Priority = 10;
            _wheelCam.Priority = 20;
        }
    }
}
