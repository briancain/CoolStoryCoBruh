using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

  private Vector2 swipeDirection;
  private int taps = 0;
  private int swipes = 0;
  private int totalActions;

  private bool isOver;
  private bool tapGame;
  private bool swipeGame;
  private float coolDown;
  private GameManager gm;

  private enum SwipeDir {
    Left, Right
  };

  public bool StartGame(int total) {
    coolDown = 3.0f;
    totalActions = total;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_MANAGER).GetComponent<GameManager>();
    swipeGame = false;
    tapGame = false;

    float rand = Random.Range(0,2);
    if (rand == 0f) {
      // mini game is swipe
      Debug.Log("Swipe Game");
      swipeGame = true;
    } else {
      // mini game is tap
      Debug.Log("Tap Game");
      tapGame = true;
    }

    return false;
  }

  void Update() {
    if (tapGame || swipeGame) {
      if (coolDown >= 0) {
        coolDown -= Time.deltaTime;
      } else {
        tapGame = false;
        swipeGame = false;
        gm.EndSnakeMiniGame(false);
      }
    }

    if (tapGame && Input.GetMouseButtonDown(0)) {
      taps++;
      if (taps == totalActions) {
        tapGame = false;
        gm.EndSnakeMiniGame(true);
      }
    } else if (swipeGame && Input.GetMouseButtonDown(0)) {
      Vector2 delta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"))*40f;
      if (delta.x != 0 || delta.y !=0) {
        swipes++;
      }
      if (swipes == totalActions) {
        swipeGame = false;
        gm.EndSnakeMiniGame(true);
      }
    }
  }
}
