using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    const float ballMaxVelocity = 50f;
    List<Rigidbody> _ballList;
    

    public void GenerateLevel(Transform parent, Level level)
    {
        if(_ballList != null)
        {
            foreach(Rigidbody ball in _ballList)
            {
                Destroy(ball.gameObject);
            }
        }
        GameObject ballParent = new GameObject("Balls");
        ballParent.transform.parent = parent;
        _ballList = new List<Rigidbody>();
        while (_ballList.Count < level.ballCount)
        {
            Rigidbody instantiatedOBject = Instantiate(level.ballPrefab, GetUniqueWorldPos(level), Quaternion.identity);
            var instantiatedObjectMeshRenderer = instantiatedOBject.GetComponent<MeshRenderer>();
            var randomColor = colors[Random.Range(0, colors.Count)];
            instantiatedObjectMeshRenderer.material.color = randomColor;
            instantiatedOBject.transform.parent = ballParent.transform;
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
