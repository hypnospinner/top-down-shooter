using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
class InteractiveEnergy : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float StoredEnergy;    // how much energy we will add
    [SerializeField] private float MagnetSpeed;     // how quickly object pulls to player
                                                    
    private SphereCollider _triggerMagneticZone;    // reference to a trigger zone that tracks player
    private bool _isMagniting;                      // whether object is magneting to player or not
    private const float _magnetikDistance = 2f;     // trigger zone radius

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
        Func<float, float> f = t => t * t;
        float time = 0f;

        while (Vector3.Distance(transform.position, playerTarget.position) > .3f)
        {
            transform.position += (playerTarget.position - transform.position).normalized 
                * MagnetSpeed 
                * f(time += Time.fixedDeltaTime);
            yield return null;
        }

        playerTarget.GetComponent<PlayerManager>().Stats.Energy += StoredEnergy;

        // TODO: insert consumption vfx

        Destroy(gameObject);

        yield break;
    }

    #endregion
}
