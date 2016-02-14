using UnityEngine;
using System.Collections;

public class StationaryEnemyController : MonoBehaviour {
  public bool alert = false;
  public Vector2 defaultFacing;

  public enum Facing { RIGHT, BACK_RIGHT, BACK, BACK_LEFT, LEFT, FRONT_LEFT, FRONT, FRONT_RIGHT }

  private float losDistance = 8f;
  private float losAngle = 120f;

  private GameManager gm;

  protected Animator anim;

  protected Vector2 facing;

  protected GameObject player;

  protected bool gameOver;

  private AudioSource audio;
  public AudioClip robotAlert;
  public AudioClip hover;

  /////////////////////////////////
  /// Unity Methods
  /////////////////////////////////

  protected virtual void Start () {
    player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
    facing = defaultFacing;
    anim = gameObject.GetComponent<Animator>();
    gm = GameObject.FindGameObjectWithTag (Tags.GAME_MANAGER).GetComponent<GameManager> ();
    gameOver = false;

    audio = GetComponent<AudioSource>();
  }

  protected virtual void Update () {
    if (!gameOver) {
      if (alert) {
        facing = (player.transform.position - gameObject.transform.position).normalized;
      }
      UpdateFacing ();
      LineOfSight ();
    }
  }

  // Send this enemy into an alert state
  public void Alert() {
    float playerDistance = Vector2.Distance(player.transform.position, gameObject.transform.position);
    Debug.Log("Player Distance: " + playerDistance);
    if (playerDistance <= 5.0f) {
      audio.PlayOneShot(robotAlert, 0.7F);
    }
    alert = true;
  }

  // End this enemy's alert status
  public virtual void EndAlert() {
    facing = defaultFacing;
    alert = false;
  }

  public void GameOver() {
    gameOver = true;
  }

  // Look for the player
  private void LineOfSight() {
    bool closeEnough = Vector2.Distance (gameObject.transform.position, player.transform.position) <= losDistance;
    Vector2 toPlayer = (player.transform.position - gameObject.transform.position).normalized;
    float angleBetween = Vector2.Angle (facing, toPlayer);
    if (closeEnough && angleBetween <= losAngle / 2.0f) {
      // The player is within the guard's LOS, so make a raycast to ensure nothing is in the way
      RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, toPlayer, losDistance, ~Layers.CreateLayerMask(Layers.IGNORE_RAYCAST));
      if (hit != null && hit.collider.gameObject.tag == Tags.PLAYER) {
        audio.PlayOneShot(robotAlert, 0.7F);
        gm.GameOver ();
      }
    }
  }

  // Update the facing of the sprite animation
  private void UpdateFacing() {
    anim = gameObject.GetComponent<Animator>();
    float angle = Vector2.Angle (facing, Vector2.right);
    int animFacing;
    if (angle < 22.5) {
      animFacing = (int)Facing.RIGHT;
    } else if (angle >= 22.5 && angle < 67.5) {
      if (facing.y > 0) {
        animFacing = (int)Facing.BACK_RIGHT;
      } else {
        animFacing = (int)Facing.FRONT_RIGHT;
      }
    } else if (angle >= 67.5 && angle < 112.5) {
      if (facing.y > 0) {
        animFacing = (int)Facing.BACK;
      } else {
        animFacing = (int)Facing.FRONT;
      }
    } else if (angle >= 112.5 && angle < 167.5) {
      if (facing.y > 0) {
        animFacing = (int)Facing.BACK_LEFT;
      } else {
        animFacing = (int)Facing.FRONT_LEFT;
      }
    } else {
      animFacing = (int)Facing.LEFT;
    }

    if (animFacing != anim.GetInteger ("Facing")) {
      anim.SetInteger ("Facing", animFacing);
      anim.SetTrigger ("Move");
    }
  }
}
