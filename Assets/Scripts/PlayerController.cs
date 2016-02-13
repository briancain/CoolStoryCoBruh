using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;

  private Rigidbody2D rb;
  private bool miniGameHappening;

  void Start() {
    rb = gameObject.GetComponent<Rigidbody2D> ();

    miniGameHappening = false;
  }

  public bool startMiniGameMode(){
    Debug.Log("Starting mini game");
    if (!miniGameHappening) {
      miniGameHappening = true;
      return true;
    } else {
      return false;
    }
  }

  public bool endMiniGameMode() {
    Debug.Log("Mini game over");
    if (miniGameHappening) {
      miniGameHappening = false;
      return true;
    } else {
      return false;
    }
  }

  // Update is called once per frame
  void Update () {
    if (!miniGameHappening) {
      Move ();
    }
  }

  // Moves the player based on input
  private void Move() {
    float xVel = playerSpeed * Input.GetAxisRaw ("Horizontal");
    float yVel = playerSpeed * Input.GetAxisRaw ("Vertical");

    rb.velocity = new Vector2 (xVel, yVel);
  }
}
