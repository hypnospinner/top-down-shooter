using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private Vector3 Offset;

    private void Awake()
    {
        transform.LookAt(Target);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            Target.position + Offset, 
            MovementSpeed * Time.deltaTime
            );
        transform.LookAt(Target);
    }

}
