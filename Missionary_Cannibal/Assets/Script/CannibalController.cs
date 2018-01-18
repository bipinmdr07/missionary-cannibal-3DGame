using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannibalController : MonoBehaviour {

	public GameObject gameController;
	private Material originalMaterial;
	private Material hoverMaterial;
	private GameController gameControllerScript;
	public Animator animator;

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
		if(!gameControllerScript.gameover)
			gameControllerScript.MouseDownFromCannibal (transform);
	}
}
