using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollector : MonoBehaviour
{
    public EventHandler<Collider> ballFellToGroundEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            ballFellToGroundEvent?.Invoke(this, other);
        }
    }
}
