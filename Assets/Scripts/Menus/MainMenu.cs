﻿
using UnityEngine;

public class MainMenu : MonoBehaviour
{
#region Variables (public)

	static public MainMenu Instance = null;

	public GameObject m_pLaunchGameSelect = null;
	public GameObject m_pControlsSelect = null;
	public GameObject m_pQuitSelect = null;

	public GameObject m_pControlsScreen = null;

	#endregion

	#region Variables (private)

	private int m_iCurrentSelection = 0;

	private bool m_bVerticalWasPressed = false;

	#endregion


	private void Awake()
	{
		Toolkit.InitSingleton(this, ref Instance);
	}

	private void OnEnable()
	{
		SetSelection(0);
	}

	private void Update()
	{
		if (!m_pControlsScreen.activeSelf)
		{
			if (Input.GetButtonDown("Submit"))
			{
				switch (m_iCurrentSelection)
				{
					case 0:
						GameManager.Instance.LaunchGame();
						gameObject.SetActive(false);
						break;
					case 1:
						m_pControlsScreen.SetActive(true);
						break;
					case 2:
						Application.Quit();
						break;
				}
			}
			else
			{
				HandleVerticalInput();
			}
		}
		else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Escape"))
		{
			m_pControlsScreen.SetActive(false);
		}
	}

	private void HandleVerticalInput()
	{
		float fVertical = Input.GetAxisRaw("MenuVertical");

		if (fVertical != 0.0f)
		{
			if (!m_bVerticalWasPressed)
			{
				m_bVerticalWasPressed = true;

				int iSelection = m_iCurrentSelection + (fVertical > 0.0f ? 1 : -1);
				iSelection %= 3;
				if (iSelection < 0)
					iSelection = 2;

				SetSelection(iSelection);
			}
		}
		else
		{
			m_bVerticalWasPressed = false;
		}
	}

	private void SetSelection(int iSelection)
	{
		m_iCurrentSelection = iSelection;

		m_pLaunchGameSelect.SetActive(m_iCurrentSelection == 0);
		m_pControlsSelect.SetActive(m_iCurrentSelection == 1);
		m_pQuitSelect.SetActive(m_iCurrentSelection == 2);
	}
}
