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
  private float rateOfChange = 8f;
  public int snakeChange = 1;
  private float snakeStateMax = 100f;
  private float snakeStateMin = -100f;

  private float footstepTimeout = 0.75f;
  private float footstepTimer = 1.5f;

  private bool moving;

  private AudioSource aSource;

  public AudioClip footsteps;
  public AudioClip keyPickup;

  private bool hasKey = false;
  private Animator anim;

  public GameObject rig;

  public GameObject tongueLeft;
  public GameObject tongueRight;

  void Start() {
    rb = gameObject.GetComponent<Rigidbody2D> ();
    gm = GameObject.FindGameObjectWithTag (Tags.GAME_MANAGER).GetComponent<GameManager> ();
    aSource = gameObject.GetComponent<AudioSource> ();
    anim = rig.GetComponent<Animator> ();

    anim.SetBool ("Left Scarf", true);

    miniGameHappening = false;
    gameOver = false;
    moving = false;

    tongueLeft.transform.localScale = new Vector3(0f,1f,1f);
    tongueRight.transform.localScale = new Vector3(0f,1f,1f);
  }

  public bool startMiniGameMode(){
    Debug.Log("Starting mini game, halting player movement");
    if (!miniGameHappening) {
      miniGameHappening = true;
      anim.SetBool ("Crouch", true);
      anim.SetBool ("Snake Fight!", true);
      return true;
    } else {
      return false;
    }
  }

  public bool endMiniGameMode() {
    Debug.Log("Mini game over, continuing player movement");
    if (miniGameHappening) {
      miniGameHappening = false;
      anim.SetBool ("Snake Fight!", false);
      anim.SetBool ("Crouch", false);
      return true;
    } else {
      return false;
    }
  }

  public void GameOver() {
    gameOver = true;
    anim.SetBool ("Tiptoe", false);
    anim.SetBool ("Head Careful", false);
    anim.SetBool ("Head Surprised", true);
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

    if (snakeChange > 0) {
      tongueRight.transform.localScale = new Vector3(0f,1f,1f);
    } else {
      tongueLeft.transform.localScale = new Vector3(0f,1f,1f);
    }

    snakeChange *= -1;

    snakeState = 0f;
    if (snakeChange < 0) {
      anim.SetBool ("Left Scarf", false);
      anim.SetBool ("Right Scarf", true);
    } else {
      anim.SetBool ("Left Scarf", true);
      anim.SetBool ("Right Scarf", false);
    }
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

        GameObject uiObj = GameObject.FindGameObjectWithTag("SnakeBar");
        uiObj.transform.parent = null;
        gameObject.transform.localScale = new Vector3 (-0.25f, scale.y, scale.z);
        uiObj.transform.parent = gameObject.transform;
      } else if (xVel < 0) {
        Vector3 scale = gameObject.transform.localScale;

        GameObject uiObj = GameObject.FindGameObjectWithTag("SnakeBar");
        uiObj.transform.parent = null;
        gameObject.transform.localScale = new Vector3 (0.25f, scale.y, scale.z);
        uiObj.transform.parent = gameObject.transform;
      }
      anim.SetBool ("Tiptoe", true);
      anim.SetBool ("Head Careful", true);
    } else {
      moving = false;
      anim.SetBool ("Tiptoe", false);
      anim.SetBool ("Head Careful", false);
    }
  }

  private void UpdateSnakeState() {
    snakeState += rateOfChange * Time.deltaTime * snakeChange;

    //Debug.Log("snake state: " + snakeState);

    if (snakeState > 0) {
      snakeState = Mathf.Min (snakeState, snakeStateMax);

      Vector3 oldScale = tongueRight.transform.localScale;
      tongueRight.transform.localScale = new Vector3((snakeState/100),oldScale.y, oldScale.x);
    } else {
      snakeState = Mathf.Max (snakeState, snakeStateMin);

      Vector3 oldScale = tongueLeft.transform.localScale;
      tongueLeft.transform.localScale = new Vector3((Mathf.Abs(snakeState)/100),oldScale.y, oldScale.x);
    }


    if (snakeState == snakeStateMax || snakeState == snakeStateMin) {
      gm.AlertEnemies ();
      anim.SetBool ("Head Careful", false);
      anim.SetBool ("Head Angry", true);
      if (snakeState > 0) {
        anim.SetBool ("Right Angry", true);
      } else {
        anim.SetBool ("Left Angry", true);
      }
    } else {
      anim.SetBool ("Head Angry", false);
      anim.SetBool ("Left Angry", false);
      anim.SetBool ("Right Angry", false);
    }

    debug.text = "Snake State: " + (snakeState < 0 ? Mathf.Ceil (snakeState) : Mathf.Floor (snakeState));
  }

  void OnCollisionEnter2D(Collision2D coll) {
    if (coll.gameObject.tag == Tags.KEY) {
      aSource.PlayOneShot(keyPickup,1.0f);
      Debug.Log("Player got key");
      hasKey = true;
      Destroy(coll.gameObject);
    }

    if (coll.gameObject.tag == Tags.DOOR && hasKey) {
      Destroy(coll.gameObject);
    }
  }

}
