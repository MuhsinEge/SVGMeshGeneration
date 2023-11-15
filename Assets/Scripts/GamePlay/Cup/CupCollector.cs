using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupCollector : MonoBehaviour
{
    public EventHandler<Collider> ballCollectedEvent;
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Ball")
        {
            ballCollectedEvent?.Invoke(this, other);
        }
    }
}
