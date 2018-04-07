
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
#region Variables (public)

	static public GameManager Instance = null;

	public Vector3 m_tPlayer1SpawnPoint = Vector3.zero;
	public Vector3 m_tPlayer2SpawnPoint = Vector3.zero;

	public FollowSpot m_pPlayer1FollowSpot = null;
	public FollowSpot m_pPlayer2FollowSpot = null;

	public Transform m_pCharactersContainer = null;
	public Text m_pWinText = null;
	public MainMenu m_pMainMenu = null;
	public GameObject m_pPauseMenu = null;

	public Character m_pPlayer1Prefab = null;
	public Character m_pPlayer2Prefab = null;

	public float m_fGameDurationInSeconds = 120.0f;

	public Vector2 m_tRespawnRectanglePos = Vector2.zero;
	public Vector2 m_tRespawnRectangleExtent = Vector2.one;
	public float m_fRespawnHeight = 3.0f;

	[Space()]
	[Header("Debug")]
	public Color m_tPlayer1SpawnDebugColor = Color.white;
	public Color m_tPlayer2SpawnDebugColor = Color.white;

	[Space(5.0f)]
	public Color m_pRespawnRadiusDebugColor = Color.white;

	#endregion

#region Variables (private)

	private Character m_pPlayer1Instance = null;
	private Character m_pPlayer2Instance = null;

	private Dictionary<Character, CharacterStats> m_pCharactersStats = null;

	private bool m_bCanRestartGame = false;
	private bool m_bInGame = false;

	#endregion


	private void Awake()
	{
		if (!Toolkit.InitSingleton(this, ref Instance))
			return;

		m_pCharactersStats = new Dictionary<Character, CharacterStats>();

		if (MainMenu.Instance == null)
			m_pMainMenu.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (m_bCanRestartGame && Input.anyKey)
			LaunchGame();
		else if (m_bInGame && Input.GetButtonDown("Escape"))
			m_pPauseMenu.SetActive(true);
	}

	public void LaunchGame()
	{
		m_bCanRestartGame = false;
		m_pWinText.gameObject.SetActive(false);

		if (m_pPlayer1Instance == null)
		{
			m_pCharactersStats.Clear();

			m_pPlayer1Instance = Instantiate(m_pPlayer1Prefab, m_tPlayer1SpawnPoint, Quaternion.Euler(Vector3.right), m_pCharactersContainer);
			m_pPlayer2Instance = Instantiate(m_pPlayer2Prefab, m_tPlayer2SpawnPoint, Quaternion.Euler(-Vector3.right), m_pCharactersContainer);

			m_pPlayer1Instance.m_pEnemy = m_pPlayer2Instance.transform;
			m_pPlayer2Instance.m_pEnemy = m_pPlayer1Instance.transform;

			m_pCharactersStats.Add(m_pPlayer1Instance, new CharacterStats());
			m_pCharactersStats.Add(m_pPlayer2Instance, new CharacterStats());

			m_pPlayer1FollowSpot.m_pTarget = m_pPlayer1Instance.transform;
			m_pPlayer2FollowSpot.m_pTarget = m_pPlayer2Instance.transform;
		}
		else
		{
			m_pPlayer1Instance.transform.position = m_tPlayer1SpawnPoint;
			m_pCharactersStats[m_pPlayer1Instance].Reset();
			m_pPlayer1Instance.gameObject.SetActive(true);

			m_pPlayer2Instance.transform.position = m_tPlayer2SpawnPoint;
			m_pCharactersStats[m_pPlayer2Instance].Reset();
			m_pPlayer2Instance.gameObject.SetActive(true);
		}

		m_bInGame = true;
		StartCoroutine(ElapseGame());
	}

	private IEnumerator ElapseGame()
	{
		float m_fGameStartTime = Time.time;
		while (Time.time - m_fGameStartTime < m_fGameDurationInSeconds)
			yield return false;

		FinishGame();
	}

	public void RespawnMe(Character pMe)
	{
		++m_pCharactersStats[pMe].m_iDeaths;

		pMe.m_pRigidBody.velocity = Vector3.zero;

		float fRandX = Random.Range(-m_tRespawnRectangleExtent.x * 0.5f, m_tRespawnRectangleExtent.x * 0.5f);
		float fRandZ = Random.Range(-m_tRespawnRectangleExtent.y * 0.5f, m_tRespawnRectangleExtent.y * 0.5f);

		Vector3 tRespawnPos = new Vector3(fRandX, m_fRespawnHeight, fRandZ) + m_tRespawnRectanglePos.xzy();
		pMe.transform.position = tRespawnPos;
	}

	private void FinishGame()
	{
		m_bInGame = false;

		int iP1Deaths = m_pCharactersStats[m_pPlayer1Instance].m_iDeaths;
		int iP2Deaths = m_pCharactersStats[m_pPlayer2Instance].m_iDeaths;

		if (iP1Deaths < iP2Deaths)
		{
			m_pWinText.text = "Player 1 Wins!";
		}
		else if (iP1Deaths == iP2Deaths)
		{
			m_pWinText.text = "Draw!";
		}
		else
		{
			m_pWinText.text = "Player 2 Wins!";
		}

		m_pWinText.text += "\n" + iP1Deaths + " to " + iP2Deaths + "!\n\nPress any key to play again.";

		m_pWinText.gameObject.SetActive(true);

		m_pPlayer1Instance.gameObject.SetActive(false);
		m_pPlayer2Instance.gameObject.SetActive(false);

		StartCoroutine(SecurityBeforeCanRestartGame());
	}

	private IEnumerator SecurityBeforeCanRestartGame()
	{
		float fStartTime = Time.time;
		while (Time.time - fStartTime < 0.5f)
			yield return false;

		m_bCanRestartGame = true;
	}

	public void AbortGame()
	{
		m_bInGame = false;

		StopAllCoroutines();
		m_pPlayer1Instance.gameObject.SetActive(false);
		m_pPlayer2Instance.gameObject.SetActive(false);
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = m_tPlayer1SpawnDebugColor;
		Gizmos.DrawCube(m_tPlayer1SpawnPoint + (Vector3.up * 0.25f), Vector3.one * 0.5f);

		Gizmos.color = m_tPlayer2SpawnDebugColor;
		Gizmos.DrawCube(m_tPlayer2SpawnPoint + (Vector3.up * 0.25f), Vector3.one * 0.5f);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = m_pRespawnRadiusDebugColor;
		Gizmos.DrawWireCube(m_tRespawnRectanglePos.xzy(), m_tRespawnRectangleExtent.xzy().SetY(0.01f));
	}
}

public class CharacterStats
{
	public int m_iDeaths = 0;

	public void Reset()
	{
		m_iDeaths = 0;
	}
}
