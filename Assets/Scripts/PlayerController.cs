using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;
  public Text debug;

  private Rigidbody2D rb;
  private GameManager gm;

  private bool miniGameHappening;
  private bool gameOver;

  public float snakeState = 0;
  private float rateOfChange = 2f;
  public int snakeChange = 1;
  private float snakeStateMax = 100f;
  private float snakeStateMin = -100f;

  private float footstepTimeout = 0.5f;
  private float footstepTimer = 1.0f;

  private bool moving;

  private AudioSource aSource;

  public AudioClip footsteps;

  private Animator anim;

  void Start() {
    rb = gameObject.GetComponent<Rigidbody2D> ();
    gm = GameObject.FindGameObjectWithTag (Tags.GAME_MANAGER).GetComponent<GameManager> ();
    aSource = gameObject.GetComponent<AudioSource> ();
    anim = gameObject.GetComponent<Animator> ();

    miniGameHappening = false;
    gameOver = false;
    moving = false;
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

  public void GameOver() {
    gameOver = true;
  }

  // Update is called once per frame
  void Update () {
    if (!miniGameHappening && !gameOver) {
      Move();
    } else {
      // make sure player isn't moving
      rb.velocity = new Vector2(0,0);
      moving = false;
    }
    UpdateSnakeState ();
    if (footstepTimer > 0) {
      footstepTimer += Time.deltaTime;
      if (footstepTimer >= footstepTimeout) {
        footstepTimer = 0.0f;
      }
    } else if (moving) {
      aSource.PlayOneShot (footsteps, 1.0f);
      footstepTimer += Time.deltaTime;
    }
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

    if (rb.velocity != Vector2.zero) {
      moving = true;
      if (xVel > 0) {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3 (-0.25f, scale.y, scale.z);
      } else if (xVel < 0) {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3 (0.25f, scale.y, scale.z);
      }
      anim.SetBool ("Tiptoe", true);
    } else {
      moving = false;
      anim.SetBool ("Tiptoe", false);
    }
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
