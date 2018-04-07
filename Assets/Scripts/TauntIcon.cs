
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TauntIcon : MonoBehaviour
{
#region Variables (public)

	public Image m_pIcon = null;
	public List<Sprite> m_pAllPossibleIcons = null;

	public Transform m_pTarget = null;

	public float m_fLifeDuration = 1.0f;
	public float m_fFollowSpeed = 10.0f;

	#endregion

#region Variables (private)

	private Camera m_pCamera = null;

	#endregion


	public void Init(Transform pTarget)
	{
		if (m_pCamera == null)
			m_pCamera = Camera.main;

		m_pTarget = pTarget;
		transform.position = m_pCamera.WorldToScreenPoint(m_pTarget.position);

		m_pIcon.sprite = m_pAllPossibleIcons[Random.Range(0, m_pAllPossibleIcons.Count)];
		gameObject.SetActive(true);

		StopAllCoroutines();
		StartCoroutine(WaitBeforeHide());
	}

	private void Update()
	{
		if (m_pTarget == null)
			return;

		FollowTarget();
	}

	private void FollowTarget()
	{
		transform.position = Vector3.Slerp(transform.position, m_pCamera.WorldToScreenPoint(m_pTarget.position), m_fFollowSpeed * Time.deltaTime);
	}

	private IEnumerator WaitBeforeHide()
	{
		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fLifeDuration)
			yield return false;

		gameObject.SetActive(false);
	}
}
