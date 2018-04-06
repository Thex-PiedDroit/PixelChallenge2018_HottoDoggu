
using UnityEngine;

public class Character : MonoBehaviour
{
#region Variables (public)

	public Rigidbody m_pRigidBody = null;

	public float m_fAcceleration = 1.0f;
	public float m_fMaxSpeed = 1.0f;

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

		if (fHorizontal != 0.0f || fVertical != 0.0f)
		{
			m_pRigidBody.AddForce(new Vector3(fHorizontal, 0.0f, fVertical) * m_fAcceleration);

			if (m_pRigidBody.velocity.sqrMagnitude > m_fMaxSpeed.Sqrd())
				m_pRigidBody.velocity = m_pRigidBody.velocity.normalized * m_fMaxSpeed;
		}
	}
}
