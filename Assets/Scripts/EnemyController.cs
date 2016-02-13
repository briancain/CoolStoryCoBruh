using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float enemySpeed;
  public GameObject[] waypoints;

  private Rigidbody2D rb;
  private int currWaypoint = 1;
  private bool forwardPath = true;
  private bool moving = true;
  public bool alert = false;

  private float stopTimer = 0f;

  private float losDistance = 15f;
  private float losAngle = 120f;

  private Vector2 facing;

  private GameObject player;

  /////////////////////////////////
  /// Unity Methods
  /////////////////////////////////

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
    player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
    facing = (waypoints [currWaypoint].transform.position - gameObject.transform.position).normalized;
	}
	
	void Update () {
    if (!alert) {
      Move ();
    } else {
      facing = (player.transform.position - gameObject.transform.position).normalized;
    }
    LineOfSight ();
  }


  /////////////////////////////////
  /// Public Methods
  /////////////////////////////////

  // Send this enemy into an alert state
  public void Alert() {
    alert = true;
  }

  // End this enemy's alert status
  public void EndAlert() {
    facing = (waypoints[currWaypoint].transform.position - gameObject.transform.position).normalized;
    alert = false;
  }

  /////////////////////////////////
  /// Private Methods
  /////////////////////////////////

  // Move the enemy along their patrol path
  private void Move() {
    GameObject dest = waypoints [currWaypoint];
    Waypoint wp = dest.GetComponent<Waypoint> ();

    if (moving) {
      // Move towards the next waypoint at the enemy speed
      rb.position = Vector2.MoveTowards (gameObject.transform.position, dest.transform.position, enemySpeed * Time.deltaTime);

    } else {
      // Make sure teh enemy stops for a certain time before moving to the next waypoint
      stopTimer += Time.deltaTime;
      if (stopTimer >= wp.waitTime) {
        moving = true;
        stopTimer = 0f;
        if (currWaypoint == 0 || currWaypoint == waypoints.Length - 1) {
          forwardPath = !forwardPath;
        }
        if (forwardPath) {
          currWaypoint++;
        } else {
          currWaypoint--;
        }
        dest = waypoints [currWaypoint];
        wp = dest.GetComponent<Waypoint> ();
        facing = (dest.transform.position - gameObject.transform.position).normalized;
      }
    }

    // If the enemy has moved onto one of its waypoints, stop 
    if (rb.transform.position == dest.transform.position && moving) {
      print ("Should stop");
      moving = false;
      facing = wp.directionToLook;
    }
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
        print ("ENEMY SAW PLAYER");
      }
    }
  }
}
