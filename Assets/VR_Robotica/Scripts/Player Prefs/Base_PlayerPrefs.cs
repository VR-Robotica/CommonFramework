using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.VR_Robotica.Base
{
	public class Base_PlayerPrefsManager : MonoBehaviour
	{
		const string KEY_MASTER_VOLUME	= "master_volume";
		const string KEY_SOUND_VOLUME	= "sound_volume";
		const string KEY_MUSIC_VOLUME	= "music_volume";
		const string KEY_DIFFICULTY		= "difficulty";
		const string KEY_LEVEL			= "level_unlocked_";

		#region MASTER VOLUME
		public static void SetMasterVolume(float volume)
		{
			if (volume >= 0 && volume <= 1)
			{
				PlayerPrefs.SetFloat(KEY_MASTER_VOLUME, volume);
			}
			else
			{
				Debug.LogError("Master Volume out of range!");
			}
		}

		public static float GetMasterVolume()
		{
			return PlayerPrefs.GetFloat(KEY_MASTER_VOLUME);
		}
		#endregion

		#region SOUND VOLUME
		public static void SetSoundVolume(float volume)
		{
			if (volume >= 0 && volume <= 1)
			{
				PlayerPrefs.SetFloat(KEY_SOUND_VOLUME, volume);
			}
			else
			{
				Debug.LogError("Sound Volume out of range!");
			}
		}

		public static float GetSoundVolume()
		{
			return PlayerPrefs.GetFloat(KEY_SOUND_VOLUME);
		}
		#endregion

		#region MUSIC VOLUME
		public static void SetMusicVolume(float volume)
		{
			if (volume >= 0 && volume <= 1)
			{
				PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, volume);
			}
			else
			{
				Debug.LogError("Music Volume out of range!");
			}
		}

		public static float GetMusicVolume()
		{
			return PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME);
		}
		#endregion

		#region DIFFICULTY SETTINGS
		public static void SetDifficulty(int difficulty)
		{
			/* 0 - Novice
			 * 1 - Easy
			 * 2 - Medium
			 * 3 - Hard
			 * 4 - Expert
			 */
			if (difficulty >= 0 && difficulty <= 4)
			{
				PlayerPrefs.SetInt(KEY_DIFFICULTY, difficulty);
			}
			else
			{
				Debug.LogError("Difficulty set out of bounds");
			}
		}

		public static int GetDifficulty()
		{
			return PlayerPrefs.GetInt(KEY_DIFFICULTY);
		}
		#endregion

		public static void UnlockLevel(int level)
		{
			if (level <= SceneManager.sceneCount - 1)
			{
				PlayerPrefs.SetInt(KEY_LEVEL + level.ToString(), 1);  // USE 1 FOR TRUE
			}
			else
			{
				Debug.LogError("Trying to Unlock Level not in build order!");
			}
		}

		public static bool IsLevelUnlocked(int level)
		{
			int levelValue = PlayerPrefs.GetInt(KEY_LEVEL + level.ToString());
			bool isLevelUnlocked = (levelValue == 1);

			if (level <= SceneManager.sceneCount - 1)
			{
				return isLevelUnlocked;
			}
			else
			{
				Debug.LogError("Querying level not in build order!");
				return false;
			}
		}
	}
}