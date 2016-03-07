using UnityEngine;
using System.Collections;

public class ElevatorAnimationEvent : MonoBehaviour {

  public GameObject gameManager;
  public AudioClip ohKO;

  private GameManager gm;
  private PlayerController pc;
  private AudioSource audio;

  void Start() {
    gm = gameManager.GetComponent<GameManager> ();
    audio = GetComponent<AudioSource> ();
  }

  void WinGame() {
    gm.WinGame ();
  }

  void PlayerSpeak() {
    audio.PlayOneShot (ohKO);
  }
}
