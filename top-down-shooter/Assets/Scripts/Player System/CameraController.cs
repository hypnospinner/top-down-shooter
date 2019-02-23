using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private Vector3 Offset;

    private void Awake()
    {
        transform.position = Target.position + Offset;
        transform.LookAt(Target);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            Target.position + Offset, 
            MovementSpeed * Time.fixedDeltaTime
            );
    }

}
