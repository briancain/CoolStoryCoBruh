using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  private GameObject[] enemies;
  private GameObject player;
  private bool miniGameOngoing;

  private MiniGameController miniGameController;
  public GameObject mgc;

  // Use this for initialization
  void Start () {
    enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

    miniGameController = mgc.GetComponent<MiniGameController>();
    miniGameOngoing = false;
  }
  // Update is called once per frame
  void Update () {
    float playerSnakeState = player.GetComponent<PlayerController>().snakeState;
    float playerSnakeChange = player.GetComponent<PlayerController>().snakeChange;
    float absPlSnakeState = Mathf.Abs(playerSnakeState);
    if (Input.GetKey(KeyCode.Space) && !miniGameOngoing
        && (absPlSnakeState >= 10 && absPlSnakeState <= 100)
        && ((playerSnakeChange > 0 && playerSnakeState > 0)
            || (playerSnakeChange < 0 && playerSnakeState < 0))) {
      miniGameOngoing = true;
      InitSnakeMiniGame();
    }
  }

  void InitSnakeMiniGame() {
    bool isSuccess = player.GetComponent<PlayerController>().startMiniGameMode();
    bool winGame = false;

    if (isSuccess) {
      Debug.Log("Game manager stopped player movement");
    } else {
      Debug.Log("Game manager could not stop player movement");
    }

    // Method takes number of actions to complete game
    miniGameController.StartGame(3);

  }

  public void EndSnakeMiniGame(bool win) {
    bool isSuccess = player.GetComponent<PlayerController>().endMiniGameMode();
    miniGameOngoing = false;

    if (isSuccess) {
      Debug.Log("Game manager started player movement");
    } else {
      Debug.Log("Game manager could not start player movement");
    }

    if(win) {
      Debug.Log("Player Won");
      player.GetComponent<PlayerController>().ScarfSwitched();
      EndAlertEnemies();
    } else {
      Debug.Log("Player Lost");
      AlertEnemies();
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
        obj.GetComponent<StationaryEnemyController>().Alert();
      }
    }
  }

  void EndAlertEnemies() {
    foreach (GameObject obj in enemies) {
      if (obj != null) {
        obj.GetComponent<StationaryEnemyController>().EndAlert();
      }
    }
  }
}
