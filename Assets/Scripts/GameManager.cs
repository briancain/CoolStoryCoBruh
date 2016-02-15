﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  private GameObject[] enemies;
  private GameObject player;
  private bool miniGameOngoing;
  private bool gameOver;

  private float gameOverTime;
  private float gameOverTimer;

  private MiniGameController miniGameController;
  public GameObject mgc;
  private AudioSource audio;
  public AudioClip gameTheme;
  public AudioClip continueGameTheme;
  public AudioClip alertTheme;
  public AudioClip gameOverTheme;

  public AudioClip loseMiniGameTheme;

  public Text gameOverText;

  private Animator leftAnim;
  private Animator rightAnim;
  public GameObject leftSnakeRig;
  public GameObject rightSnakeRig;

  // Use this for initialization
  void Start () {
    enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

    miniGameController = mgc.GetComponent<MiniGameController>();
    miniGameOngoing = false;

    audio = GetComponent<AudioSource>();

    audio.PlayOneShot(gameTheme, 1.0F);

    gameOver = false;
    gameOverTime = 8.0f;
    gameOverTimer = 0.0f;

    leftAnim = leftSnakeRig.GetComponent<Animator>();
    rightAnim = rightSnakeRig.GetComponent<Animator>();
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

    if (gameOver) {
      gameOverTimer += Time.deltaTime;
      if (gameOverTimer >= gameOverTime) {
        SceneManager.LoadScene (0);
      }
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

    leftAnim.SetBool("Slide In", true);
    rightAnim.SetBool("Slide In", true);

    if (player.GetComponent<PlayerController>().snakeChange > 0) {
      rightAnim.SetBool("Skarf On", true);
    } else {
      leftAnim.SetBool("Skarf On", true);
    }

    // Method takes number of actions to complete game
    float randGame = Random.Range(0,2);
    miniGameController.StartGame(3, randGame);

  }

  public void EndSnakeMiniGame(bool win) {
    bool isSuccess = player.GetComponent<PlayerController>().endMiniGameMode();
    miniGameOngoing = false;

    if (isSuccess) {
      Debug.Log("Game manager started player movement");
    } else {
      Debug.Log("Game manager could not start player movement");
    }

    leftAnim.SetBool("Slide In", false);
    rightAnim.SetBool("Slide In", false);

    leftAnim.SetBool("Slide Out", true);
    rightAnim.SetBool("Slide Out", true);

    if(win) {
      Debug.Log("Player Won");
      player.GetComponent<PlayerController>().ScarfSwitched();
      audio.Stop();
      EndAlertEnemies();
    } else {
      Debug.Log("Player Lost");
      audio.Stop();
      audio.PlayOneShot(loseMiniGameTheme, 0.7F);
      AlertEnemies();
    }
  }

  public void GameOver() {
    Debug.Log("Game Over");
    gameOver = true;
    gameOverText.enabled = true;
    player.GetComponent<PlayerController> ().GameOver ();
    foreach (GameObject obj in enemies) {
      if (obj != null) {
        obj.GetComponent<StationaryEnemyController>().GameOver();
      }
    }

    audio.Stop();
    audio.PlayOneShot(gameOverTheme, 1.0F);
  }

  public void WinGame() {
    Debug.Log("You win!!");
  }

  public void AlertEnemies() {
    foreach (GameObject obj in enemies) {
      if (obj != null) {
        obj.GetComponent<StationaryEnemyController>().Alert();
      }
    }
    audio.PlayOneShot(alertTheme, 1.0F);
  }

  void EndAlertEnemies() {
    foreach (GameObject obj in enemies) {
      if (obj != null) {
        obj.GetComponent<StationaryEnemyController>().EndAlert();
      }
    }

    audio.PlayOneShot(continueGameTheme, 1.0F);
  }
}
