using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureCreator : MonoSingleton<TextureCreator>
{
    public Texture2D grass;
    public Image image;


    List<Texture2D> createdTextures;

    void Start()
    {
        createdTextures = new List<Texture2D>();

        CreateNoisedTextureFrom(grass);
        CreatePNG(createdTextures);
    }

    void CreateNoisedTextureFrom(Texture2D texture)
    {
        Color[,] colors = new Color[grass.width, grass.height]; //64x64

        for(int i_y = 0; i_y < grass.height; i_y++)
        {
            for(int j_x = 0; j_x < grass.width; j_x++)
            {
                colors[j_x, i_y] = grass.GetPixel(j_x, i_y);
            }
        }

        int[,] triangles = new int[colors.GetLength(0), colors.GetLength(1)];

        for (int i_y = 0; i_y < triangles.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < triangles.GetLength(1); j_x++)
            {
                triangles[j_x, i_y] = -1;
                if (colors[j_x, i_y].a != 0) triangles[j_x, i_y] = 0;
            }
        }

        for (int i_y = 0; i_y < triangles.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < triangles.GetLength(1); j_x++)
            {
                if (triangles[j_x, i_y] == -1) continue;
                //좌 == 1
                if (triangles[Mathf.Max(0, j_x - 1), i_y] == -1 || 
                    triangles[Mathf.Max(0, j_x - 2), i_y] == -1 || 
                    triangles[Mathf.Max(0, j_x - 3), i_y] == -1)
                {
                    triangles[j_x, i_y] = 1;
                }
                //우 == 2
                if (triangles[Mathf.Min(63, j_x + 1), i_y] == -1 || 
                    triangles[Mathf.Min(63, j_x + 2), i_y] == -1 || 
                    triangles[Mathf.Min(63, j_x + 3), i_y] == -1)
                {
                    triangles[j_x, i_y] = 2;
                }
                //하 == 3
                if (i_y <= 2)
                {
                    triangles[j_x, i_y] = 3;
                }
            }
        }

        for (int i_y = 0; i_y < colors.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < colors.GetLength(1); j_x++)
            {
                if (triangles[j_x, i_y] == 0)
                {
                    float scale = Mathf.PerlinNoise(j_x * 0.05f, i_y * 0.05f);
                    colors[j_x, i_y] = new Color(0, 1 - scale / 2.0f, 0, 1);
                }
                if (triangles[j_x, i_y] == 1)
                    colors[j_x, i_y] = Color.red;
                if (triangles[j_x, i_y] == 2)
                    colors[j_x, i_y] = Color.blue;
                if (triangles[j_x, i_y] == 3)
                    colors[j_x, i_y] = Color.yellow;
            }
        }

        Texture2D newTexture = new Texture2D(colors.GetLength(0), colors.GetLength(1));
        for (int i_y = 0; i_y < colors.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < colors.GetLength(1); j_x++)
            {
                newTexture.SetPixel(j_x, i_y, colors[j_x, i_y]);
            }
        }

        createdTextures.Add(newTexture);
    }

    public Texture2D GetNoisedTextureFrom(Texture2D texture)
    {
        Color[,] originalColors = new Color[texture.width, texture.height];

        for (int i_y = 0; i_y < texture.height; i_y++)
        {
            for (int j_x = 0; j_x < texture.width; j_x++)
            {
                originalColors[j_x, i_y] = texture.GetPixel(j_x, i_y);
            }
        }

        Color[,] colors = new Color[64, 64]; //64x64

        for (int i_y = 0; i_y < 64; i_y++)
        {
            for (int j_x = 0; j_x < 64; j_x++)
            {
                colors[j_x, i_y] = Color.green;
            }
        }

        int[,] triangles = new int[colors.GetLength(0), colors.GetLength(1)];

        for (int i_y = 0; i_y < triangles.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < triangles.GetLength(1); j_x++)
            {
                triangles[j_x, i_y] = -1;
                if (colors[j_x, i_y].a != 0) triangles[j_x, i_y] = 0;
            }
        }

        for (int i_y = 0; i_y < triangles.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < triangles.GetLength(1); j_x++)
            {
                if (triangles[j_x, i_y] == -1) continue;
                //좌 == 1
                if (triangles[Mathf.Max(0, j_x - 1), i_y] == -1 ||
                    triangles[Mathf.Max(0, j_x - 2), i_y] == -1 ||
                    triangles[Mathf.Max(0, j_x - 3), i_y] == -1)
                {
                    triangles[j_x, i_y] = 1;
                }
                //우 == 2
                if (triangles[Mathf.Min(63, j_x + 1), i_y] == -1 ||
                    triangles[Mathf.Min(63, j_x + 2), i_y] == -1 ||
                    triangles[Mathf.Min(63, j_x + 3), i_y] == -1)
                {
                    triangles[j_x, i_y] = 2;
                }
                //하 == 3
                if (i_y <= 2)
                {
                    triangles[j_x, i_y] = 3;
                }
            }
        }

        for (int i_y = 0; i_y < colors.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < colors.GetLength(1); j_x++)
            {
                Vector2 offset = new Vector2(Random.Range(0, 64), Random.Range(0, 64));

                float scale = Mathf.PerlinNoise((j_x+ offset.x) * 0.05f, (i_y+ offset.y) * 0.05f);

                colors[j_x, i_y] = new Color
                (
                    originalColors[j_x, i_y].r - scale / 2.0f,
                    originalColors[j_x, i_y].g - scale / 2.0f,
                    originalColors[j_x, i_y].b,
                    originalColors[j_x, i_y].a);
            }
        }

        Texture2D newTexture = new Texture2D(colors.GetLength(0), colors.GetLength(1));
        for (int i_y = 0; i_y < colors.GetLength(0); i_y++)
        {
            for (int j_x = 0; j_x < colors.GetLength(1); j_x++)
            {
                newTexture.SetPixel(j_x, i_y, colors[j_x, i_y]);
            }
        }

        newTexture.Apply();


        Rect rect = new Rect(0, 0, 64, 64);
        image.sprite = Sprite.Create(newTexture, rect, new Vector2(0.5f, 0.5f));

        return newTexture;
    }

    void CreatePNG(List<Texture2D> textures)
    {
        for(int i = 0; i < textures.Count; i++)
        {
            byte[] bytes = textures[i].EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Textures/newimg_" + i.ToString() + ".png", bytes);
        }
    }
}

//TODO: 각 테두리 별 fixed 펄린노이즈 (boundary = 2)