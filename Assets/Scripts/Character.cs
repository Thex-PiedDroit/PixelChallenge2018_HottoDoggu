﻿
using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
#region Variables (public)

	public Transform m_pEnemy = null;

	public Rigidbody m_pRigidBody = null;
	public Animator m_pAnimator = null;
	public SpriteRenderer m_pSpriteRenderer = null;

	public float m_fAcceleration = 10.0f;
	public float m_fMaxSpeed = 5.0f;

	public float m_fDashSpeed = 15.0f;
	public float m_fDashPower = 12.0f;
	public float m_fDashDuration = 0.5f;
	public float m_fAimAssistAngle = 30.0f;

	public float m_fRespawnImpactImpulseForce = 15.0f;

	public int m_iCharacterID = 0;

	#endregion

#region Variables (private)

	private bool m_bCanMove = true;
	private bool m_bIsDead = false;

	#endregion


	private void Update()
	{
		CatchInputs();
		UpdateAnimator();
	}

	private void CatchInputs()
	{
		if (!m_bCanMove || transform.position.y < -0.05f)
			return;

		float fHorizontal = Input.GetAxis("Horizontal_" + m_iCharacterID);
		float fVertical= Input.GetAxis("Vertical_" + m_iCharacterID);

		Vector3 tDirection = new Vector3(fHorizontal, 0.0f, fVertical).normalized;

		if (fHorizontal != 0.0f || fVertical != 0.0f)
		{
			if (m_bIsDead)
				tDirection *= 0.2f;

			if (m_pRigidBody.velocity.x0z().sqrMagnitude < m_fMaxSpeed.Sqrd() || Vector3.Dot(tDirection, m_pRigidBody.velocity) < 0.0f)
				m_pRigidBody.AddForce(tDirection * m_fAcceleration);
			else
				m_pRigidBody.AddForce((tDirection * m_fAcceleration) - m_pRigidBody.velocity);
		}

		if (!m_bIsDead && Input.GetButtonDown("Attack_" + m_iCharacterID))
			Attack(tDirection);
	}

	private void Attack(Vector3 tDirection)
	{
		Vector3 tMeToTargetDirection = (m_pEnemy.position - transform.position).normalized;
		if (Vector3.Angle(tDirection, tMeToTargetDirection) <= m_fAimAssistAngle * 0.5f)
			tDirection = tMeToTargetDirection;

		StartCoroutine(Dash(tDirection));
	}

	private IEnumerator Dash(Vector3 tDashDirection)
	{
		m_bCanMove = false;

		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fDashDuration)
		{
			m_pRigidBody.velocity = tDashDirection * m_fDashSpeed;
			yield return false;
		}

		m_bCanMove = true;
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
		else if (pCollision.gameObject.tag == "Character" && !m_bCanMove)	// "Can't move" means it's dashing -- for now
		{
			Vector3 tDirection = (pCollision.transform.position.x0z() - transform.position.x0z()).normalized;
			pCollision.rigidbody.AddForce(tDirection * m_fDashPower, ForceMode.Impulse);

			StopAllCoroutines();
			m_bCanMove = true;
		}
	}

	private void Die()
	{
		StopAllCoroutines();
		m_bCanMove = true;

		GameManager.Instance.RespawnMe(this);
		m_bIsDead = true;
	}
}
