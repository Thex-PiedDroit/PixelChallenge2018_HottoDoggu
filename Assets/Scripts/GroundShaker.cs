
using UnityEngine;
using System.Collections;

public class GroundShaker : MonoBehaviour
{
#region Variables (public)

	public Animator m_pAnimator = null;

	public float m_fDelayBetweenShakeAndFall = 3.0f;

	#endregion

#region Variables (private)



	#endregion


	public void Reset()
	{
		m_pAnimator.SetTrigger("Reset");
	}

	public void StartShaking()
	{
		m_pAnimator.SetTrigger("Shake");
		StartCoroutine(WaitBeforeFalling());
	}

	private IEnumerator WaitBeforeFalling()
	{
		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fDelayBetweenShakeAndFall)
			yield return false;

		m_pAnimator.SetTrigger("Fall");
	}
}
