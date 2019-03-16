using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform target;      // target transform
    [SerializeField] private float MovementSpeed;   // camera movement speed
    [SerializeField] private Vector3 Offset;        // relative camera position from target

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    #endregion

    #region Behaviour

    public void InitializeComponent()
    {
        transform.position = target.position + Offset;
        transform.LookAt(target);
    }

    // moving camera
    private void FixedUpdate()
    {
        if(target != null)
            transform.position = Vector3.Lerp(
                transform.position, 
                target.position + Offset, 
                MovementSpeed * Time.fixedDeltaTime
                );
    }

    #endregion
}
