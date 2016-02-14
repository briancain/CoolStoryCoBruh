using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

  private Vector2 swipeDirection;
  private int taps;
  private int totalTaps;

  private bool winning;
  private bool isOver;
  private bool tapGame;
  private bool swipeGame;
  private float coolDown;
  private GameManager gm;

  public bool StartGame() {
    winning = false;
    coolDown = 5.0f;
    totalTaps = 3;
    taps = 0;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_MANAGER).GetComponent<GameManager>();
    swipeGame = false;
    tapGame = false;

    //if (Random.Range(0, 1) == 0) {
    //  // mini game is swipe
    //  winning = StartSwipeGame();
    //} else {
    //  // mini game is tap
    //  winning = StartTapGame();
    //}

    tapGame = true;
    return false;
  }

  void Update() {
    if (tapGame || swipeGame) {
      Debug.Log("tap game & swipegame " + tapGame + " " + swipeGame);
      if (coolDown >= 0) {
        coolDown -= Time.deltaTime;
        Debug.Log("Cooldown: " + coolDown);
      } else {
        winning = false;
        tapGame = false;
        swipeGame = false;
        gm.EndSnakeMiniGame(winning);
      }
    }

    if (tapGame && Input.GetMouseButtonDown(0)) {
      Debug.Log("click: " + taps);
      taps++;
      if (taps == totalTaps) {
        winning = true;
        tapGame = false;
        gm.EndSnakeMiniGame(winning);
      }
    }
  }
}
