﻿using UnityEngine;
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

  public bool StartGame(int total, float randGame) {
    coolDown = 3.0f;
    totalActions = total;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_MANAGER).GetComponent<GameManager>();
    swipeGame = false;
    tapGame = false;

    if (randGame == 0f) {
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


    if (Input.GetMouseButtonDown(0)) {
      Debug.Log("Tap Game: " + tapGame);
      Debug.Log("Swipe Game: " + swipeGame);

      if (tapGame) {
        taps++;
        Debug.Log("Taps! : " + taps);
        if (taps == totalActions) {
          taps = 0;
          tapGame = false;
          gm.EndSnakeMiniGame(true);
        }
      } else if (swipeGame) {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"))*40f;
        Debug.Log("Swipe Delta: " + delta);
        if (delta.x != 0 || delta.y !=0) {
          swipes++;
        }
        if (swipes == totalActions) {
          swipes = 0;
          swipeGame = false;
          gm.EndSnakeMiniGame(true);
        }
      }
    }
  }
}
