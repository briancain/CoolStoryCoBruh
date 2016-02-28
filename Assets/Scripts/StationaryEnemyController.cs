using UnityEngine;
using System.Collections;
using Pathfinding;

public class StationaryEnemyController : MonoBehaviour {
  public bool alert = false;
  public Vector2 defaultFacing;

  public enum Facing { RIGHT, BACK_RIGHT, BACK, BACK_LEFT, LEFT, FRONT_LEFT, FRONT, FRONT_RIGHT }

  private float losDistance = 5f;
  private float losAngle = 90f;
  public float enemySpeed;

  private GameManager gm;

  protected Animator anim;

  protected Vector2 facing;

  protected GameObject player;

  protected bool gameOver;

  private AudioSource audio;
  public AudioClip robotAlert;
  public AudioClip hover;
  private float hoverTimeout = 0.5f;
  private float hoverCooldown = 1f;
  private Seeker seeker;

  private Path path;

  public GameObject losArc;

  private int currentPathWaypoint = 0;
  //The max distance from the AI to a waypoint for it to continue to the next waypoint
  public float nextWaypointDistance = 1f;

  private Transform startPosition;

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
    seeker = gameObject.GetComponent<Seeker> ();
    startPosition = gameObject.transform;
  }

  protected virtual void Update () {
    if (!gameOver) {
      if (alert) {
        facing = (player.transform.position - gameObject.transform.position).normalized;
      }
      Move ();
      UpdateFacing ();
      LineOfSight ();

      if (hoverCooldown > 0) {
        hoverCooldown += Time.deltaTime;
        if (hoverCooldown >= hoverTimeout) {
          hoverCooldown = 0.0f;
        }
      } else {
        PlayHover();
        hoverCooldown += Time.deltaTime;
      }
    }
  }

  public virtual void Move() {
    if (alert) {
      if (seeker.IsDone ()) {
        seeker.StartPath (transform.position, player.transform.position, OnPathComplete);
      }
    }
    if (path != null) {
      if (currentPathWaypoint >= path.vectorPath.Count)
      {
        path = null;
        if (!alert) {
          facing = defaultFacing;
        }
        return;
      }

      //Direction to the next waypoint
      Vector3 dir = ( path.vectorPath[currentPathWaypoint] - transform.position ).normalized;
      facing = dir;
      dir *= enemySpeed * Time.deltaTime;
      this.gameObject.transform.Translate(dir);

      //Check if we are close enough to the next waypoint
      //If we are, proceed to follow the next waypoint
      if (Vector3.Distance( transform.position, path.vectorPath[ currentPathWaypoint ] ) < nextWaypointDistance)
      {
        currentPathWaypoint++;
        return;
      }
    }
  }

  void OnPathComplete(Path p) {
    if (!p.error) {
      path = p;
      currentPathWaypoint = 0;
    }
  }

  void PlayHover() {
    bool closeEnough = Vector2.Distance (gameObject.transform.position, player.transform.position) <= 15f;

    if (closeEnough) {
      audio.PlayOneShot(hover, 1.0F);
    }
  }

  // Send this enemy into an alert state
  public virtual void Alert() {
    float playerDistance = Vector2.Distance(player.transform.position, gameObject.transform.position);
    if (playerDistance <= 5.0f) {
      audio.PlayOneShot(robotAlert, 0.7F);
    }
    alert = true;
  }

  // End this enemy's alert status
  public virtual void EndAlert() {
    seeker.StartPath (transform.position, startPosition.position, OnPathComplete);
    //facing = defaultFacing;
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
  protected void UpdateFacing() {
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

    // Update the LOS cone
    float rotation = Vector2.Angle(new Vector2(-1, 1), facing) * Mathf.Sign(Vector3.Dot(new Vector3(0f, 0f, 1f), Vector3.Cross(new Vector3(-1, 1, 0), (Vector3)facing)));  
    losArc.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);
  }
}
