using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshCreator : MonoBehaviour
{
    public GameObject vertexPref;
    public GameObject[,] vertice;

    public GameObject cellPref;
    public GameObject[,] cells;
    public GameObject cellParent;

    public Material[] materials;

    public Image noise;

    int x = 50;
    int z = 50;

    void Start()
    {
        CreateVertice();
        CreateCells();
        SetMaterial(false);  
    }


    void CreateVertice()
    {
        vertice = new GameObject[x, z];
        float[,] yPos = new float[x, z];

        for (int i_z = 0; i_z < z; i_z++)
        {
            for (int j_x = 0; j_x < x; j_x++)
            {
                yPos[j_x, i_z] = Mathf.PerlinNoise(j_x * 0.1f, i_z * 0.1f) * 10;
                Vector3 pos = new Vector3(-0.5f + j_x + (float)(i_z % 2) / 2.0f, yPos[j_x, i_z], -0.5f * Mathf.Sqrt(3) / 2.0f + i_z);

                vertice[j_x, i_z] = Instantiate(vertexPref, pos, Quaternion.identity);

                if (yPos[j_x, i_z] <= 4) vertice[j_x, i_z].GetComponent<Vertex>().type = Vertex.VertexType.DIRT;
                else vertice[j_x, i_z].GetComponent<Vertex>().type = Vertex.VertexType.GRASS;
            }
        }


        for (int i_z = 0; i_z < z; i_z++)
        {
            for (int j_x = 0; j_x < x; j_x++)
            {
                yPos[j_x, i_z] /= 10.0f;
            }
        }
        Rect rect = new Rect(0, 0, yPos.GetLength(0), yPos.GetLength(1));
        //noise.sprite = Sprite.Create(TextureCreator.Instance.GetTextureFrom(yPos), rect, new Vector2(0.5f, 0.5f));
    }
    bool IsCombination(Vertex.VertexType[] types)
    {
        Vertex.VertexType[,] comb =
        {
                        {   Vertex.VertexType.DIRT,   Vertex.VertexType.DIRT,   Vertex.VertexType.DIRT},
                        {   Vertex.VertexType.GRASS,   Vertex.VertexType.GRASS,   Vertex.VertexType.DIRT},
                        {   Vertex.VertexType.DIRT,   Vertex.VertexType.DIRT,   Vertex.VertexType.GRASS},
                        {   Vertex.VertexType.GRASS,   Vertex.VertexType.GRASS,   Vertex.VertexType.GRASS},
        };

        bool isOk = true;

        for (int i = 0; i < comb.GetLength(0); i++)
        {
            isOk = true;
            for (int j = 0; j < 3; j++)
            {
                if (types[j] != comb[i, j])
                {
                    isOk = false;
                    break;
                }
            }

            if (isOk) break;
        }

        return isOk;
    }


    void CreateCells()
    {
        int cellX = (x - 1) * 2;
        int cellZ = z - 1;
        cells = new GameObject[cellX, cellZ] ;

        for (int i_z = 0; i_z < z - 1; i_z++)
        {
            for (int j_x = 0; j_x < x - 1; j_x++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Vector3[] cellVertice = new Vector3[3];
                    Vertex.VertexType[] verticeType = new Vertex.VertexType[3];

                    int[] v = { 0, 1, 2 };
                    int[, ] combs =
                    {
                        {0, 1, 2},
                        {2, 1, 0},
                        {1, 2, 0},
                    };


                    if (k == 0)
                    {
                        verticeType[v[0]] = vertice[j_x, i_z].GetComponent<Vertex>().type;
                        verticeType[v[1]] = vertice[Mathf.Min(j_x + 1, x - 1), i_z].GetComponent<Vertex>().type;
                        verticeType[v[2]] = vertice[Mathf.Min(j_x + i_z % 2, x - 1), Mathf.Min(i_z + 1, z - 1)].GetComponent<Vertex>().type;
                    }
                    else if (k == 1)
                    {
                        verticeType[v[0]] = vertice[j_x + 1 - i_z % 2, i_z].GetComponent<Vertex>().type;
                        verticeType[v[1]] = vertice[j_x, i_z + 1].GetComponent<Vertex>().type;
                        verticeType[v[2]] = vertice[j_x + 1, Mathf.Min(i_z + 1, z - 1)].GetComponent<Vertex>().type;
                    }

                    Vertex.VertexType[] tmp = { verticeType[0], verticeType[1], verticeType[2] };

                    int cnt = 0;
                    while (!IsCombination(verticeType) && cnt < combs.GetLength(0))
                    {
                        verticeType[0] = tmp[combs[cnt, 0]];
                        verticeType[1] = tmp[combs[cnt, 1]];
                        verticeType[2] = tmp[combs[cnt, 2]];

                        v[0] = combs[cnt, 0];
                        v[1] = combs[cnt, 1];
                        v[2] = combs[cnt, 2];

                        cnt++;
                    }

                    if (cnt==combs.GetLength(0))
                    {
                        //Debug.Log("fuck");
                    }

                    //Debug.Log(verticeType[0].ToString() + verticeType[1].ToString() + verticeType[2]);

                    //정점:좌하-우하-상중
                    if (k == 0)
                    {
                        cellVertice[v[0]] = vertice[j_x, i_z].transform.position;
                        cellVertice[v[1]] = vertice[Mathf.Min(j_x + 1, x - 1), i_z].transform.position;
                        cellVertice[v[2]] = vertice[Mathf.Min(j_x + i_z % 2, x - 1), Mathf.Min(i_z + 1, z - 1)].transform.position;
                    }
                    if (k == 1)
                    {
                        cellVertice[v[0]] = vertice[j_x + 1 - i_z % 2, i_z].transform.position;
                        cellVertice[v[1]] = vertice[j_x, i_z + 1].transform.position;
                        cellVertice[v[2]] = vertice[j_x + 1, Mathf.Min(i_z + 1, z - 1)].transform.position;
                    }

                    

                    int[] triangle = { 0, 1, 2 };

                    Vector2[] uvs = new Vector2[] { new Vector2(0f, 0f),
                                                new Vector2(1f, 0f),
                                                new Vector2(0.5f,  0.5f * Mathf.Sqrt(3))}; //0.5f, 0.5f*sqrt(3)

                    Mesh mesh = new Mesh();
                    mesh.vertices = cellVertice;
                    mesh.triangles = triangle;
                    mesh.uv = uvs;

                    cells[j_x * 2 + k, i_z] = Instantiate(cellPref, cellParent.transform);
                    cells[j_x * 2 + k, i_z].GetComponent<MeshFilter>().mesh = mesh;
                    cells[j_x * 2 + k, i_z].GetComponent<Cell>().vertices = cellVertice;

                    for(int l = 0; l < 3; l ++)
                     cells[j_x * 2 + k, i_z].GetComponent<Cell>().SetVerticeType(l, verticeType[l]);
                }
            }
        }
    }

    void SetMaterial(bool isDebug = false)
    { 
        foreach (GameObject obj in cells)
        {
            Cell cell = obj.GetComponent<Cell>();
            if (isDebug) cell.SetMaterial(materials[4]);
            else cell.SetMaterial();
        }
    }
}
