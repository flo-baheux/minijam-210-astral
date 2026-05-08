#nullable enable
using UnityEngine;

namespace Laywelin {
  public static class Lay2D {
    public static void LookAtTarget(Transform transform, Vector3 target) {
      var dir = target - transform.position;
      var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public static void LookAtDirection(Transform transform, Vector3 dir) {
      var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public static Vector2 RandomAroundTarget(Vector2 target, float minRadius = 0, float maxRadius = 1) {
      return target + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
    }

    public static RaycastHit2D[] RaycastCameraToMousePos() {
      return Physics2D.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.transform.forward);
    }
  }
}