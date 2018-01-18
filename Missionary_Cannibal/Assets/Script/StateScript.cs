using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript {
	public enum boat_pos {left_bank, right_bank};
	public int missionary_count_left;
	public int cannibal_count_left;
	public int missionary_count_right;
	public int cannibal_count_right;
	public string boat_position = "";

	public StateScript(int m_no_l, int c_no_l, int m_no_r, int c_no_r, string b_pos){
		missionary_count_left = m_no_l;
		cannibal_count_left = c_no_l;
		missionary_count_right = m_no_r;
		cannibal_count_right = c_no_r;
		boat_position = b_pos;
	}

	public StateScript(){
	}

	public void SetMissionaryCannibalCountInLeft(int m_count_left, int c_count_left){
		missionary_count_left = m_count_left;
		cannibal_count_left = c_count_left;
	}

	public void SetMissionaryCannibalCountInRight(int m_count_right, int c_count_right){
		missionary_count_right = m_count_right;
		cannibal_count_right = c_count_right;
	}
}
