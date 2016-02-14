using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

  private Vector2 swipeDirection;
  private int taps;
  private int totalTaps;
  private int totalActions;

  private bool isOver;
  private bool tapGame;
  private bool swipeGame;
  private float coolDown;
  private GameManager gm;

  public bool StartGame(int total) {
    coolDown = 3.0f;
    totalActions = total;
    taps = 0;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_MANAGER).GetComponent<GameManager>();
    swipeGame = false;
    tapGame = false;

    //if (Random.Range(0, 1) == 0) {
    //  // mini game is swipe
    //  swipeGame = true;
    //} else {
    //  // mini game is tap
    //  tapGame = true;
    //}

    tapGame = true;
    return false;
  }

  void Update() {
    if (tapGame || swipeGame) {
      if (coolDown >= 0) {
        coolDown -= Time.deltaTime;
        Debug.Log("Cooldown: " + coolDown);
      } else {
        tapGame = false;
        swipeGame = false;
        gm.EndSnakeMiniGame(false);
      }
    }

    if (tapGame && Input.GetMouseButtonDown(0)) {
      Debug.Log("click: " + taps);
      taps++;
      if (taps == totalActions) {
        tapGame = false;
        gm.EndSnakeMiniGame(true);
      }
    }
  }
}
