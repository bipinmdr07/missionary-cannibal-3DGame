using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
	// when game is over

	// 0th child - autoplay button
	// 1st child - GameOver Panel
	// 2nd child - Congratulation Panel
	// 3rd child - Autoplay Panel
	public GameObject canvas;

	public void ReloadGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
	}

	public void Menu(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1, LoadSceneMode.Single);
	}

	public void ShowAutoPlayBtn(){
		transform.GetChild (0).gameObject.SetActive (true);
	}

	public void ShowGameOverMenu(){
		for (int i = 0; i < 4; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
		transform.GetChild (1).gameObject.SetActive (true);
	}

	public void ShowCongratulationMenu(){
		for (int i = 0; i < 4; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
		transform.GetChild (2).gameObject.SetActive (true);
	}

	public void ShowAutoPlayMenu(){
		for (int i = 0; i < 4; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
		transform.GetChild (3).gameObject.SetActive (true);
	}

	public void Hide(){
		transform.GetChild (3).gameObject.SetActive (false);
		ShowAutoPlayBtn ();
	}
}
