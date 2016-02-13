using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float enemySpeed;
  public enum Facing { LEFT, RIGHT, UP, DOWN };
	public Facing currFacing;
  public Transform[] waypoints;

	private Rigidbody2D rb;
  private int currWaypoint = 1;
  private bool forwardPath = true;
  private bool moving = true;
  private float stopTime = 2f;
  private float stopTimer = 0f;
  private float losDistance = 15f;
  private float losAngle = 120f;

  private GameObject player;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
    player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
    LineOfSight ();
	}

  // Move the enemy based on their facing
  private void Move() {
    if (moving) {
      // Move towards the next waypoint at the enemy speed
      rb.position = Vector2.MoveTowards (gameObject.transform.position, waypoints [currWaypoint].position, enemySpeed * Time.deltaTime);

    } else {
      // Make sure teh enemy stops for a certain time before moving to the next waypoint
      stopTimer += Time.deltaTime;
      if (stopTimer >= stopTime) {
        moving = true;
        stopTimer = 0f;
      }
    }

    // If the enemy has moved onto one of its waypoints, stop movement and target the
    // next waypoint
    if (rb.transform.position == waypoints [currWaypoint].position && moving) {
      moving = false;
      if (currWaypoint == 0 || currWaypoint == waypoints.Length - 1) {
        forwardPath = !forwardPath;
      }
      if (forwardPath) {
        currWaypoint++;
      } else {
        currWaypoint--;
      }
    }
  }

	// Returns a normalized direction vector based on the Enemy's
	// current facing
	private Vector2 GetDirectionVector() {
		switch (currFacing) {
    case Facing.LEFT:
      return Vector2.left;
    case Facing.RIGHT:
      return Vector2.right;
    case Facing.UP:
      return Vector2.up;
    case Facing.DOWN:
      return Vector2.down;
    default:
      // This case should be unreachable
      return Vector2.zero;
		}
	}

  // Look for the player
  private void LineOfSight() {
    bool closeEnough = Vector2.Distance (gameObject.transform.position, player.transform.position) <= losDistance;
    Vector2 toPlayer = (player.transform.position - gameObject.transform.position).normalized;
    float angleBetween = Vector2.Angle (GetDirectionVector (), toPlayer);
    if (closeEnough && angleBetween <= losAngle / 2.0f) {
      // The player is within the guard's LOS, so make a raycast to ensure nothing is in the way
      RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, toPlayer, losDistance, ~Layers.CreateLayerMask(Layers.IGNORE_RAYCAST));
      if (hit != null && hit.collider.gameObject.tag == Tags.PLAYER) {
        print ("ENEMY SAW PLAYER");
      }
    }
  }
}
