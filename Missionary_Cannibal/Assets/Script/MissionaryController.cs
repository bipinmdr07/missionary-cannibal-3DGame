using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionaryController : MonoBehaviour {
	public GameObject gameController;
	public Animator animator;
	private Material originalMaterial;
	private Material hoverMaterial;
	private GameController gameControllerScript;

	// Use this for initialization
	void Start () {
		gameControllerScript = gameController.GetComponent<GameController> ();
		originalMaterial = transform.GetChild (1).GetComponent<SkinnedMeshRenderer> ().material;
		hoverMaterial = new Material(Shader.Find("Specular"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver(){
		if (!gameControllerScript.gameover)
			transform.GetComponentInChildren<SkinnedMeshRenderer> ().material = hoverMaterial;
	}

	void OnMouseExit(){
		transform.GetComponentInChildren<SkinnedMeshRenderer> ().material = originalMaterial;
	}

	void OnMouseDown(){
		if (!gameControllerScript.gameover)
			gameControllerScript.MouseDownFromMissionary (transform);
	}
}
