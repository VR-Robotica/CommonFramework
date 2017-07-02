using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
// using TMPro;

using com.VR_Robotica.Base;
using com.VR_Robotica.Patterns.Design;
using com.VR_Robotica.Utils;

namespace com.VR_Robotica.Managers
{
	/// <summary>
	/// This Manages the loading of the core elements of a scene.
	/// It starts off as a light weight load screen with UI elements, 
	/// then automatically loads the core scene asynchronously.
	/// </summary>

	//	[RequireComponent(typeof(Manager_System))]
	public class Manager_Scenes : MonoBehaviour
	{
		/// <summary>
		/// The Unity scene used as the loading screen with custom environment & animations
		/// </summary>
		public string LoadingScene;

		/// <summary>
		/// An array of the scene names reflected in the build settings.
		/// We use this array to load the core scenes because the build settings 
		/// will have other non-level related scenes that mix up the array order.
		/// 
		/// * these scenes must be added to the build settings in order to properly
		/// access them.
		/// </summary>
		public string[] CoreScenes;

		/// <summary>
		/// The current scene index of the CoreScenes array
		/// </summary>
		public int CurrentSceneIndex = 0;

		/// <summary>
		/// The graphic object on the main camera that occludes the view when
		/// switching scenes.
		/// </summary>
		private CanvasFader canvasToFade;

		/// <summary>
		/// Operation object to control asynchronous loading
		/// </summary>
		private AsyncOperation asyncOperation;

		/// <summary>
		/// Minimum duration we hold the loading scene.
		/// This adds a slight delay to ensure the in-between scene loading gets seen
		/// by the user
		/// </summary>
		private float minDuration = 1.5f;
		private float delayTime;

		private GameObject loadingBar;
		private GameObject loadingPercentage;
		private Text percentage;
		//private TextMeshPro percentage;

		private float barY;
		private float barZ;

		private void Awake()
		{
			canvasToFade = GameObject.FindObjectOfType<CanvasFader>();
			if (canvasToFade == null)
			{
				Debug.LogError("Canvas to fade NOT Found!");
			}

			if(CoreScenes.Length == 0)
			{
				Debug.LogError("There are no scenes in this game!");
			}
		}

		/// <summary>
		/// Simply resets the current level/scene index
		/// </summary>
		public void ResetGame()
		{
			CurrentSceneIndex = 0;
		}

		/// <summary>
		/// Moves to the next index in the CoreScenes array
		/// </summary>
		public void GotoNextLevel()
		{
			if (CurrentSceneIndex < CoreScenes.Length)
			{
				CurrentSceneIndex++;
			}
			else
			{
				CurrentSceneIndex = 0;
			}

			LoadScene(CurrentSceneIndex);
		}

		public void LoadScene(int index)
		{
			CurrentSceneIndex = index;
			Start_LoadingScene(CoreScenes[CurrentSceneIndex]);
		}

		#region LOADING - SCENE LOADER
		/// <summary>
		/// I use a COROUTINE Container to avoid the potential Unity bug where Unity
		/// fails to stop the coroutine when requested. So I double my effort to kill
		/// the coroutine by calling a "StopCoroutine" on the container 
		/// AND then setting the container to "null"! 
		/// </summary>
		public void Start_LoadingScene(string sceneName)
		{
			Stop_LoadingScene();
			container_loadingLoadScene = loadingLoadScene(sceneName);
			StartCoroutine(container_loadingLoadScene);
		}

		public void Stop_LoadingScene()
		{
			if (container_loadingLoadScene != null)
			{
				StopCoroutine(container_loadingLoadScene);
				container_loadingLoadScene = null;
			}
		}
		
		private IEnumerator container_loadingLoadScene;
		private IEnumerator loadingLoadScene(string sceneName)
		{
			Manager_System.Instance.ResetPlayerPosition();
			Manager_System.Instance.HideFloorMenu();


			//	canvasToFade = GameObject.FindGameObjectWithTag("FadingCanvas").GetComponent<CanvasFader>();
			if (canvasToFade != null)
			{
				canvasToFade.gameObject.SetActive(true);
				// *** FADE TO LOADING SCREEN ***

				// check to see if canvasToFade is ready
				while (!canvasToFade.IsReady)
				{
					// hold until it is...
					yield return null;
				}

				// Fade-To-Black Coroutine (Current Scene --> Black)
				yield return StartCoroutine(canvasToFade.FadeIn());
			}

			// Now Load the Loading Screen as a NEW SCENE
			yield return SceneManager.LoadSceneAsync(LoadingScene);

			findLoadingBar();

			// Fade-To-Clear Coroutine, (Black --> Loading Screen)
			yield return StartCoroutine(canvasToFade.FadeOut());

			// Minimum DELAY 
			delayTime = Time.time + minDuration;
			while (Time.time < delayTime)
			{
				yield return null;
			}

			//	canvasToFade = null;
			Start_LoadCoreScene(sceneName);
			Stop_LoadingScene();
		}
		#endregion

