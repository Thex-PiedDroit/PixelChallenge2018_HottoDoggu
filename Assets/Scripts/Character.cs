
using UnityEngine;

public class Character : MonoBehaviour
{
#region Variables (public)

	public Rigidbody m_pRigidBody = null;

	public float m_fAcceleration = 10.0f;
	public float m_fMaxSpeed = 5.0f;

	public float m_fAttackImpulseForce = 10.0f;

	public int m_iCharacterID = 0;

	#endregion

#region Variables (private)



	#endregion


	private void Update()
	{
		CatchInputs();
	}

	private void CatchInputs()
	{
		float fHorizontal = Input.GetAxis("Horizontal_" + m_iCharacterID);
		float fVertical= Input.GetAxis("Vertical_" + m_iCharacterID);

		Vector3 tDirection = new Vector3(fHorizontal, 0.0f, fVertical);

		if ((fHorizontal != 0.0f || fVertical != 0.0f) && m_pRigidBody.velocity.sqrMagnitude < m_fMaxSpeed.Sqrd())
		{
			m_pRigidBody.AddForce(tDirection * m_fAcceleration);
		}

		if (Input.GetButtonDown("Attack_" + m_iCharacterID))
		{
			m_pRigidBody.velocity = Vector3.zero;
			m_pRigidBody.AddForce(tDirection * m_fAttackImpulseForce, ForceMode.Impulse);
		}
	}
}
