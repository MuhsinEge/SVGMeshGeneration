using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public static class MeshGenerator
{

    public const float radius = 14f;
    public const int segments = 32;
    public static Mesh GenerateTubeMesh(TextAsset svgFolder, Vector3 targetPipeStart)
    {
        var coordinates = SVGConverter.GenerateCoordinates(svgFolder);
        List<Vector3> pathPoints = ParseSVGPath(coordinates);
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

    private static void MoveToTargetPipeStart(ref List<Vector3> pathPoints, Vector3 targetPipeStart)
    {
        if(Vector3.Distance(pathPoints[0], targetPipeStart) > Vector3.Distance(pathPoints[pathPoints.Count - 1], targetPipeStart))
        {
            pathPoints.Reverse();
        }
        Vector3 displacement = pathPoints[0] - targetPipeStart;

        for (int i = 0; i < pathPoints.Count; i++)
        {
           pathPoints[i] -= displacement;
            pathPoints[i] += Vector3.up * 8;
            pathPoints[i] += Vector3.left; // Some magic number. tube_empty model is not symetric. 
        }
    }

    static List<Vector3> ParseSVGPath(string[] pathPointDatas)
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
                    float y = float.Parse(parameters, CultureInfo.InvariantCulture);
                    Vector3 endPoint = new Vector3(currentPoint.x, y, 0);
                    pathPoints.Add(endPoint);
                    currentPoint = endPoint;
                    break;
            }
        }
        return pathPoints;
    }

    static Vector3 ParseVector(string vectorString)
    {
        string[] components = vectorString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (components.Length >= 2)
        {
            float x = float.Parse(components[0], CultureInfo.InvariantCulture);
            float y = float.Parse(components[1], CultureInfo.InvariantCulture);
            x = Mathf.Round(x * 1000f) / 1000f;
            y = Mathf.Round(y * 1000f) / 1000f;
            return new Vector3(x, y, 0);
        }
        else
        {
            Debug.LogError("Invalid vector string: " + vectorString);
            return Vector3.zero;
        }
    }

    static List<Vector3> ParseVectorList(string vectorListString)
    {
        string[] vectorStrings = vectorListString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<Vector3> vectors = new List<Vector3>();

        for (int i = 0; i < vectorStrings.Length; i += 2)
        {
            if (i + 1 < vectorStrings.Length)
            {
                float x = float.Parse(vectorStrings[i], CultureInfo.InvariantCulture);
                float y = float.Parse(vectorStrings[i + 1], CultureInfo.InvariantCulture);
                x = Mathf.Round(x * 1000f) / 1000f;
                y = Mathf.Round(y * 1000f) / 1000f;
                vectors.Add(new Vector3(x, y, 0));
            }
            else
            {
                Debug.LogError("Invalid vector list string: " + vectorListString);
            }
        }

        return vectors;
    }

    static List<Vector3> BezierCurve(Vector3 startPoint, List<Vector3> controlPoints)
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

    //public Mesh GenerateTubeMeshWIP(TextAsset svgFolder, Vector3 targetPipeStart) // DRAWS BETTER ON VECTOR 3 SVG, BUT WORSE ON VECTOR 2
    //{
    //    List<Vector3> pathPoints;
    //    List<Vector3> vertices = new List<Vector3>();
    //    pathPoints = ParseSVGPath(SVGConverter.GenerateCoordinates(svgFolder));
    //    MoveToTargetPipeStart(ref pathPoints, targetPipeStart);

    //    Mesh mesh = new Mesh();


    //    List<Vector2> uv = new List<Vector2>();

    //    GameObject previousParentObject = null;
    //    GameObject parentObject = null;

    //    for (int i = 0; i < pathPoints.Count; i++)
    //    {
    //        Vector3 point = pathPoints[i];
    //        parentObject = new GameObject("ParentObject_" + i);
    //        parentObject.transform.position = pathPoints[i];
    //        List<GameObject> children = new List<GameObject>();

    //        for (int j = 0; j <= segments; j++)
    //        {
    //            float angle = 2 * Mathf.PI * j / segments;
    //            Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius + point.x, point.y, Mathf.Sin(angle) * radius + point.z);
    //            var child = new GameObject("child" + j);
    //            children.Add(child);
    //            child.transform.position = vertex;
    //            child.transform.parent = parentObject.transform;

    //            uv.Add(new Vector2((float)j / segments, (float)i / (pathPoints.Count - 1)));
    //        }

    //        if (previousParentObject != null)
    //        {
    //            var direction = parentObject.transform.position - previousParentObject.transform.position;
    //            parentObject.transform.rotation = Quaternion.FromToRotation(Vector3.up / 100, direction);
    //            //parentObject.transform.LookAt(previousParentObject.transform, Vector3.down * 1000);
    //            Destroy(previousParentObject);
    //        }

    //        foreach (var child in children)
    //        {
    //            vertices.Add(child.transform.position);
    //        }
    //        previousParentObject = parentObject;
    //    }

    //    if (parentObject != null)
    //    {
    //        Destroy(parentObject);
    //    }

    //    int[] triangles = new int[segments * 6 * (pathPoints.Count - 1)];

    //    int triangleIndex = 0;

    //    for (int i = 0; i < pathPoints.Count - 1; i++)
    //    {
    //        for (int j = 0; j < segments; j++)
    //        {
    //            int next = (j + 1) % segments;

    //            int current = i * (segments + 1) + j;
    //            int currentNext = i * (segments + 1) + next;
    //            int nextRow = (i + 1) * (segments + 1) + j;
    //            int nextRowNext = (i + 1) * (segments + 1) + next;

    //            // First triangle
    //            triangles[triangleIndex++] = current;
    //            triangles[triangleIndex++] = currentNext;
    //            triangles[triangleIndex++] = nextRow;

    //            // Second triangle
    //            triangles[triangleIndex++] = currentNext;
    //            triangles[triangleIndex++] = nextRowNext;
    //            triangles[triangleIndex++] = nextRow;
    //        }
    //    }

    //    mesh.vertices = vertices.ToArray();
    //    mesh.uv = uv.ToArray();
    //    mesh.triangles = triangles;
    //    return mesh;
    //}

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

    //public int startingGizmo = 0;
    //public int endGizmo = 2;
    //private void OnDrawGizmos()
    //{
    //    if (pathPoints.Count > 0)
    //    {
    //        for (int i = 1; i < pathPoints.Count; i++)
    //        {
    //            Gizmos.DrawSphere(pathPoints[i], 0.1f);
    //        }
    //    }
    //    if (vertices.Count > 0)
    //    {
    //        for (int i = (startingGizmo * 32) -1 >= 0 ? (startingGizmo * 32) -1 : 0; i < endGizmo * 32; i++)
    //        {
    //            Gizmos.DrawSphere(vertices[i], 0.2f);
    //        }
    //    }
    //}
}
