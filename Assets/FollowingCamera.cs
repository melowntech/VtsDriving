using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public GameObject target;

    public Vector3 targetOffset;
    public float cameraPitch;
    public bool freezeRotation = false;

    private Quaternion Rotation()
    {
        if (freezeRotation)
            return Quaternion.Euler(90, target.transform.rotation.eulerAngles[1], 0);
        return target.transform.rotation * Quaternion.Euler(cameraPitch, 0, 0);
    }

    private void Start()
    {
        transform.rotation = Rotation();
        transform.position = target.transform.position + transform.rotation * targetOffset;
    }

    void FixedUpdate()
    {
        Quaternion rot = Rotation();
        transform.position = Vector3.Lerp(transform.position, target.transform.position + rot * targetOffset, 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
    }
}
