using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform Target;      // target transform
    [SerializeField] private float MovementSpeed;   // camera movement speed
    [SerializeField] private Vector3 Offset;        // relative camera position from target

    #endregion

    #region Behaviour

    // initializing
    private void Awake()
    {
        transform.position = Target.position + Offset;
        transform.LookAt(Target);
    }

    // moving camera
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            Target.position + Offset, 
            MovementSpeed * Time.fixedDeltaTime
            );
    }

    #endregion
}
