using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
class Energy : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float StoredEnergy;
    [SerializeField] private float MagnetSpeed;

    private SphereCollider _triggerMagneticZone;
    private bool _isMagniting;
    private const float _magnetikDistance = 2f;

    #endregion

    #region Behaviour

    private void Awake()
    {
        _triggerMagneticZone = GetComponent<SphereCollider>();
        _triggerMagneticZone.radius = _magnetikDistance;
        _triggerMagneticZone.isTrigger = true;

        _isMagniting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered collider zone");

        if(other.tag == "Player" && !_isMagniting)
        {
            Debug.Log("magneting to player");

            _isMagniting = true;
            StartCoroutine(MagnetToPlayer(other.transform));
        }
    }

    private IEnumerator MagnetToPlayer(Transform playerTarget)
    {
        while (Vector3.Distance(transform.position, playerTarget.position) > .3f)
        {
            transform.position = Vector3.Slerp(transform.position, playerTarget.position, MagnetSpeed * Time.deltaTime);
            yield return null;
        }

        playerTarget.GetComponent<PlayerManager>().Stats.Energy += StoredEnergy;

        Destroy(gameObject);

        yield break;
    }

    #endregion
}
