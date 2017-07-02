using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using TMPro;

namespace VR_Robotica.Managers
{
	/// <summary>
	/// Manages the additive loading of a scene.
	/// 
	/// example: 
	///		Adding an animation sequence of objects from one scene 
	///		into a preloaded environment scene.	
	///	* these loaded scenes should have their own self destruct function call
	/// </summary>

	public class Manager_AdditiveObjectLoading : MonoBehaviour
	{
		// Object at scene origin, that visually displays loading information
		// - animations should be aware of this origin offset.
		public GameObject LoadingDisplay;

		public string SceneToLoad;

		private AsyncOperation asyncOperation;


		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}