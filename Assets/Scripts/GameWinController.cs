using UnityEngine;
using System.Collections;

public class GameWinController : MonoBehaviour {

  public AudioClip gameWin;
  private AudioSource audio;

  void Start() {
    audio = GetComponent<AudioSource>();
    audio.PlayOneShot(gameWin, 1.0f);
  }

  // Update is called once per frame
  void Update () {
    if (Input.GetKeyDown (KeyCode.Space)) {
      Application.Quit ();
    }
  }
}
