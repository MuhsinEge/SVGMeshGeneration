using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    const float ballMaxVelocity = 50f;
    List<Rigidbody> _ballList;

    public void GenerateLevel(Level level)
    {
        if(_ballList != null)
        {
            foreach(Rigidbody ball in _ballList)
            {
                Destroy(ball.gameObject);
            }
        }
        _ballList = new List<Rigidbody>();
        while (_ballList.Count < level.ballCount)
        {
            Rigidbody instantiatedOBject = Instantiate(level.ballPrefab, GetUniqueWorldPos(level), Quaternion.identity);
            _ballList.Add(instantiatedOBject);
        }
    }

    Vector3 GetUniqueWorldPos(Level level) {
        var pos = new Vector3();
        pos = Random.insideUnitSphere * level.spawnRadius;
        pos.z = 0;
        pos += transform.position + level.spawnOffset;
        return pos;
    }

    private void FixedUpdate()
    {
        if(_ballList == null) return;
        foreach(var ball in _ballList)
        {
            ball.velocity = Vector3.ClampMagnitude(ball.velocity, ballMaxVelocity);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position + spawnOffset, radius);
    //}
}
