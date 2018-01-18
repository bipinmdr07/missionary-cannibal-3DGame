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
//		sscript = gameObject.GetComponent<StateScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (auto_play_enabled) {
			
		}
	}

	public void ReadyToGoAuto(){
		Debug.Log ("Click");
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

			auto_play_enabled = true;
			AutoSolve ();
		}
	}

	public void ReloadCurrentScene(){
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene);//, LoadSceneMode.Single);
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

		/*Debug.Log ("Buffer: " + CheckForValueInBuffer(startState).ToString());

		buffer.Push (startState);

		Debug.Log ("Buffer: " + CheckForValueInBuffer(startState).ToString());
*/
		foreach (StateScript s in stateStack) {
//			Debug.Log (s.cannibal_count_left.ToString() + ", " + s.missionary_count_left + "," + s.boat_position);
			answerStack.Push(s);
		}

		foreach (StateScript ans in answerStack) {
			Debug.Log (ans.cannibal_count_left.ToString () + ", " + ans.missionary_count_left + "," + ans.boat_position);
		}
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

		Debug.Log (currentState == finalState);

		StateScript tempState = null;

		while (true) {
			currentState = stateStack.Peek ();

			if (currentState.cannibal_count_left == finalState.cannibal_count_left && currentState.missionary_count_left == finalState.missionary_count_left && currentState.cannibal_count_right == finalState.cannibal_count_right && currentState.missionary_count_right == finalState.missionary_count_right && currentState.boat_position == finalState.boat_position) {
				break;
			}
//			Debug.Log ("Current State: " + currentState.cannibal_count_left.ToString() + ", " + currentState.missionary_count_left.ToString() + ", " + currentState.boat_position + "Stack Size: " + stateStack.Count.ToString());

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
//							Debug.Log ("From Left Bank: " + tempState.cannibal_count_left.ToString() + ", " + tempState.missionary_count_left.ToString() + ", " + tempState.boat_position);
							found = true;
							break;
						}
					}
					if (found) {
//						Debug.Log ("Found on left bank");
						break;
					}
//					Debug.Log ("Loop not broken");
				}

				if (!found) {
//					Debug.Log ("Not found on left bank");
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
//							Debug.Log ("From right bank: " + tempState.cannibal_count_left.ToString() + ", " + tempState.missionary_count_left.ToString() + ", " + tempState.boat_position);
							break;
						}
					}
					if (found) {
//						Debug.Log ("Found on right bank");
						break;
					}
				}
				if (!found) {
//					Debug.Log ("Not Found on right bank");
					StateScript popped = stateStack.Pop ();
//					Debug.Log ("Popped: " + popped.cannibal_count_left.ToString() + ", " + popped.missionary_count_left.ToString() + ", " + popped.boat_position + " Stack Size: " + stateStack.Count.ToString());

				}
			}
		}

		Debug.Log ("Finished");
	}

	private bool CheckForValueInBuffer(StateScript ss){
		foreach (StateScript temp in this.buffer) {
			if (temp.cannibal_count_left == ss.cannibal_count_left && temp.cannibal_count_right == ss.cannibal_count_right && temp.missionary_count_left == ss.missionary_count_left && temp.missionary_count_right == ss.missionary_count_right && temp.boat_position == ss.boat_position) {
				return true;
			}
		}
		return false;
	}
}
