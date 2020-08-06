using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePrefabController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //particle = this.GetComponent<ParticleSystem>();
        //particleRenderer = this.GetComponent<ParticleSystemRenderer>();
    }
    
    public float time = 0;
    public ParticleSystemRenderer particleRenderer;

    public ParticleSystem particle;
    // Update is called once per frame
    void Update()
    {
        if (particle.isStopped)
        {
            Destroy(this.gameObject);
        }
        time -= Time.deltaTime;
        if (time <= 0)
        {
            particleRenderer.sortingOrder = 0;

        }
    }
}
