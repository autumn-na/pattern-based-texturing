using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3[] vertices;
    public Vertex.VertexType[] verticeType;

    public string mat = "";

    public Material mat000;
    public Material mat011;
    public Material mat100;
    public Material mat111;

    public Texture ggg;
    public Texture ddd;
    public Texture dgg;
    public Texture gdd;

    private void Awake()
    {
        verticeType = new Vertex.VertexType[3];
    }

    public void SetVerticeType(int num, Vertex.VertexType type)
    {
        verticeType[num] = type;
        mat += ((int)(type)).ToString();
    }

    public void SetMaterial()
    {
        switch (mat)
        {
            
            case "000":
                GetComponent<MeshRenderer>().material = mat000;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", TextureCreator.Instance.GetNoisedTextureFrom((Texture2D)ddd));
                break;
            case "011":
            case "101":
            case "110":
                GetComponent<MeshRenderer>().material.SetInt("_Rotation", 180);
                GetComponent<MeshRenderer>().material = mat011;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", TextureCreator.Instance.GetNoisedTextureFrom((Texture2D)dgg));
                break;
            case "100":
            case "010":
            case "001":
                GetComponent<MeshRenderer>().material = mat100;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", TextureCreator.Instance.GetNoisedTextureFrom((Texture2D)gdd));
                break;
            case "111":
                GetComponent<MeshRenderer>().material = mat111;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", TextureCreator.Instance.GetNoisedTextureFrom((Texture2D)ggg));
                break;
            default:
                break;
        }
    }

    public void SetMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }
}   
