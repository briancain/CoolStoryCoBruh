using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;
  public Text debug;

  private Rigidbody2D rb;
  private GameManager gm;

  private bool miniGameHappening;

  public float snakeState = 0;
  private float rateOfChange = 2f;
  public int snakeChange = 1;
  private float snakeStateMax = 100f;
  private float snakeStateMin = -100f;

  void Start() {
    rb = gameObject.GetComponent<Rigidbody2D> ();
    gm = GameObject.FindGameObjectWithTag (Tags.GAME_MANAGER).GetComponent<GameManager> ();

    miniGameHappening = false;
  }

  public bool startMiniGameMode(){
    Debug.Log("Starting mini game, halting player movement");
    if (!miniGameHappening) {
      miniGameHappening = true;
      return true;
    } else {
      return false;
    }
  }

  public bool endMiniGameMode() {
    Debug.Log("Mini game over, continuing player movement");
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
      Move();
    } else {
      // make sure player isn't moving
      rb.velocity = new Vector2(0,0);
    }
    UpdateSnakeState ();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == Tags.ELEVATOR) {
      gm.WinGame ();
    }
  }

  public void ScarfSwitched() {
    snakeChange *= -1;
  }

  // Moves the player based on input
  private void Move() {
    float xVel = playerSpeed * Input.GetAxisRaw ("Horizontal");
    float yVel = playerSpeed * Input.GetAxisRaw ("Vertical");

    rb.velocity = new Vector2 (xVel, yVel);
  }

  private void UpdateSnakeState() {
    snakeState += rateOfChange * Time.deltaTime * snakeChange;
    if (snakeState > 0) {
      snakeState = Mathf.Min (snakeState, snakeStateMax);
    } else {
      snakeState = Mathf.Max (snakeState, snakeStateMin);
    }

    if (snakeState == snakeStateMax || snakeState == snakeStateMin) {
      gm.AlertEnemies ();
    }

    debug.text = "Snake State: " + (snakeState < 0 ? Mathf.Ceil (snakeState) : Mathf.Floor (snakeState));
  }

}
