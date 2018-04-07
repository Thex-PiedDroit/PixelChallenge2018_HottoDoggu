
using UnityEngine;

public class MainMenu : MonoBehaviour
{
#region Variables (public)

	static public MainMenu Instance = null;

	#endregion

#region Variables (private)



	#endregion


	private void Awake()
	{
		Toolkit.InitSingleton(this, ref Instance);
	}

	private void Update()
	{
		if (Input.GetButtonDown("Submit"))
		{
			GameManager.Instance.LaunchGame();
			gameObject.SetActive(false);
		}
	}
}
