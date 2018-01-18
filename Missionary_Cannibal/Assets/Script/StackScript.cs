using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackScript : MonoBehaviour {
	public void Push(int left_m_count, int left_c_count, int right_m_count, int right_c_count, string boat_p){
		GameObject newEmpty = new GameObject ();
//		newEmpty.AddComponent<StateScript> ();
		StateScript sscript = newEmpty.GetComponent<StateScript> ();
		sscript.missionary_count_left = left_m_count;
		sscript.cannibal_count_left = left_c_count;
		sscript.missionary_count_right = right_m_count;
		sscript.cannibal_count_right = right_c_count;
		sscript.boat_position = boat_p;

		Instantiate (newEmpty, transform);
	}

	public void Pop(){
		int last = transform.childCount - 1;

		Destroy (transform.GetChild (last).gameObject);
	}
}
