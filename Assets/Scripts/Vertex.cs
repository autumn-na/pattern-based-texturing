using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public enum VertexType
    {
        DIRT,
        GRASS
    }

    public VertexType type;
    public Vector3 localPos;

    public void RandType()
    {
        type = (VertexType)Random.Range(0, System.Enum.GetValues(typeof(VertexType)).Length);
    }
}
