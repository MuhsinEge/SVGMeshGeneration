using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public List<Transform> testTransform;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < testTransform.Count; i++)
        {
            var direction = testTransform[i-1].position - testTransform[i].position;
            testTransform[i].rotation = Quaternion.FromToRotation(Vector3.up, direction);
            //testTransform[i].LookAt(testTransform[i-1], Vector3.down);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
