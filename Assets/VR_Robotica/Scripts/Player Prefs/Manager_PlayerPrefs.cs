using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VR_Robotica.Managers
{
	public class Manager_PlayerPrefs : Base.Base_PlayerPrefsManager
	{
		public Manager_PlayerPrefs Instance;

		void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
		}
	}
}