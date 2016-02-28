using UnityEngine;
using System.Collections;

using Pathfinding;

public class EnemyController : StationaryEnemyController {

  public GameObject[] waypoints;

  private Rigidbody2D rb;
  public int currWaypoint = 1;
  private bool forwardPath = true;
  public bool moving = true;
  public bool patrolling = true;

  private Transform target;
  private Seeker seeker;
  private Path path;
  //The max distance from the AI to a waypoint for it to continue to the next waypoint
  public float nextWaypointDistance = 1f;
  private int currentPathWaypoint = 0;

  private float stopTimer = 0f;

  private Transform lastPatrolPosition;

  /////////////////////////////////
  /// Unity Methods
  /////////////////////////////////

	void Start () {
    base.Start ();
    rb = gameObject.GetComponent<Rigidbody2D> ();
    facing = (waypoints [currWaypoint].transform.position - gameObject.transform.position).normalized;
    anim.speed = 5;

    seeker = gameObject.GetComponent<Seeker> ();
    //seeker.StartPath (transform.position, waypoints [currWaypoint].transform.position, OnPathComplete);
	}

  void OnPathComplete(Path p) {
    if (!p.error) {
      path = p;
      currentPathWaypoint = 0;
      if (target != null) {
        seeker.StartPath (transform.position, target.position, OnPathComplete);
        target = null;
      }
    }
  }
	
	void Update () {
    if (!gameOver) {
      Move ();
    }
    base.Update ();
  }


  /////////////////////////////////
  /// Public Methods
  /////////////////////////////////

  // End this enemy's alert status
  public override void EndAlert() {
    facing = (lastPatrolPosition.position - gameObject.transform.position).normalized;
    alert = false;
    //calculatingPath = true;
    //seeker.ReleaseClaimedPath ();
    //seeker.StartPath (transform.position, lastPatrolPosition.position, OnPathComplete);
  }

  public override void Alert() {
    base.Alert ();
    patrolling = false;
    //calculatingPath = true;
    //lastPatrolPosition = gameObject.transform;
    //seeker.StartPath (transform.position, player.transform.position, OnPathComplete);
  }

  /////////////////////////////////
  /// Private Methods
  /////////////////////////////////

  // Move the enemy along their patrol path
  public override void Move() {
    if (!alert) {
      GameObject dest = waypoints [currWaypoint];
      Waypoint wp = dest.GetComponent<Waypoint> ();

      if (moving) {
        // Move towards the next waypoint at the enemy speed
        rb.position = Vector2.MoveTowards (gameObject.transform.position, dest.transform.position, enemySpeed * Time.deltaTime);

      } else {
        // Make sure the enemy stops for a certain time before moving to the next waypoint
        stopTimer += Time.deltaTime;
        if (stopTimer >= wp.waitTime) {
          moving = true;
          anim.speed = 5;
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
        anim.speed = 1;
      }
    }
//    else if (path != null) {
//      if (currentPathWaypoint >= path.vectorPath.Count)
//      {
//        if (!alert) {
//          patrolling = true;
//          Debug.Log ("PATROLLING");
//        }
//        path = null;
//        return;
//      }
//
//      //Direction to the next waypoint
//      Vector3 dir = ( path.vectorPath[currentPathWaypoint] - transform.position ).normalized;
//      facing = dir;
//      dir *= enemySpeed * Time.deltaTime;
//      this.gameObject.transform.Translate(dir);
//
//      //Check if we are close enough to the next waypoint
//      //If we are, proceed to follow the next waypoint
//      if (Vector3.Distance( transform.position, path.vectorPath[ currentPathWaypoint ] ) < nextWaypointDistance)
//      {
//        currentPathWaypoint++;
//        return;
//      }
//
//      if (alert) {
//        if (calculatingPath) {
//          target = player.transform;
//        } else {
//          seeker.StartPath (transform.position, player.transform.position, OnPathComplete);
//        }
//      }
//    }
  }
}
