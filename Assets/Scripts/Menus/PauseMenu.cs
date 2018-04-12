
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
#region Variables (public)

	public GameObject m_pResumeSelect = null;
	public GameObject m_pBackToMenuSelect = null;
	public GameObject m_pControlsSelect = null;
	public GameObject m_pQuitSelect = null;

	public GameObject m_pControlsScreen = null;

	#endregion

#region Variables (private)

	private int m_iCurrentSelection = 0;

	private bool m_bVerticalWasPressed = false;

	#endregion


	private void OnEnable()
	{
		SetSelection(0);
		Time.timeScale = 0.0f;

		Character.GameIsPaused = true;

		m_bVerticalWasPressed = Input.GetButton("MenuVertical");
	}

	private void OnDisable()
	{
		Time.timeScale = 1.0f;
		Character.GameIsPaused = false;
	}

	private void Update()
	{
		if (!m_pControlsScreen.activeSelf)
			CatchPauseInputs();
		else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Escape"))
			m_pControlsScreen.SetActive(false);
	}

	private void CatchPauseInputs()
	{
		float fVertical = Input.GetAxisRaw("MenuVertical");

		if (fVertical != 0.0f)
		{
			if (!m_bVerticalWasPressed)
			{
				m_bVerticalWasPressed = true;

				int iSelection = m_iCurrentSelection + (fVertical > 0.0f ? 1 : -1);
				iSelection %= 4;
				if (iSelection < 0)
					iSelection = 3;

				SetSelection(iSelection);
			}
		}
		else
		{
			m_bVerticalWasPressed = false;
		}

		if (Input.GetButtonDown("Escape") || Input.GetButtonDown("Cancel"))
		{
			gameObject.SetActive(false);
		}
		else if (Input.GetButtonDown("Submit"))
		{
			switch (m_iCurrentSelection)
			{
				case 0:
					gameObject.SetActive(false);
					break;
				case 1:
					GameManager.Instance.AbortGame();
					MainMenu.Instance.gameObject.SetActive(true);
					gameObject.SetActive(false);
					break;
				case 2:
					m_pControlsScreen.SetActive(true);
					break;
				case 3:
					Application.Quit();
					break;
			}
		}
	}

	private void SetSelection(int iSelection)
	{
		m_iCurrentSelection = iSelection;

		m_pResumeSelect.SetActive(m_iCurrentSelection == 0);
		m_pBackToMenuSelect.SetActive(m_iCurrentSelection == 1);
		m_pControlsSelect.SetActive(m_iCurrentSelection == 2);
		m_pQuitSelect.SetActive(m_iCurrentSelection == 3);
	}
}
