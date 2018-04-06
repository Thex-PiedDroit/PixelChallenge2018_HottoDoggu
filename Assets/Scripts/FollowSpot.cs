
using UnityEngine;

public class FollowSpot : MonoBehaviour
{
#region Variables (public)

	public Transform m_pTarget = null;

	#endregion

#region Variables (private)



	#endregion


	private void LateUpdate()
	{
		if (m_pTarget != null)
			transform.forward = (m_pTarget.position - transform.position).normalized;
	}
}
