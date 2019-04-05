using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
class InteractiveEnergy : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float StoredEnergy;    // how much energy we will add
    [SerializeField] private float MagnetSpeed;     // 
                                                    
    private SphereCollider _triggerMagneticZone;    //
    private bool _isMagniting;                      //
    private const float _magnetikDistance = 2f;     //

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

        Destroy(gameObject);

        yield break;
    }

    #endregion
}
