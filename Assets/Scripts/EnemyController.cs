using UnityEngine;
using System.Collections;

public class EnemyController : StationaryEnemyController {

  public float enemySpeed;
  public GameObject[] waypoints;

  private Rigidbody2D rb;
  private int currWaypoint = 1;
  private bool forwardPath = true;
  private bool moving = true;

  private float stopTimer = 0f;

  /////////////////////////////////
  /// Unity Methods
  /////////////////////////////////

	void Start () {
    base.Start ();
    rb = gameObject.GetComponent<Rigidbody2D> ();
    facing = (waypoints [currWaypoint].transform.position - gameObject.transform.position).normalized;
	}
	
	void Update () {
    if (!alert) {
      Move ();
    }
    base.Update ();
  }


  /////////////////////////////////
  /// Public Methods
  /////////////////////////////////

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
}
