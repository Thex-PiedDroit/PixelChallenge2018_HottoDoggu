
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventsManager : MonoBehaviour
{
#region Variables (public)

	static public EventsManager Instance = null;

	public List<GroundShaker> m_pGroundShakers = null;

	#endregion

#region Variables (private)

	private List<GroundShaker> m_pGroundShakersStillStanding = null;

	#endregion


	private void Awake()
	{
		Toolkit.InitSingleton(this, ref Instance);
	}

	public void Init()
	{
		m_pGroundShakersStillStanding = m_pGroundShakers.Clone();

		for (int i = 0; i < m_pGroundShakersStillStanding.Count; ++i)
			m_pGroundShakersStillStanding[i].Reset();
		StartCoroutine(GroundShakersLoop());
	}

	private IEnumerator GroundShakersLoop()
	{
		float fStartTime = Time.time;

		while (Time.time - fStartTime < 5.0f)
			yield return false;

		int iRandomGround = Random.Range(0, m_pGroundShakersStillStanding.Count);
		m_pGroundShakersStillStanding[iRandomGround].StartShaking();
		m_pGroundShakersStillStanding.RemoveAt(iRandomGround);

		if (m_pGroundShakersStillStanding.Count > 0)
			StartCoroutine(GroundShakersLoop());
	}
}
