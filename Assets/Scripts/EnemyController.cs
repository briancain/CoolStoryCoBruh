using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float enemySpeed;
  public enum Facing { LEFT, RIGHT, UP, DOWN };
	public Facing currFacing;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

  // Move the enemy based on their facing
  private void Move() {
    rb.velocity = enemySpeed * GetDirectionVector ();
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
}
