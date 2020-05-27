/*
 * Developer Name: Longchang Cui
 * Date: May-25-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem[] particles;
     
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Play the particle effects if a line is cleared.
    /// </summary>
    public void PlayParticle()
    {
        foreach (var particleSys in particles)
        {
            particleSys.Stop();
            particleSys.Play();
        }
    }

}
