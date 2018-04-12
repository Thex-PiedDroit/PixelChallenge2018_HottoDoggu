
using UnityEngine;
using System.Collections;

public class Cows : GameEvent
{
	#region Variables (public)

	public AudioSource m_pAudioSourceMoo = null;
	public AudioSource m_pAudioSourceGallop = null;

	public float m_fTravelSpeed = 8.0f;

	public float m_fInitPosX = 20.0f;
	public float m_fDestinationX = 0.0f;
	public float m_fMaxOffsetZ = 2.0f;

	public float m_fPower = 80.0f;

	#endregion

#region Variables (private)



	#endregion


	public override void Reset()
	{
		transform.position = new Vector3(m_fInitPosX, 0.0f, Random.Range(-m_fMaxOffsetZ, m_fMaxOffsetZ));
	}

	public override void LaunchEvent()
	{
		AudioManager.Instance.PlaySound("Cow_Moo", m_pAudioSourceMoo);
		AudioManager.Instance.PlaySound("Cow_Gallop", m_pAudioSourceGallop, true);
		StartCoroutine(Travel());
	}

	private IEnumerator Travel()
	{
		while (transform.position.x > m_fDestinationX)
		{
			transform.position += (transform.forward * m_fTravelSpeed) * Time.deltaTime;
			yield return false;
		}

		gameObject.SetActive(false);
	}

	public void OnCollisionEnter(Collision pCollision)
	{
		Vector3 tDirection = (pCollision.transform.position.x0z() - pCollision.contacts[0].point.x0z()).normalized;
		pCollision.rigidbody.AddForce(tDirection * m_fPower, ForceMode.Impulse);
		pCollision.gameObject.GetComponent<Character>().StunAndResetConditions();
	}
}
