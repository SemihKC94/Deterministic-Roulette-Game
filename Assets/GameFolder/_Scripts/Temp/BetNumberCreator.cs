using SKC.DeterministicRoulette.Bet;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SKC.DeterministicRoulette.Temp
{
    public class BetNumberCreator : MonoBehaviour
    {
        public BetNumber betNumberPrefab;
        public Vector3 startPosition;
        public int maxNumber;
        public float zOffset, xOffset;

        private int index = 0;

        [ContextMenu("Create")]
        public void CreateNumbers()
        {
            for (int i = 0; i < maxNumber / 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    BetNumber numberObject = PrefabUtility.InstantiatePrefab(betNumberPrefab) as BetNumber;
                    numberObject.transform.SetParent(this.transform);
                    numberObject.transform.position = Vector3.zero;
                    Vector3 nextPosition = startPosition + new Vector3(xOffset * i, 0f, zOffset * j);
                    nextPosition.y = 0;
                    numberObject.transform.position = nextPosition;
                }
            }
        }

        [ContextMenu("Delete All Children")]
        public void DeleteAllWheelNumber()
        {
            int childCount = transform.childCount;

            for (int i = childCount - 1; i > -1; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
