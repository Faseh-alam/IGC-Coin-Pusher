using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MachineSwitcher : MonoBehaviour {

	// The active machine index
	public int activeMachine = 1;

	// This is to enable the switcher buttons on the GUI
	public bool isSwitcherGUIActive = false;

	// Use this for initialization
	void Start () {

    }
    
	// Update is called once per frame
	void Update () {
	

	}

	/// <summary>
	/// This is called from the GUI to switch the active machine
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void switchMachineButton(int direction)
	{
        SceneManager.LoadScene(direction);
	}
}
