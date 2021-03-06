﻿
using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
#region Variables (public)

	public Character m_pEnemy = null;

	public TauntIcon m_pTauntIconPrefab = null;
	public Transform m_pTauntFollowPoint = null;

	public Rigidbody m_pRigidBody = null;
	public Animator m_pAnimator = null;
	public SpriteRenderer m_pSpriteRenderer = null;

	public SpriteRenderer m_pBlockFlash = null;

	public AudioSource m_pAudioSource = null;
	public AudioSource m_pTauntsAudioSource = null;

	public float m_fAcceleration = 10.0f;
	public float m_fMaxSpeed = 5.0f;

	public float m_fDashSpeed = 15.0f;
	public float m_fDashPower = 12.0f;
	public float m_fDashDuration = 0.5f;
	public float m_fDashCD = 1.5f;
	public float m_fAimAssistAngle = 30.0f;

	public float m_fBlockDuration = 1.0f;
	public float m_fTimedBlockWindow = 0.2f;
	public float m_fBlockClampedSpeed = 3.0f;
	public float m_fBlockStunDuration = 1.0f;
	public float m_fBlockRetaliationPower = 20.0f;

	public float m_fRespawnImpactImpulseForce = 15.0f;

	public int m_iCharacterID = 0;

	#endregion

	#region Variables (private)

	private TauntIcon m_pTauntIcon = null;

	private string m_sName = null;
	public string CharacterName
	{
		get { return m_sName; }
	}

	static private bool s_bGameIsPaused = false;
	static public bool GameIsPaused
	{
		set { s_bGameIsPaused = value; }
	}

	private bool m_bActive = true;
	public bool IsActive
	{
		set { m_bActive = value; }
		get { return m_bActive; }
	}

	private bool m_bBlocking = false;
	private bool m_bTimedBlock = false;

	private bool m_bIsStunned = false;
	private bool m_bCanMove = true;
	private bool m_bCanDash = true;

	private bool m_bIsDead = false;

	#endregion


	private void Awake()
	{
		m_sName = m_iCharacterID == 0 ? "F" : "M";
	}

	private void Update()
	{
		if (!m_bIsStunned && m_bActive && !s_bGameIsPaused)
			CatchInputs();
		UpdateAnimator();
	}

	private void CatchInputs()
	{
		if (!m_bCanMove || transform.position.y < -0.1f)
			return;

		float fHorizontal = Input.GetAxis("Horizontal_" + m_iCharacterID);
		if (fHorizontal.Sqrd() <= 0.15f.Sqrd())
			fHorizontal = 0.0f;
		float fVertical = Input.GetAxis("Vertical_" + m_iCharacterID);
		if (fVertical.Sqrd() <= 0.15f.Sqrd())
			fVertical = 0.0f;

		Vector3 tDirection = new Vector3(fHorizontal, 0.0f, fVertical).normalized;

		if (fHorizontal != 0.0f || fVertical != 0.0f)
		{
			if (!m_bBlocking)
			{
				if (m_bIsDead)
					tDirection *= 0.2f;

				if (m_pRigidBody.velocity.x0z().sqrMagnitude < m_fMaxSpeed.Sqrd() || Vector3.Dot(tDirection, m_pRigidBody.velocity) < 0.0f)
					m_pRigidBody.AddForce(tDirection * m_fAcceleration);
				else
					m_pRigidBody.AddForce((tDirection * m_fAcceleration) - m_pRigidBody.velocity);
			}
			else
			{
				transform.forward = tDirection;
			}
		}

		if (!m_bIsDead && m_bCanDash && !m_bBlocking && Input.GetButtonDown("Attack_" + m_iCharacterID))
			Attack(tDirection);

		if (!m_bIsDead && Input.GetButtonDown("Block_" + m_iCharacterID))
			Block(tDirection);

		if (Input.GetButtonDown("Taunt_" + m_iCharacterID))
			Taunt();
	}

	private void Attack(Vector3 tDirection)
	{
		Vector3 tMeToTargetDirection = (m_pEnemy.transform.position - transform.position).normalized;
		if (Vector3.Angle(tDirection, tMeToTargetDirection) <= m_fAimAssistAngle * 0.5f)
			tDirection = tMeToTargetDirection;

		StartCoroutine(Dash(tDirection));
	}

	private IEnumerator Dash(Vector3 tDashDirection)
	{
		AudioManager.Instance.PlaySound("Spin", m_pAudioSource);

		m_bCanMove = false;
		m_bCanDash = false;

		m_pAnimator.ResetTrigger("Spin");
		m_pAnimator.SetTrigger("Spin");

		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fDashDuration)
		{
			m_pRigidBody.velocity = tDashDirection * m_fDashSpeed;
			yield return false;
		}

		m_bCanMove = true;

		fStartTime = Time.time;
		while (Time.time - fStartTime < m_fDashCD - m_fDashDuration)
			yield return false;

		m_bCanDash = true;
	}

	private void Block(Vector3 tDirection)
	{
		if (tDirection != Vector3.zero)
			transform.forward = tDirection.normalized;

		StartCoroutine(ElapseBlock());
	}

	private IEnumerator ElapseBlock()
	{
		AudioManager.Instance.PlaySound("Impact" + m_sName, m_pAudioSource);

		m_bBlocking = true;
		m_bTimedBlock = true;

		if (m_pRigidBody.velocity.sqrMagnitude > m_fBlockClampedSpeed)
			m_pRigidBody.velocity = m_pRigidBody.velocity.normalized * m_fBlockClampedSpeed;

		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fTimedBlockWindow)
			yield return false;

		m_bTimedBlock = false;

		while (Time.time - fStartTime < m_fBlockDuration - m_fTimedBlockWindow && Input.GetButton("Block_" + m_iCharacterID))
			yield return false;

		m_bBlocking = false;
	}

	public void StunAndResetConditions()
	{
		ResetConditions();
		Stun();
	}

	private void Stun()
	{
		StopAllCoroutines();
		ResetConditions();
		StartCoroutine(ElapseStun());
	}

	private IEnumerator ElapseStun()
	{
		m_bIsStunned = true;

		float fStartTime = Time.time;
		while (Time.time - fStartTime < m_fBlockStunDuration)
			yield return false;

		m_bIsStunned = false;
	}

	private void UpdateAnimator()
	{
		bool bMoving = m_pRigidBody.velocity.sqrMagnitude > 0.1f;
		m_pAnimator.SetBool("Moving", bMoving);
		m_pAnimator.SetBool("Block", m_bBlocking);
		m_pAnimator.SetBool("Stun", m_bIsStunned);

		if (!m_bBlocking)
			m_pSpriteRenderer.flipX = m_pRigidBody.velocity.x < 0.0f;
	}

	private void OnTriggerEnter(Collider pCollider)
	{
		if (pCollider.tag == "Death")
			Die();
	}

	private void OnCollisionEnter(Collision pCollision)
	{
		if (pCollision.gameObject.tag == "Character")
			AudioManager.Instance.PlaySound("Impact", m_pAudioSource);

		if (m_bIsDead)
		{
			m_bIsDead = false;

			if (pCollision.gameObject.tag == "Character")
			{
				Vector3 tDirection = (pCollision.transform.position.x0z() - transform.position.x0z()).normalized;
				pCollision.rigidbody.AddForce(tDirection * m_fRespawnImpactImpulseForce, ForceMode.Impulse);
			}
		}
		else if (pCollision.gameObject.tag == "Character")
		{
			if (!m_bCanMove)	// Means it's dashing -- for now
			{
				Vector3 tDirection = (pCollision.transform.position.x0z() - transform.position.x0z()).normalized;
				float fPower = m_fDashPower;
				if (m_pEnemy.m_bIsStunned)
				{
					fPower = m_fBlockRetaliationPower;
					m_pEnemy.Stun();
				}
				pCollision.rigidbody.AddForce(tDirection * fPower, ForceMode.Impulse);

				StopAllCoroutines();
				ResetConditions();
			}

			if (m_bBlocking)
			{
				if (m_bTimedBlock)
				{
					m_pBlockFlash.flipX = m_pRigidBody.velocity.x > 0.0f;
					m_pBlockFlash.gameObject.SetActive(true);

					StopAllCoroutines();
					ResetConditions();
					pCollision.rigidbody.velocity = Vector3.zero;
					pCollision.gameObject.GetComponent<Character>().Stun();

					m_pRigidBody.velocity = Vector3.zero;
					m_pRigidBody.Sleep();
				}
				else
				{

				}
			}
		}
	}

	private void ResetConditions()
	{
		m_bCanMove = true;
		m_bCanDash = true;
		m_bIsStunned = false;
		m_bBlocking = false;
		m_bTimedBlock = false;
	}

	private void Die()
	{
		AudioManager.Instance.PlaySound("Death_" + m_sName, m_pAudioSource);

		StopAllCoroutines();
		ResetConditions();
		m_bActive = true;

		GameManager.Instance.RespawnMe(this);
		m_bIsDead = true;
	}

	private void Taunt()
	{
		m_bActive = false;

		m_pAnimator.SetTrigger("Taunt");
		AudioManager.Instance.PlaySound("Taunt_" + m_sName, m_pTauntsAudioSource);

		if (m_pTauntIcon == null)
			m_pTauntIcon = Instantiate(m_pTauntIconPrefab, GameManager.Instance.m_pTauntIconsContainer);

		m_pTauntIcon.Init(m_pTauntFollowPoint);
	}
}
