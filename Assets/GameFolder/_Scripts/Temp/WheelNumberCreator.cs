using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.DeterministicRoulette.Wheel;
using TMPro;
using UnityEditor;

namespace SKC.DeterministicRoulette.Temp
{
    public class WheelNumberCreator : MonoBehaviour
    {
        public WheelNumber WheelNumberPrefab;
        public Transform innerDiskTransform;
        public Transform numbersParent;
        public float diskRadius, numberOffset;
        public int currentNumberOfSegments;

        [ContextMenu("Create")]
        public void CreateWheelNumber()
        {
            float angleStep = 360f / currentNumberOfSegments;

            for (int i = 0; i < currentNumberOfSegments; i++)
            {
                float angle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 position = innerDiskTransform.position + rotation * Vector3.forward * (diskRadius + numberOffset);

                WheelNumber numberObject = PrefabUtility.InstantiatePrefab(WheelNumberPrefab) as WheelNumber;//Instantiate(WheelNumberPrefab, position, rotation, numbersParent);
                numberObject.transform.SetParent(numbersParent);
                numberObject.transform.position = position;
                numberObject.transform.rotation = rotation;
            }
        }

        [ContextMenu("Delete All Children")]
        public void DeleteAllWheelNumber()
        {
            int childCount = numbersParent.childCount;

            for (int i = childCount - 1; i > -1; i--)
            {
                DestroyImmediate(numbersParent.GetChild(i).gameObject);
            }
        }
    }
}
