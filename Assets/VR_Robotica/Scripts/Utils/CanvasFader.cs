using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A more complex and robust fading script for UI objects with the Canvas Renderer component.
/// Primarily used for camera fading during scene changes
/// </summary>
namespace com.VR_Robotica.Utils
{
	public class CanvasFader : MonoBehaviour
	{
		public bool StartVisible;

		[HideInInspector]
		public bool IsReady;

		private bool m_startsVisible;// = false;
		private bool m_fadeOnAwake = false;
		private bool m_continuous = false;
		private float m_fadeSpeed = 1.0f;
		private float m_minAlpha = 0;
		private float m_maxAlpha = 1.0f;

		private CanvasRenderer m_canvasRenderer = null;

		public bool isVisible
		{
			get
			{
				if (m_canvasRenderer.GetAlpha() == m_maxAlpha)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public bool isHidden
		{
			get
			{
				if (m_canvasRenderer.GetAlpha() == m_minAlpha)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public float fadeSpeed
		{
			get
			{
				return m_fadeSpeed;
			}

			set
			{
				m_fadeSpeed = value;
			}
		}

		public float minAlpha
		{
			get
			{
				return m_minAlpha;
			}

			set
			{
				m_minAlpha = value;
			}
		}

		public float maxAlpha
		{
			get
			{
				return m_maxAlpha;
			}

			set
			{
				m_maxAlpha = value;
			}
		}

		public bool continuous
		{
			get
			{
				return m_continuous;
			}

			set
			{
				m_continuous = value;
			}
		}

		void Start()
		{
			m_canvasRenderer = GetComponent<CanvasRenderer>();
			m_startsVisible = StartVisible;

			if (m_canvasRenderer == null)
			{
				Debug.LogError("FadeCanvas: No CanvasRenderer found!");
				return;
			}

			if (m_startsVisible)
			{
				m_canvasRenderer.SetAlpha(m_maxAlpha);

				if (m_fadeOnAwake)
				{
					Start_FadeOut();
				}
			}
			else
			{
				m_canvasRenderer.SetAlpha(m_minAlpha);

				if (m_fadeOnAwake)
				{
					Start_FadeIn();
				}
			}

			IsReady = true;
		}

		public void Start_FadeIn()
		{
			Stop_FadeIn();
			container_fadeIn = FadeIn();
			StartCoroutine(container_fadeIn);
		}

		public void Stop_FadeIn()
		{
			if (container_fadeIn != null)
			{
				StopCoroutine(container_fadeIn);
				container_fadeIn = null;
			}
		}

		private IEnumerator container_fadeIn;
		public IEnumerator FadeIn()
		{
			float fadeAmount;

			if (m_canvasRenderer != null)
			{
				while (m_canvasRenderer.GetAlpha() < m_maxAlpha)
				{
					yield return null;

					fadeAmount = m_canvasRenderer.GetAlpha() + Time.deltaTime / m_fadeSpeed;

					if (fadeAmount > 0.99f)
					{
						fadeAmount = maxAlpha;
					}

					m_canvasRenderer.SetAlpha(fadeAmount);
				}
			}

			m_canvasRenderer.SetAlpha(m_maxAlpha);

			if (m_continuous)
			{
				Start_FadeOut();
			}
		}

		public void Start_FadeOut()
		{
			Stop_FadeIn();
			container_fadeOut = FadeOut();
			StartCoroutine(container_fadeOut);
		}

		public void Stop_FadeOut()
		{
			if (container_fadeOut != null)
			{
				StopCoroutine(container_fadeOut);
				container_fadeOut = null;
			}
		}

		private IEnumerator container_fadeOut;
		public IEnumerator FadeOut()
		{
			float fadeAmount;

			while (m_canvasRenderer.GetAlpha() > m_minAlpha)
			{
				yield return null;
				fadeAmount = m_canvasRenderer.GetAlpha() - Time.deltaTime / m_fadeSpeed;

				if (fadeAmount < 0.01f)
				{
					fadeAmount = minAlpha;
				}

				m_canvasRenderer.SetAlpha(fadeAmount);
			}

			m_canvasRenderer.SetAlpha(m_minAlpha);

			if (m_continuous)
			{
				Start_FadeIn();
			}
		}
	}
}