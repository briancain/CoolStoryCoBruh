using UnityEngine;
using System.Collections;

public class ElevatorAnimationEvent : MonoBehaviour {

  public GameObject gameManager;

  private GameManager gm;

  void Start() {
    gm = gameManager.GetComponent<GameManager> ();
  }

  void WinGame() {
    gm.WinGame ();
  }
}
