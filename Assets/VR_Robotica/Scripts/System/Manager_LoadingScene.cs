using UnityEngine;
using System.Collections;

namespace com.VR_Robotica.Managers
{
	/// <summary>
	/// A Simple Manager to easily call for the "loading scene's" destruction or disabling.
	/// 
	/// The "loading scene" is a tiny light weight scene that holds the UI graphics and 
	/// environment that displays the loading progress of an asynchronous additive load event.
	/// </summary>

	public class Manager_LoadingScene : MonoBehaviour
	{
		public static Manager_LoadingScene Instance;

		private void Awake()
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

		public static void UnloadLoadingScene()
		{
			GameObject.Destroy(Instance.gameObject);
		}

		public static void HideLoadingScene()
		{
			Instance.gameObject.SetActive(false);
		}

		public static void ShowLoadingScene()
		{
			Instance.gameObject.SetActive(true);
		}

		public static void ResetLoadingScene()
		{
			// reset UI components
		}
	}
}