using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenController : MonoBehaviour {
  float timer = 0.0f;
  void Update() {
    timer += Time.deltaTime;
    if (timer >= 171.0f || Input.GetKeyDown(KeyCode.Space)) {
      SceneManager.LoadScene (1);
    }
  }
}
