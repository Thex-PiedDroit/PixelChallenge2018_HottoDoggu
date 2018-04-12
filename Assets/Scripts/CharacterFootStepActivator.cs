
using UnityEngine;

public class CharacterFootStepActivator : MonoBehaviour
{
#region Variables (public)

	public Character m_pMaster = null;
	public AudioSource m_pAudioSource = null;

	#endregion

#region Variables (private)

	private string m_sName = null;

	#endregion


	private void Start()
	{
		m_sName = m_pMaster.CharacterName;
	}

	public void LaunchFootStepSound()
	{
		if (Physics.Raycast(m_pMaster.transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Floor")))
			AudioManager.Instance.PlaySound("FootSteps_" + m_sName, m_pAudioSource, false, true, 0.1f);
	}
}
