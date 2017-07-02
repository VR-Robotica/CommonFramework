using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VR_Robotica.Patterns.Design
{
	/// <summary>
	/// Singleton Design Pattern: 
	/// Often inheritted by Manager scripts to maintain a single instance across scenes
	/// </summary>

	public class VRR_SimpleSingleton : MonoBehaviour
	{
		public static VRR_SimpleSingleton Instance;

		protected void Awake()
		{
			if (Instance == null)
			{
				// Ensure persistent presence accross scenes:
				DontDestroyOnLoad(this.gameObject);
				// Create Singelton instance:
				Instance = this;
			}
			else
			// if a manager already exists (check by calling the static variable),
			// and it's not this one (that we are attempting to create)...
			if (Instance != this)
			{
				//...then delete this one, we don't need two!
				Destroy(gameObject);
			}
		}
	}
}