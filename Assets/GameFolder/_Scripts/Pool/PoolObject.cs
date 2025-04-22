using System;
using UnityEngine;
using SKC.DeterministicRoulette.Core;
using TMPro;

namespace SKC.DeterministicRoulette.Pool
{
	public class PoolObject : MonoBehaviour
	{
		public ChipType ChipType;

		public event Action Destroyed;

		private TextMeshPro _text;

        private void Start()
        {
			if (!GetComponentInChildren<TextMeshPro>()) return;

			_text = GetComponentInChildren<TextMeshPro>();
			_text.SetText(((int)ChipType).ToString());
        }

		public void SetInactive()
        {
			transform.SetParent(null);
			gameObject.SetActive(false);
        }

        void OnDestroy()
		{
			Destroyed?.Invoke();
		}
	}
}
