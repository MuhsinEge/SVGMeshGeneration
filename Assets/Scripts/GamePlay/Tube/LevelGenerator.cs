using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;
using System.Linq;

public class LevelGenerator : MeshGenerator
{
    GameObject currentTube;
    BallGenerator _ballGenerator;
    [SerializeField] Material pipeMaterial;

    public void Initialize()
    {
        _ballGenerator = GetComponent<BallGenerator>();
    }

    public void LoadLevel(Level levelData)
    {
        if (!ReferenceEquals(currentTube, null))
        {
            Destroy(currentTube);
        }

        currentTube = Instantiate(levelData.tubePrefab, transform);

        if (levelData.svgLevelFile != null)
        {
            var pipe1 = new GameObject("Pipe1");
            var pipe2 = new GameObject("Pipe2");

            pipe1.transform.parent = currentTube.transform;
            pipe1.transform.localPosition = Vector3.zero;

            pipe2.transform.parent = currentTube.transform;
            pipe2.transform.localPosition = Vector3.zero;

            var pipe1MeshFilter = pipe1.AddComponent<MeshFilter>();
            var pipe1MeshRenderer = pipe1.AddComponent<MeshRenderer>();

            var pipe2MeshFilter = pipe2.AddComponent<MeshFilter>();
            var pipe2MeshRenderer = pipe2.AddComponent<MeshRenderer>();

            var mesh = GenerateTubeMesh(levelData.svgLevelFile, currentTube.transform.localPosition);

            pipe1MeshFilter.mesh = mesh;
            pipe2MeshFilter.mesh = mesh;
            pipe2MeshFilter.mesh.triangles = pipe2MeshFilter.mesh.triangles.Reverse().ToArray();

            pipe1MeshRenderer.material = pipeMaterial;
            pipe2MeshRenderer.material = pipeMaterial;
            pipe1.AddComponent<MeshCollider>();
            pipe2.AddComponent<MeshCollider>();
            pipe1.transform.localScale = Vector3.one * 0.025f;
            pipe2.transform.localScale = Vector3.one * 0.025f;

        }
        _ballGenerator.GenerateLevel(currentTube.transform,levelData);
    }
}
