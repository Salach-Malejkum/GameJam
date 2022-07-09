using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;
    public float perlinX = .3f;
    public float perlinZ = .3f;
    public float perlinMultiplier = 2f;
    public Gradient gradient;
    public int seed = 1000;

    public GameObject[] treeModels;
    public float upperTreeThreshold;
    public float lowerTreeThreshold;
    public float perlinSpawnX;
    public float perlinSpawnZ;

    public GameObject[] rockModels;
    public float upperRockThreshold;
    public float lowerRockThreshold;

    public GameObject waterModel;
    public float waterLevel;

    float minTerrainHeight;
    float maxTerrainHeight;

    
    GameObject[] trees;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        GameObject water = Instantiate(waterModel, new Vector3(0, waterLevel, 0), Quaternion.identity);
        water.transform.localScale = new Vector3(xSize/4, 1, zSize/4);
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;

        for(int z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((x + seed) * perlinX, (z + seed) * perlinZ) * perlinMultiplier;

                if(x == xSize / 2)
                {
                    y = -2f;
                }
                

                if(y > maxTerrainHeight)
                    maxTerrainHeight = y;
                
                if(y < minTerrainHeight)
                    minTerrainHeight = y;
                
                vertices[i] = new Vector3(x, y, z);

                float spawnNoise = Mathf.PerlinNoise((x + seed) * perlinSpawnX, (z + seed) * perlinSpawnZ);
                if((x < xSize/2 - 3 || x > xSize/2 + 3))
                {
                    if(spawnNoise < upperTreeThreshold && spawnNoise > lowerTreeThreshold)
                    {
                        float whatToSpawn = Mathf.PerlinNoise(x + (seed * 5), z + (seed * 5));

                        whatToSpawn = whatToSpawn * treeModels.Length;
                        whatToSpawn = Mathf.RoundToInt(whatToSpawn);
                        GameObject tree = Instantiate(treeModels[(int)whatToSpawn], new Vector3(x, y + 1f, z), Quaternion.identity);
                        tree.transform.parent = transform;
                    }

                    if(spawnNoise < upperRockThreshold && spawnNoise > lowerRockThreshold)
                    {
                        float whatToSpawn = Mathf.PerlinNoise(x + (seed * 5), z + (seed * 5));

                        whatToSpawn = whatToSpawn * rockModels.Length;
                        whatToSpawn = Mathf.RoundToInt(whatToSpawn);
                        GameObject rock = Instantiate(rockModels[(int)whatToSpawn], new Vector3(x, y + 1f, z), Quaternion.identity);
                        rock.transform.parent = transform;
                    }
                }
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vertice = 0;
        int triangle = 0;
        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[triangle] = vertice;
                triangles[triangle + 1] = vertice + xSize + 1;
                triangles[triangle + 2] = vertice + 1;
                triangles[triangle + 3] = vertice + 1;
                triangles[triangle + 4] = vertice + xSize + 1;
                triangles[triangle + 5] = vertice + xSize + 2;

                vertice++;
                triangle += 6;
            }

            vertice++;
        }

        colors = new Color[vertices.Length];

        i = 0;

        for(int z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight,  vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
}
