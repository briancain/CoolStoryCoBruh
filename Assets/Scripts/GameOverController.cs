using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour {

  void Update() {
    if (Input.GetKeyDown (KeyCode.Space)) {
      SceneManager.LoadScene (1);
    }
  }
}
