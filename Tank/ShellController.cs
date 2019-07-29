using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField] private LayerMask fireableLayer;
    [SerializeField] private float explodeRadius;
    private ParticleSystem explodeParticles;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        explodeParticles = GameObject.Find("ShellExplosion").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Collider[] colliders = Physics.OverlapSphere(
        //    transform.position,
        //    explodeRadius,
        //    fireableLayer);
        //for(int i = 0;i < colliders.Length;++i)
        //{
        //    Rigidbody currRigibody = colliders[i].GetComponent<Rigidbody>();
        //    if (!currRigibody)
        //        continue;


        //}
        explodeParticles.transform.position  = transform.position;
        explodeParticles.Play();
        //Destroy(explodeParticles.gameObject, explodeParticles.main.duration);
        Destroy(gameObject);
    }
}
