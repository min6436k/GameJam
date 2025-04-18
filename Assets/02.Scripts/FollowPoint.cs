using UnityEngine;
using UnityEngine.InputSystem;

public class FollowPoint : MonoBehaviour
{

    public void Move(Vector3 offset)
    {
        Vector3 a = GameManager.Instance.MousePos - offset;
        a.z = -2.1f;
        transform.position = a;
    }
}
