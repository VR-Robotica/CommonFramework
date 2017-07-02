using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using com.VR_Robotica.Base;
using com.VR_Robotica.Patterns.Design;

namespace com.VR_Robotica.Managers
{
	/// <summary>
	/// Game/System Manager that inherits from the Singleton Pattern
	/// </summary>
	
	public class Manager_System : VRR_Singleton<Manager_System> //VRR_Singleton<Manager_System>
	{
		public string[] SplashScenes;
		public float	TimeBetweenSplashScenes;
		[Space]
		public string[] GameScenes;
		[Space]
		public GameObject	Player;
		public GameObject	FloorMenu;

		private Manager_Scenes sceneManager;
		private int nextSceneIndex = 0; // default starting index
		private Vector3 playerOrigin;

		// Use this for initialization
		void Start()
		{
			getReferences();

			if (GameScenes != null && GameScenes.Length > 0)
			{
				Start_DelayedLoading();
			}

			HideFloorMenu();
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				//QuitApplication();
				Application.Quit();
			}
		}

		private void getReferences()
		{
			// Public References
			if (Player == null)
			{
				Player = GameObject.FindGameObjectWithTag("Player");
			}

			// Private References
			sceneManager = this.gameObject.GetComponent<Manager_Scenes>();
			playerOrigin = Player.transform.position;
		}

		#region DELAY LOADING THE NEXT SCENE
		public void Start_DelayedLoading()
		{
			Debug.Log("Delay STARTED...");

			Stop_DelayedLoading();
			container_delayedLoading = delayedLoading();
			StartCoroutine(container_delayedLoading);
		}

		public void Stop_DelayedLoading()
		{
			if (container_delayedLoading != null)
			{
				Debug.Log("Delay STOPPED...");

				StopCoroutine(container_delayedLoading);
				container_delayedLoading = null;
			}
		}

		private IEnumerator container_delayedLoading;
		private IEnumerator delayedLoading()
		{
			Debug.Log("Delay Coroutine for " + TimeBetweenSplashScenes + " second(s).");
			yield return new WaitForSeconds(TimeBetweenSplashScenes);

			Debug.Log("Now loading Scene...");
			LoadNextScene();

			yield return null;

			Stop_DelayedLoading();
		}
		#endregion

		public void LoadNextScene()
		{
			// if just starting or at the end, start at 0
			if (nextSceneIndex == 0 || nextSceneIndex > GameScenes.Length - 1)
			{
				nextSceneIndex = 0;
			}

			Debug.Log("Next Scene Index to Load is: " + nextSceneIndex + " : " + GameScenes[nextSceneIndex]);

			sceneManager.Start_LoadingScene(GameScenes[nextSceneIndex]);
			nextSceneIndex += 1;
		}

		public void QuitApplication()
		{
			sceneManager.FadeToQuit();
		}

		public void HideFloorMenu()
		{
			if(FloorMenu != null)
			{
				FloorMenu.SetActive(false);
			}
		}

		public virtual void ShowFloorMenu()
		{
			if (FloorMenu != null)
			{
				FloorMenu.SetActive(true);
			}
		}

		public virtual void ResetPlayerPosition()
		{
			Player.transform.position = playerOrigin;
		}
	}
}
