
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
#region Variables (public)

	public bool m_bLockYRotation = false;
	public float m_fMaxXRotation = 0.0f;

	#endregion

#region Variables (private)

    static Camera s_pCamera = null;

    #endregion


    private void Awake()
    {
        if (s_pCamera == null)
            s_pCamera = Camera.main;
    }

    private void LateUpdate()
	{
		Vector3 tSpriteForward = transform.position - s_pCamera.transform.position;

		float fPreviousRotationZ = transform.localEulerAngles.z;
		transform.forward = tSpriteForward.normalized;

		Vector3 tNewEuleur = transform.localEulerAngles;
		if (m_fMaxXRotation != 0.0f)
			tNewEuleur.x = Mathf.Clamp(tNewEuleur.x, 0.0f, m_fMaxXRotation);
		tNewEuleur.z = fPreviousRotationZ;
		transform.localEulerAngles = tNewEuleur;

		if (m_bLockYRotation)
		{
			Vector3 tWorldRotation = transform.eulerAngles;
			tWorldRotation.y = s_pCamera.transform.localEulerAngles.y;
			transform.eulerAngles = tWorldRotation;
		}
	}
}
