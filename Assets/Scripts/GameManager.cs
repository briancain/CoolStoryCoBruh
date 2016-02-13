using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  private GameObject[] enemies;
  private GameObject player;

  // Use this for initialization
  void Start () {
    enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
  }
  // Update is called once per frame
  void Update () {
  }

  void InitSnakeMiniGame() {
    bool isSuccess = player.GetComponent<PlayerController>().startMiniGameMode();

    if (isSuccess) {
      Debug.Log("Game manager started mini game");
    } else {
      Debug.Log("Game manager could not start mini game");
    }

  }

  void GameOver() {
    Debug.Log("Game Over");
    //SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  void WinGame() {
    Debug.Log("You win!!");
  }

  public void AlertEnemies() {
    foreach (GameObject obj in enemies) {
      if (obj != null) {
          //obj.Alert();
      }
    }
  }

  void EndAlertEnemies() {
    foreach (GameObject obj in enemies) {
      if (obj != null) {
          //obj.EndAlert();
      }
    }
  }
}
