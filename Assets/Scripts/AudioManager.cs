
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
#region Variables (public)

	static public AudioManager Instance = null;

	public AudioSource m_pMusicSource = null;
	public AudioSource m_pAmbienceSource = null;
	public AudioSource m_pSoundsSource = null;

	public List<KeyedSound> m_pAllSounds = null;

	#endregion

#region Variables (private)

	private Dictionary<string, List<AudioClip>> m_pSoundsDictionary = null;

	#endregion


	private void Awake()
	{
		if (!Toolkit.InitSingleton(this, ref Instance))
			return;

		m_pSoundsDictionary = new Dictionary<string, List<AudioClip>>(m_pAllSounds.Count);
		for (int i = 0; i < m_pAllSounds.Count; ++i)
			m_pSoundsDictionary.Add(m_pAllSounds[i].m_sSoundKey, m_pAllSounds[i].m_pClips.Clone());
	}

	public void PlaySound(string sSoundKey, AudioSource pSource, bool bLoop = false, bool bRandomizePitch = false, float fRandomRange = 0.3f)
	{
		if (!m_pSoundsDictionary.ContainsKey(sSoundKey))
			return;

		if (bRandomizePitch)
			pSource.pitch = 1.0f + Random.Range(-fRandomRange, fRandomRange);

		List<AudioClip> pAllSounds = m_pSoundsDictionary[sSoundKey];
		pSource.clip = pAllSounds[Random.Range(0, pAllSounds.Count)];

		pSource.loop = bLoop;
		pSource.Play();
	}
}

[System.Serializable]
public class KeyedSound
{
	public string m_sSoundKey = null;
	public List<AudioClip> m_pClips = null;
}
