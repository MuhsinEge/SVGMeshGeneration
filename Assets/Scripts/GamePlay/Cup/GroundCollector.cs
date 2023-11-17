using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollector : MonoBehaviour
{
    public EventHandler<Collider> ballFellToGroundEvent;
    [SerializeField] ParticleSystem ballFellToGroundParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            ballFellToGroundEvent?.Invoke(this, other);
            ballFellToGroundParticle.Play();
        }
    }
}
