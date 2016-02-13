using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  private GameObject[] enemies;

  // Use this for initialization
  void Start () {
    enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
  }
  // Update is called once per frame
  void Update () {
  }

  void GameOver() {
    Debug.Log("Game Over");
    //SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  void WinGame() {
    Debug.Log("You win!!");
  }

  void AlertEnemies() {
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
