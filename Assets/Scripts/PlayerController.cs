using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;

  private Rigidbody2D rb;

  void Start() {
    rb = gameObject.GetComponent<Rigidbody2D> ();
  }

  // Update is called once per frame
  void Update () {
    Move ();
  }

  // Moves the player based on input
  private void Move() {
    float xVel = playerSpeed * Input.GetAxisRaw ("Horizontal");
    float yVel = playerSpeed * Input.GetAxisRaw ("Vertical");

    rb.velocity = new Vector2 (xVel, yVel);
  }
}
