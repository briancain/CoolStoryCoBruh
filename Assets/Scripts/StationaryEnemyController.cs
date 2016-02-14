using UnityEngine;
using System.Collections;

public class StationaryEnemyController : MonoBehaviour {
  protected bool alert = false;
  public Vector2 defaultFacing;

  private float losDistance = 15f;
  private float losAngle = 120f;

  protected Vector2 facing;

  protected GameObject player;

  /////////////////////////////////
  /// Unity Methods
  /////////////////////////////////

  protected virtual void Start () {
    player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
    facing = defaultFacing;
  }

  protected virtual void Update () {
    if (alert) {
      facing = (player.transform.position - gameObject.transform.position).normalized;
    }
    LineOfSight ();
  }

  // Send this enemy into an alert state
  public void Alert() {
    alert = true;
  }

  // End this enemy's alert status
  public virtual void EndAlert() {
    facing = defaultFacing;
    alert = false;
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
