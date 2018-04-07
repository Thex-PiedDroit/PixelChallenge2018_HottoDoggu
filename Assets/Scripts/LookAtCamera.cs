
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
#region Variables (public)

	public float m_fMaxXRotation = 20.0f;
	public bool m_bLockYRotation = true;
	public bool m_bLocalUpIsAlwaysUp = false;

	#endregion

#region Variables (private)

	private Camera m_pCamera = null;

	#endregion


	private void Awake()
	{
		m_pCamera = Camera.main;
	}

	private void LateUpdate()
	{
		Vector3 tSpriteForward = transform.position - m_pCamera.transform.position;

		if (m_bLocalUpIsAlwaysUp)
			transform.up = Vector3.up;

		float fPreviousRotationZ = transform.eulerAngles.z;
		transform.forward = tSpriteForward.normalized;

		Vector3 tNewEuleur = transform.eulerAngles;
		if (m_fMaxXRotation != 0.0f)
			tNewEuleur.x = Mathf.Clamp(tNewEuleur.x, 0.0f, m_fMaxXRotation);
		tNewEuleur.z = fPreviousRotationZ;
		transform.eulerAngles = tNewEuleur;

		if (m_bLockYRotation)
		{
			Vector3 tWorldRotation = transform.eulerAngles;
			tWorldRotation.y = m_pCamera.transform.localEulerAngles.y;
			transform.eulerAngles = tWorldRotation;
		}
	}
}