		#region LOADING - SCENE CORE 
		/// <summary>
		/// Core scenes are loaded additively to the LoadingScene
		/// </summary>
		/// <param name="sceneName"></param>
		public void Start_LoadCoreScene(string sceneName)
		{
			Stop_LoadCoreScene();
			container_loadingCore = loadingCore(sceneName);
			StartCoroutine(container_loadingCore);
		}

		public void Stop_LoadCoreScene()
		{
			if (container_loadingCore != null)
			{
				StopCoroutine(container_loadingCore);
				container_loadingCore = null;
			}
		}

		private IEnumerator container_loadingCore;
		private IEnumerator loadingCore(string sceneName)
		{
			// load core scene as an ADDITIVE SCENE
			asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			asyncOperation.allowSceneActivation = false;

			// isDone will only return true 
			// if the data is loaded AND the scene is active.
			while (!asyncOperation.isDone)
			{
				// track the progression of the loading here:
				// [0, 0.9] > [0, 1]

				if (asyncOperation.progress <= 0.9f)
				{
					float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
					if (loadingBar != null)
					{
						loadingBar.transform.localScale = new Vector3(progress, barY, barZ);
						percentage.text = Mathf.Round(progress * 100) + "%";
					}
				}

				// Progress only returns values between 0 and 0.9; 
				// 1 is only returned if the scene is ALSO active, 
				// in which isDone is set to true

				if (asyncOperation.progress == 0.9f)
				{
					// Loading completed...
					yield return StartCoroutine(canvasToFade.FadeIn());

					// Activate Scene		
					asyncOperation.allowSceneActivation = true;
					Manager_LoadingScene.UnloadLoadingScene();
				}

				yield return null;
			}

			//	canvasToFade = GameObject.FindGameObjectWithTag("FadingCanvas").GetComponent<CanvasFader>();
			if (canvasToFade != null)
			{
				// Fade-In Coroutine, (Black --> New Scene)
				yield return StartCoroutine(canvasToFade.FadeOut());

				canvasToFade.gameObject.SetActive(false);
			}

			if (sceneName != "MainMenu")
			{
				Manager_System.Instance.ShowFloorMenu();
			}

			Stop_LoadCoreScene();
		}
		#endregion

		#region LOADER UI
		/// <summary>
		/// When a new loading scene is loaded, we need to find the UI elements.
		/// This may be an expensive process, but the scene itself is tiny and quick
		/// </summary>
		private void findLoadingBar()
		{
			loadingBar = GameObject.Find("LoadingBar");
			if (loadingBar != null)
			{
				barY = loadingBar.transform.localScale.y;
				barZ = loadingBar.transform.localScale.z;

				loadingBar.transform.localScale = new Vector3(0, barY, barZ);
			}
			else
			{
				Debug.LogWarning("Can't find the loading bar.");
			}

			loadingPercentage = GameObject.Find("Percentage");
			if (loadingPercentage != null)
			{
				percentage = loadingPercentage.GetComponent<Text>();
				//percentage = loadingPercentage.GetComponent<TextMeshPro>();
				if (percentage != null)
				{
					percentage.text = "0%";
				}
				else
				{
					Debug.LogWarning("TextMesh Pro not attached to percentage readout.");
				}
			}
			else
			{
				Debug.LogWarning("Can't find the percentage readout.");
			}
		}
		#endregion

		/// <summary>
		/// Fade the graphic one last time before the application quits
		/// </summary>
		/// <returns></returns>
		public IEnumerator FadeToQuit()
		{
			// check to see if canvasToFade is ready
			while (!canvasToFade.IsReady)
			{
				// hold until it is...
				yield return null;
			}

			// Fade-To-Black Coroutine (Current Scene --> Black)
			yield return StartCoroutine(canvasToFade.FadeIn());

			Application.Quit();
		}
	}
}