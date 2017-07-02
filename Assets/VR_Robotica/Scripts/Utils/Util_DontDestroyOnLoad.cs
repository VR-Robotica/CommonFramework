using UnityEngine;

/// <summary>
/// Simply attach this to a Game Object so it isn't destroyed on load
/// 
/// </summary>

namespace com.VR_Robotica.Utils
{
	public class Util_DontDestroyOnLoad : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}