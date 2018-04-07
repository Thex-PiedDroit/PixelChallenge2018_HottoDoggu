
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventsManager : MonoBehaviour
{
#region Variables (public)

	static public EventsManager Instance = null;

	public List<GroundShaker> m_pGroundShakers = null;
	public List<GameEvent> m_pAllEvents = null;

	public float m_fSmallestRandomTimeBeforeEvent = 20.0f;
	public float m_fBiggestRandomTimeBeforeEvent = 30.0f;

	#endregion

#region Variables (private)

	private List<GroundShaker> m_pGroundShakersStillStanding = null;
	private List<GameEvent> m_pAllEventsStillAlive = null;

	#endregion


	private void Awake()
	{
		Toolkit.InitSingleton(this, ref Instance);
	}

	public void Init()
	{
		m_pGroundShakersStillStanding = m_pGroundShakers.Clone();
		m_pAllEventsStillAlive = m_pAllEvents.Clone();

		for (int i = 0; i < m_pGroundShakersStillStanding.Count; ++i)
			m_pGroundShakersStillStanding[i].Reset();
		for (int i = 0; i < m_pAllEventsStillAlive.Count; ++i)
			m_pAllEventsStillAlive[i].Reset();

		StartCoroutine(GroundShakersLoop());
		StartCoroutine(LaunchEvents());
	}

	private IEnumerator GroundShakersLoop()
	{
		float fStartTime = Time.time;

		while (Time.time - fStartTime < 20.0f)
			yield return false;

		int iRandomGround = Random.Range(0, m_pGroundShakersStillStanding.Count);
		m_pGroundShakersStillStanding[iRandomGround].StartShaking();
		m_pGroundShakersStillStanding.RemoveAt(iRandomGround);

		if (m_pGroundShakersStillStanding.Count > 0)
			StartCoroutine(GroundShakersLoop());
	}

	private IEnumerator LaunchEvents()
	{
		float fRandomWait = Random.Range(m_fSmallestRandomTimeBeforeEvent, m_fBiggestRandomTimeBeforeEvent);

		float fStartTime = Time.time;
		while (Time.time - fStartTime < fRandomWait)
			yield return false;

		int iRandomEvent = Random.Range(0, m_pAllEventsStillAlive.Count);
		m_pAllEventsStillAlive[iRandomEvent].gameObject.SetActive(true);
		m_pAllEventsStillAlive[iRandomEvent].LaunchEvent();
		m_pAllEventsStillAlive.RemoveAt(iRandomEvent);

		if (m_pAllEventsStillAlive.Count > 0)
			StartCoroutine(LaunchEvents());
	}
}
