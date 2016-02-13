using UnityEngine;
using System.Collections;

public class Layers : MonoBehaviour {

  public const int DEFAULT = 0;
  public const int IGNORE_RAYCAST = 2;

  public static int CreateLayerMask(int layer) {
    return 1 << layer;
  }
}
