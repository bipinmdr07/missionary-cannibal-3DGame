using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {
	public Transform gameController;
	public bool passenger1SpotEmpty;
	public bool passenger2SpotEmpty;
	private GameController gameControllerScript;
	public enum boatPositionEnum
	{
		left_bank,
		right_bank
	};

	private string previousBoatPos;
	public string boatPosition;

	private Material originalMaterial;
	private Material hoverMaterial;

	public bool moveBoat;
	public bool isMoving;


	// Use this for initialization
	void Start () {
		gameControllerScript = gameController.GetComponent<GameController> ();
		passenger1SpotEmpty = true;
		passenger2SpotEmpty = true;
		boatPosition = boatPositionEnum.left_bank.ToString();
		previousBoatPos = boatPosition;
		moveBoat = false;
		isMoving = false;

		originalMaterial = transform.GetComponent<MeshRenderer> ().material;
		hoverMaterial = new Material (Shader.Find ("Specular"));
		hoverMaterial.SetColor ("Red Color", Color.red);
	}

	// Update is called once per frame
	void Update () {
		// acts as a trigger, call for check whenever there is change in the boat position
		if (boatPosition != previousBoatPos) {
			gameControllerScript.Check ();
			previousBoatPos = boatPosition;
		}
	}

	void OnMouseOver(){
		transform.GetComponent<MeshRenderer> ().material = hoverMaterial;
	}

	void OnMouseExit(){
		transform.GetComponent<MeshRenderer> ().material = originalMaterial;
	}

	void OnMouseDown(){
		if (!isMoving)
			moveBoat = true;
	}
}
