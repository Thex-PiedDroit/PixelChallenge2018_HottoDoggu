
using UnityEngine;

public class Character : MonoBehaviour
{
#region Variables (public)

	public Rigidbody m_pRigidBody = null;
	public Animator m_pAnimator = null;
	public SpriteRenderer m_pSpriteRenderer = null;

	public float m_fAcceleration = 10.0f;
	public float m_fMaxSpeed = 5.0f;

	public float m_fAttackImpulseForce = 10.0f;
	public float m_fRespawnImpactImpulseForce = 15.0f;

	public int m_iCharacterID = 0;

	#endregion

#region Variables (private)

	private bool m_bIsDead = false;

	#endregion


	private void Update()
	{
		CatchInputs();
		UpdateAnimator();
	}

	private void CatchInputs()
	{
		float fHorizontal = Input.GetAxis("Horizontal_" + m_iCharacterID);
		float fVertical= Input.GetAxis("Vertical_" + m_iCharacterID);

		Vector3 tDirection = new Vector3(fHorizontal, 0.0f, fVertical).normalized;

		if (transform.position.y >= -0.05f && (fHorizontal != 0.0f || fVertical != 0.0f) && m_pRigidBody.velocity.x0z().sqrMagnitude < m_fMaxSpeed.Sqrd())
		{
			if (m_bIsDead)
				tDirection *= 0.2f;

			m_pRigidBody.AddForce(tDirection * m_fAcceleration);
		}

		if (!m_bIsDead && Input.GetButtonDown("Attack_" + m_iCharacterID))
			m_pRigidBody.velocity = tDirection * m_fAttackImpulseForce;
	}

	private void UpdateAnimator()
	{
		bool bMoving = m_pRigidBody.velocity.sqrMagnitude > 0.1f;
		m_pAnimator.SetBool("Moving", bMoving);
		m_pSpriteRenderer.flipX = bMoving && m_pRigidBody.velocity.x < 0.0f;
	}

	private void OnTriggerEnter(Collider pCollider)
	{
		if (pCollider.tag == "Death")
			Die();
	}

	private void OnCollisionEnter(Collision pCollision)
	{
		if (m_bIsDead)
		{
			m_bIsDead = false;

			if (pCollision.gameObject.tag == "Character")
			{
				Vector3 tDirection = (pCollision.transform.position.x0z() - transform.position.x0z()).normalized;
				pCollision.rigidbody.AddForce(tDirection * m_fRespawnImpactImpulseForce, ForceMode.Impulse);
			}
		}
	}

	private void Die()
	{
		GameManager.Instance.RespawnMe(this);
		m_bIsDead = true;
	}
}
