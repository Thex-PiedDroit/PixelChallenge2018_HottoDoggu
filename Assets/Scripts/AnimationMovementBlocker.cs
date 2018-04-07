
using UnityEngine;

public class AnimationMovementBlocker : MonoBehaviour
{
#region Variables (public)

	public Character m_pMaster = null;

	#endregion

#region Variables (private)



	#endregion


	public void AllowMovement()
	{
		m_pMaster.IsActive = true;
	}
}
