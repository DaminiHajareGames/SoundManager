using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FP11.LudoGame
{
	public class ButtonSound : MonoBehaviour
	{
		void Start()
		{
			Button btn = GetComponent<Button>();
			btn.onClick.AddListener(TaskOnClick);
		}

		void TaskOnClick()
		{
			SoundManager.instance.GenrateSoundForGenralButton();
		}
	}
}
