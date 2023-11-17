using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem collectBallParticle;
    [SerializeField] ParticleSystem ballOnGroundParticle;

    public void PlaySuccessParticle() { successParticle.Play(); }
    public void PlayCollectBallParticle() { collectBallParticle.Play(); }
    public void PlayBallOnGroundParticle() { ballOnGroundParticle.Play(); }

    public void KillAllParticles()
    {
        successParticle.Stop();
        collectBallParticle.Stop();
        ballOnGroundParticle.Stop();
    }
}
