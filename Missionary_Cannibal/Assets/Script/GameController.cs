using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	// array of size 2 for storing the transform of cannibal and missionary
	public GameObject canvas;
	public Transform[] charTransform;
	private PauseMenuScript pms;

	// get the gameobject from the scene that contains all the positions of the characters
	public Transform charPosition;

	// lets get the boat
	private Transform boat;

	private float leftMax, rightMax;

	// grab the script of boat for checking the passangers in boat;
	public BoatController boatScript;

	// Use this for initialization
	void Start () {
		pms = canvas.GetComponent<PauseMenuScript> ();
		leftMax = 300f;
		rightMax = 270f;

		// at the start of the game spawn all the characters in the leftBank at their respective position
		Transform leftRiverBank = charPosition.GetChild(0);
		// now spawn missionary for first 3 children of leftRiverBank and next 3 children with cannibal
		// spawn missionary
		for(int i = 0; i < 3; i++){
			Instantiate (charTransform [0], leftRiverBank.GetChild(i).position, Quaternion.identity, leftRiverBank.GetChild(i)); 
		}

		// sqawn cannibal
		for (int i = 3; i < 6; i++) {
			Instantiate (charTransform [1], leftRiverBank.GetChild(i).position, Quaternion.identity, leftRiverBank.GetChild(i));
		}

		boat = charPosition.GetChild (1);
		boatScript = boat.GetComponent<BoatController> ();
	}

	void Update(){
		if (boatScript.moveBoat && (boat.GetChild (0).childCount != 0 || boat.GetChild (1).childCount != 0)) {
			if (boatScript.boatPosition == "left_bank") {
				// we need to go to right river bank
				if (boat.position.x > rightMax) {
					boatScript.isMoving = true;
					boat.position -= (new Vector3 (1, 0, 0) * 10 * Time.deltaTime);
				} else {
					boatScript.isMoving = false;
					boatScript.moveBoat = false;
					boatScript.boatPosition = BoatController.boatPositionEnum.right_bank.ToString ();
				}
			} else if (boatScript.boatPosition == "right_bank") {
				if (boat.position.x < leftMax) {
					boatScript.isMoving = true;
					boat.position += (new Vector3 (1, 0, 0) * 10 * Time.deltaTime);
				} else {
					boatScript.isMoving = false;
					boatScript.moveBoat = false;
					boatScript.boatPosition = BoatController.boatPositionEnum.left_bank.ToString ();
				}
			}
		} else {
			boatScript.moveBoat = false;
		}
	}

	// This method is called whenever missionary is being clicked
	public void MouseDownFromMissionary(Transform missionaryTransform){
		
		// move the character to boat if it is in river bank
		if (!boatScript.isMoving && ((missionaryTransform.parent.parent.name == "LeftRiverBank" && boatScript.boatPosition == "left_bank") || (missionaryTransform.parent.parent.name == "RightRiverBank" && boatScript.boatPosition == "right_bank"))) {
			
			// move the transform to boat and make it's availability false
			// then make it a child of boat
			if (boatScript.passenger1SpotEmpty) {
				
				missionaryTransform.position = boat.GetChild (0).position;
				boatScript.passenger1SpotEmpty = false;
				missionaryTransform.SetParent (charPosition.GetChild(1).GetChild(0));

			} else if (boatScript.passenger2SpotEmpty) {
				
				missionaryTransform.position = boat.GetChild (1).position;
				boatScript.passenger2SpotEmpty = false;
				missionaryTransform.SetParent (charPosition.GetChild (1).GetChild (1));
			}

		} else if (missionaryTransform.parent.parent.name == "Boat") { // else character is in the boat and we want it to go to the riverbank, i.e. 2 possible river_banks

			// we need to check the boat position and move to the corresponding bank from boat
			if (boatScript.boatPosition == "left_bank" && !boatScript.isMoving) {
				
				for (int i = 0; i < 3; i++) {
					
					Transform new_parent = charPosition.GetChild (0);
					if (new_parent.GetChild(i).childCount == 0) {
						
						if (missionaryTransform.parent.name == "Passenger1") {
							
							boatScript.passenger1SpotEmpty = true;

						} else if (missionaryTransform.parent.name == "Passenger2") {
							
							boatScript.passenger2SpotEmpty = true;

						}

						missionaryTransform.position = new_parent.GetChild(i).position;
						missionaryTransform.SetParent (new_parent.GetChild(i));
						break;
					}
				}
			} else if (boatScript.boatPosition == "right_bank" && !boatScript.isMoving) {

				for (int i = 0; i < 3; i++) {
					Transform newParent = charPosition.GetChild (2);

					if (newParent.GetChild (i).childCount == 0) {

						if (missionaryTransform.parent.name == "Passenger1") {
							boatScript.passenger1SpotEmpty = true;
						} else if (missionaryTransform.parent.name == "Passenger2"){
							boatScript.passenger2SpotEmpty = true;
						}

						missionaryTransform.position = newParent.GetChild(i).position;
						missionaryTransform.SetParent (newParent.GetChild(i));
					}
				}
			}
		}
	}

	// This method is called whenever cannibal is being clicked
	public void MouseDownFromCannibal(Transform cannibalTransform){
		// move the character to boat if it is in river bank
		if (!boatScript.isMoving && ((cannibalTransform.parent.parent.name == "LeftRiverBank" && boatScript.boatPosition == "left_bank") || (cannibalTransform.parent.parent.name == "RightRiverBank" && boatScript.boatPosition == "right_bank"))) {

			// move the transform to boat and make it's availability false
			// then make it a child of boat
			if (boatScript.passenger1SpotEmpty) {

				cannibalTransform.position = boat.GetChild (0).position;
				boatScript.passenger1SpotEmpty = false;
				cannibalTransform.SetParent (charPosition.GetChild(1).GetChild(0));

			} else if (boatScript.passenger2SpotEmpty) {

				cannibalTransform.position = boat.GetChild (1).position;
				boatScript.passenger2SpotEmpty = false;
				cannibalTransform.SetParent (charPosition.GetChild (1).GetChild (1));
			}

		} else if (cannibalTransform.parent.parent.name == "Boat") { // else character is in the boat and we want it to go to the riverbank, i.e. 2 possible river_banks

			// we need to check the boat position and move to the corresponding bank from boat
			if (boatScript.boatPosition == "left_bank" && !boatScript.isMoving) {

				for (int i = 3; i < 6; i++) {

					Transform new_parent = charPosition.GetChild (0);
					if (new_parent.GetChild(i).childCount == 0) {

						if (cannibalTransform.parent.name == "Passenger1") {

							boatScript.passenger1SpotEmpty = true;

						} else if (cannibalTransform.parent.name == "Passenger2") {

							boatScript.passenger2SpotEmpty = true;

						}

						cannibalTransform.position = new_parent.GetChild(i).position;
						cannibalTransform.SetParent (new_parent.GetChild(i));
						break;
					}
				}
			} else if (boatScript.boatPosition == "right_bank" && !boatScript.isMoving) {
				for (int i = 3; i < 6; i++) {
					Transform newParent = charPosition.GetChild (2);

					if (newParent.GetChild (i).childCount == 0) {

						if (cannibalTransform.parent.name == "Passenger1") {
							boatScript.passenger1SpotEmpty = true;
						} else if (cannibalTransform.parent.name == "Passenger2"){
							boatScript.passenger2SpotEmpty = true;
						}

						cannibalTransform.position = newParent.GetChild(i).position;
						cannibalTransform.SetParent (newParent.GetChild(i));
					}
				}
			}

		}
	}

	public void Check(){
		// we need to check for two conditions
		// first, for outnumbering of cannibals over missionaries
		// second, the successful completion of the game

		// first we keep record of cannibals and missionaries in boat
		int missionaries_boat = 0;
		int cannibals_boat = 0;
		for (int i = 0; i <= 1; i++) {
			if (boat.GetChild (i).childCount != 0) {
				Transform t = boat.GetChild (i).GetChild (0);
				if (t.tag == "missionary") {
					missionaries_boat += 1;
				} else {
					cannibals_boat += 1;
				}
			}
		}

		int missionaries_left_bank = 0;
		int cannibals_left_bank = 0;
		// missionaries in left bank
		for(int i = 0; i < 3; i++){
			if (charPosition.GetChild (0).GetChild (i).childCount != 0) {
				missionaries_left_bank += 1;
			}
		}

		// cannibals in left bank
		for (int i = 3; i < 6; i++) {
			if (charPosition.GetChild (0).GetChild (i).childCount != 0) {
				cannibals_left_bank += 1;
			}
		}

		if (boatScript.boatPosition == "left_bank") {
			missionaries_left_bank += missionaries_boat;
			cannibals_left_bank += cannibals_boat;
		}

		if ((missionaries_left_bank != 0) && (cannibals_left_bank > missionaries_left_bank) || ((3 - missionaries_left_bank) != 0) && ((3 - cannibals_left_bank) > (3 - missionaries_left_bank))) {
			pms.ShowGameOverMenu ();
			Debug.Log ("GameOver!!!!!!!");
		}

		if (missionaries_left_bank == 0 && cannibals_left_bank == 0) {
			pms.ShowCongratulationMenu ();
			Debug.Log ("*******Congratulation*******");
		}
	}
}
