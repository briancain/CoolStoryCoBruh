﻿using UnityEngine;
using System.Collections;

public class GameWinController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown (KeyCode.Space)) {
      Application.Quit ();
    }
	}
}
