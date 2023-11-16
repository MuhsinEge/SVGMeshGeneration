using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    public float radius = 1.0f;
    public int segments = 16;

    public Mesh GenerateTubeMesh(TextAsset svgFolder, Vector3 targetPipeStart)
    {

        List<Vector3> pathPoints;
        pathPoints = ParseSVGPath(SVGConverter.GenerateCoordinates(svgFolder));
        MoveToTargetPipeStart(ref pathPoints, targetPipeStart);

        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 point = pathPoints[i];

            for (int j = 0; j <= segments; j++)
            {
                float angle = 2 * Mathf.PI * j / segments;
                Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius + point.x, point.y, Mathf.Sin(angle) * radius + point.z);
                vertices.Add(vertex);

                uv.Add(new Vector2((float)j / segments, (float)i / (pathPoints.Count - 1)));
            }
        }

        int[] triangles = new int[segments * 6 * (pathPoints.Count - 1)];

        int triangleIndex = 0;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int next = (j + 1) % segments;

                int current = i * (segments + 1) + j;
                int currentNext = i * (segments + 1) + next;
                int nextRow = (i + 1) * (segments + 1) + j;
                int nextRowNext = (i + 1) * (segments + 1) + next;

                // First triangle
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = currentNext;
                triangles[triangleIndex++] = nextRow;

                // Second triangle
                triangles[triangleIndex++] = currentNext;
                triangles[triangleIndex++] = nextRowNext;
                triangles[triangleIndex++] = nextRow;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles;
        return mesh;
    }

    private void MoveToTargetPipeStart(ref List<Vector3> pathPoints, Vector3 targetPipeStart)
    {
        var startingVector = Vector3.Distance(pathPoints[0],targetPipeStart) < Vector3.Distance(pathPoints[pathPoints.Count -1],targetPipeStart) ? 0 : pathPoints.Count -1;
        Vector3 displacement = pathPoints[startingVector] - targetPipeStart;

        for (int i = 0; i < pathPoints.Count; i++)
        {
           pathPoints[i] -= displacement;
            pathPoints[i] += Vector3.up * 8;
            pathPoints[i] += Vector3.right * 1.5f; // Some magic number. tube_empty model is not symetric. 
        }
    }

    List<Vector3> ParseSVGPath(string[] pathPointDatas)
    {
        List<Vector3> pathPoints = new List<Vector3>();

        Vector3 currentPoint = Vector3.zero;

        foreach (string command in pathPointDatas)
        {
            char type = command[0];
            string parameters = command.Substring(1);

            switch (type)
            {
                case 'M':
                case 'L':
                    currentPoint = ParseVector(parameters);
                    pathPoints.Add(currentPoint);
                    break;
                case 'C':
                    List<Vector3> controlPoints = ParseVectorList(parameters);
                    pathPoints.AddRange(BezierCurve(currentPoint, controlPoints));
                    currentPoint = controlPoints[controlPoints.Count - 1];
                    break;
                case 'V':
                    float y = float.Parse(parameters);
                    Vector3 endPoint = new Vector3(currentPoint.x, y, 0);
                    pathPoints.Add(endPoint);
                    currentPoint = endPoint;
                    break;
            }
        }

        foreach (Vector3 x in pathPoints)
        {
            Debug.Log(x);
        }
        return pathPoints;
    }

    Vector3 ParseVector(string vectorString)
    {
        string[] components = vectorString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (components.Length >= 2)
        {
            float x = float.Parse(components[0]);
            float y = float.Parse(components[1]);
            return new Vector3(x, y, 0);
        }
        else
        {
            Debug.LogError("Invalid vector string: " + vectorString);
            return Vector3.zero;
        }
    }

    List<Vector3> ParseVectorList(string vectorListString)
    {
        string[] vectorStrings = vectorListString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<Vector3> vectors = new List<Vector3>();

        for (int i = 0; i < vectorStrings.Length; i += 2)
        {
            if (i + 1 < vectorStrings.Length)
            {
                float x = float.Parse(vectorStrings[i]);
                float y = float.Parse(vectorStrings[i + 1]);
                vectors.Add(new Vector3(x, y, 0));
            }
            else
            {
                Debug.LogError("Invalid vector list string: " + vectorListString);
            }
        }

        return vectors;
    }

    List<Vector3> BezierCurve(Vector3 startPoint, List<Vector3> controlPoints)
    {
        List<Vector3> points = new List<Vector3>();

        for (float t = 0; t <= 1; t += 0.1f)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * startPoint;
            p += 3 * uu * t * controlPoints[0];
            p += 3 * u * tt * controlPoints[1];
            p += ttt * controlPoints[2];

            points.Add(p);
        }

        return points;
    }
    //private void Update()
    //{
    //    if (pathPoints.Count > 0)
    //    {
    //        for (int i = 1; i < pathPoints.Count; i++)
    //        {
    //            Debug.DrawLine(pathPoints[i - 1], pathPoints[i]);
    //        }
    //    }
    //}
}
