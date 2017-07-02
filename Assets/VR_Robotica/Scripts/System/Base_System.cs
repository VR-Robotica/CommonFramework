using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VR_Robotica.Base
{
	/// <summary>
	/// Base Class for a Singleton System Managers:
	/// 
	/// </summary>

	public class Base_SystemManager : MonoBehaviour //VRR_Singleton<Base_SystemManager>
	{
		public GameObject ExplosionPrefab;

		private bool isPaused;

		public virtual void Game_Start()
		{

		}

		public virtual void Game_Restart()
		{

		}

		public virtual void Player_SpawnIn()
		{

		}

		public virtual void Player_Respawn()
		{

		}

		public virtual void Player_LostLife()
		{

		}

		public virtual void Player_Died()
		{

		}

		public virtual void Enemy_SpawnIn()
		{

		}

		public virtual void Enemy_Destroyed(Vector3 objectPosition, int pointValue, int hitByID)
		{

		}

		public virtual void Boss_Destoyed()
		{

		}

		public virtual void Explode(Vector3 objectPostion)
		{
			// instantiate an explosion in the position passsed
			Instantiate(ExplosionPrefab, objectPostion, Quaternion.identity);
		}

		public bool IsPaused
		{
			get
			{
				return isPaused;
			}
			set
			{
				isPaused = value;

				if(isPaused)
				{
					// Pause Game Time
					Time.timeScale = 0.0f;
				}
				else
				{
					// Unpause Game Time
					Time.timeScale = 1.0f;
				}
			}
		}
	}
}