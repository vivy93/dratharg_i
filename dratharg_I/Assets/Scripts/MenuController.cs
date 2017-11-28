using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	int count = 0;

	///////////////////////////////////////////////////
	//				Button functions				 //
	///////////////////////////////////////////////////

	public void LoadScene(string scenesName)
	{
		SceneManager.LoadScene(scenesName);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void TurnOffOnMusic()
	{
		count += 1;
		if (count % 2 == 1) {			
			AudioListener.pause = true;
		} else {
			AudioListener.pause = false;
		}

	}

}
