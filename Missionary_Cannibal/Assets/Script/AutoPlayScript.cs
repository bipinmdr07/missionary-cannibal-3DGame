using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoPlayScript : MonoBehaviour {
	public GameObject gamecontroller;
	private GameController gameControllerScript;
	private bool auto_play_enabled = false;
	private bool auto_btn_clicked = false;
	private StateScript sscript;

	public Stack<StateScript> stateStack = new Stack<StateScript>();
	public Stack<StateScript> buffer = new Stack<StateScript>();
	public Stack<StateScript> answerStack = new Stack<StateScript> ();

	// Use this for initialization
	void Start () {
		gameControllerScript = gamecontroller.GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (auto_play_enabled) {
			
		}
	}

	public void ReadyToGoAuto(){
		if (auto_btn_clicked == false) {
			auto_btn_clicked = true;
			Transform leftBank = gameControllerScript.charPosition.GetChild (0);
			for (int i = 0; i < 6; i++) {
				if (leftBank.GetChild (i).childCount == 0) {
					auto_play_enabled = false;
					auto_btn_clicked = false;
					Debug.Log ("Game Restart required");
					Debug.Log ("Reloading scene");
					ReloadCurrentScene ();
					return;
				}
			}
			AutoSolve ();
		}
	}

	public void ReloadCurrentScene(){
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene, LoadSceneMode.Single);
	}

	// initial state [3.3] [0.0] left_bank
	private void AutoSolve(){
		StateScript startState = new StateScript ();
		startState.SetMissionaryCannibalCountInLeft (3, 3);
		startState.SetMissionaryCannibalCountInRight (0, 0);
		startState.boat_position = StateScript.boat_pos.left_bank.ToString();

		StateScript finalState = new StateScript ();
		finalState.SetMissionaryCannibalCountInLeft (0, 0);
		finalState.SetMissionaryCannibalCountInRight (3, 3);
		finalState.boat_position = StateScript.boat_pos.right_bank.ToString();

		HelperFunction (startState, finalState);

		foreach (StateScript s in stateStack) {
			answerStack.Push(s);
		}

		foreach (StateScript ans in answerStack) {
			Debug.Log (ans.cannibal_count_left.ToString () + ", " + ans.missionary_count_left + "," + ans.boat_position);
		}

		auto_play_enabled = true;
		StartCoroutine ("GoToNextState");
	}

	private bool CheckForValidState(StateScript ss){
		// return true if it is a valid state else return false
		// in m_count >= c_count in both bank then it's a valid
		// m_count < c_count is also valid iff m_count == 0
		if (ss.cannibal_count_left > ss.missionary_count_left && ss.missionary_count_left != 0) {
			return false;
		}
		if (ss.cannibal_count_right > ss.missionary_count_right && ss.missionary_count_right != 0) {
			return false;
		}
		return true;
	}

	// this function is responsible for selecting appropriate state using DFS algorithm
	private void HelperFunction(StateScript s, StateScript final){
		StateScript currentState = s;
		StateScript finalState = final;
		stateStack.Push (currentState);

//		Debug.Log (currentState == finalState);

		StateScript tempState = null;

		while (true) {
			currentState = stateStack.Peek ();

			if (currentState.cannibal_count_left == finalState.cannibal_count_left && currentState.missionary_count_left == finalState.missionary_count_left && currentState.cannibal_count_right == finalState.cannibal_count_right && currentState.missionary_count_right == finalState.missionary_count_right && currentState.boat_position == finalState.boat_position) {
				break;
			}

			if (!CheckForValueInBuffer (currentState)) {
				buffer.Push (currentState);
			}
			// if boat is in the left bank then next state will result the boat in right bank
			if (currentState.boat_position == StateScript.boat_pos.left_bank.ToString()){
				bool found = false;
				for (int missionary = 0; missionary <= currentState.missionary_count_left; missionary++) {
					for (int cannibal = 0; cannibal <= currentState.cannibal_count_left; cannibal++) {
						if ((missionary + cannibal) == 0 || (missionary + cannibal) > 2)
							continue;

						tempState = new StateScript (currentState.missionary_count_left - missionary, currentState.cannibal_count_left - cannibal, currentState.missionary_count_right + missionary, currentState.cannibal_count_right + cannibal, StateScript.boat_pos.right_bank.ToString());
						if (CheckForValidState (tempState) && !CheckForValueInBuffer(tempState)) {
							stateStack.Push (tempState);
							found = true;
							break;
						}
					}
					if (found) {
						break;
					}
				}

				if (!found) {
					stateStack.Pop ();
				}
			}
				
			// if boat is in the right bank then next state will result the boat in left bank
			else {
				bool found = false;
				for (int missionary = 0; missionary <= currentState.missionary_count_right; missionary++) {
					for (int cannibal = 0; cannibal <= currentState.cannibal_count_right; cannibal++) {
						if ((missionary + cannibal) == 0 || (missionary + cannibal) > 2)
							continue;

						tempState = new StateScript (currentState.missionary_count_left + missionary, currentState.cannibal_count_left + cannibal, currentState.missionary_count_right - missionary, currentState.cannibal_count_right - cannibal, StateScript.boat_pos.left_bank.ToString());

						if (CheckForValidState (tempState) && !CheckForValueInBuffer(tempState)) {
							stateStack.Push (tempState);
							found = true;
							break;
						}
					}
					if (found) {
						break;
					}
				}
				if (!found) {
					StateScript popped = stateStack.Pop ();
				}
			}
		}
	}

	private bool CheckForValueInBuffer(StateScript ss){
		foreach (StateScript temp in this.buffer) {
			if (temp.cannibal_count_left == ss.cannibal_count_left && temp.cannibal_count_right == ss.cannibal_count_right && temp.missionary_count_left == ss.missionary_count_left && temp.missionary_count_right == ss.missionary_count_right && temp.boat_position == ss.boat_position) {
				return true;
			}
		}
		return false;
	}

	IEnumerator GoToNextState(){
		StateScript thisState = null;
		StateScript thatState = null;

		Transform pos = gameControllerScript.charPosition;

		while (answerStack.Count > 1) {
			thisState = answerStack.Pop ();
			thatState = answerStack.Peek ();

			int miss = 0;
			int cann = 0;
			if (thisState.boat_position == StateScript.boat_pos.left_bank.ToString ()) {
				miss = thisState.missionary_count_left - thatState.missionary_count_left;
				cann = thisState.cannibal_count_left - thatState.cannibal_count_left;


				// missionary
				for (int i = 0; i < 3; i++) {
					if (pos.GetChild (0).GetChild (i).childCount != 0 && miss != 0) {
						miss -= 1;
						gameControllerScript.MouseDownFromMissionary (pos.GetChild (0).GetChild (i).GetChild (0));
					}
				}

				// cannibal
				for (int i = 3; i < 6; i++) {
					if (pos.GetChild (0).GetChild (i).childCount != 0 && cann != 0) {
						cann -= 1;
						gameControllerScript.MouseDownFromCannibal (pos.GetChild (0).GetChild (i).GetChild (0));
					}
				}
			} else {
				miss = thisState.missionary_count_right - thatState.missionary_count_right;
				cann = thisState.cannibal_count_right - thatState.cannibal_count_right;


				// missionary
				for (int i = 0; i < 3; i++) {
					if (pos.GetChild (2).GetChild(i).childCount != 0 && miss != 0) {
						miss -= 1;
						gameControllerScript.MouseDownFromMissionary (pos.GetChild (2).GetChild (i).GetChild (0));
					}
				}

				// cannibal
				for (int i = 3; i < 6; i++) {
					if (pos.GetChild (2).GetChild (i).childCount != 0 && cann != 0) {
						cann -= 1;
						gameControllerScript.MouseDownFromCannibal (pos.GetChild (2).GetChild (i).GetChild (0));
					}
				}
			}

			gameControllerScript.boatScript.MoveTheBoat ();
			yield return new WaitForSeconds (3.5f);

			for (int i = 0; i < 2; i++) {
				if (pos.GetChild(1).GetChild(i).childCount != 0){
					if (pos.GetChild (1).GetChild (i).GetChild (0).tag == "cannibal") {
						gameControllerScript.MouseDownFromCannibal (pos.GetChild (1).GetChild (i).GetChild (0));
						Debug.Log ("cannibal");
					}

					else if (pos.GetChild (1).GetChild (i).GetChild (0).tag == "missionary") {
						gameControllerScript.MouseDownFromMissionary (pos.GetChild (1).GetChild (i).GetChild (0));
						Debug.Log ("missionary");
					}
				}
			}
			yield return new WaitForSeconds (0.75f);
		}


	}
}
