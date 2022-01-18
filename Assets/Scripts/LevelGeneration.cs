using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Field
{
    public GameObject obj;
    public GameObject plan;
    public Quaternion rotation;
    public TerrainField field;
}
public class LevelGeneration : MonoBehaviour
{
    private Field[,] level;
    private Terrains terrain;
    public int levelSize;
    public float noiseScale = 1.1f;
    public float noiseSeed;
    public int numberCastles;
    public Vector3 size = new Vector3(200,100,200);

    
    // Start is called before the first frame update
    void Start()
    {
        terrain = gameObject.GetComponent<Terrains>();
        GenerateLevel();
        Camera.main.GetComponent<CameraMovement>()
            .Initialize(levelSize, terrain.plain.GetComponent<Renderer>().bounds.size.x);
    }

    public void GenerateLevel()
    {
        level = new Field[levelSize, levelSize];
        if (noiseSeed == 0)
        {
            noiseSeed = Random.value * noiseScale;
        }

        List<Vector2> castleCoord = new List<Vector2>();
        int minDist = levelSize / numberCastles + levelSize/10;
        Boolean insert;
        for(int i = 0; i < numberCastles; i++)
        {
            int x = Random.Range(0, levelSize);
            int z = Random.Range(0, levelSize);
            insert = true;
            if (level[x, z].plan == null)
            {
                foreach (Vector2 coords in castleCoord)
                {
                    float difX = Math.Abs(coords.x - x);
                    float difY = Math.Abs(coords.y - z);
                    if (difX < minDist && difY < minDist)
                    {
                        insert = false;
                    }
                }
                
                if (insert)
                {
                    level[x, z].plan = terrain.castle;
                    level[x, z].rotation = Quaternion.Euler(new Vector3(0, Random.Range(1,4)*90 , 0));
                    castleCoord.Add(new Vector2(x, z));
                }
                else
                {
                    i--;
                }
            }
            else
            {
                i--;
            }

        }

        foreach (int z in Enumerable.Range(0, levelSize))
        {
            foreach (int x in Enumerable.Range(0, levelSize))
            {
                if (level[x, z].plan == null)
                {
                
                    float sample = Mathf.PerlinNoise(x * noiseScale + noiseSeed, z * noiseScale + noiseSeed);
                    
                    if (sample <= 0.20f)
                    {
                        level[x, z].plan = terrain.water;
                        level[x, z].rotation = Quaternion.Euler(0, Random.Range(1,4)*90 , 0);
                    }
                    else if (sample <= 0.45f)
                    {
                        level[x, z].plan = terrain.plain;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1,4)*90, 0);
                    }
                    else if (sample <= 0.60f)
                    {
                        level[x, z].plan = terrain.forest;
                        level[x, z].rotation = Quaternion.Euler(0, Random.Range(1,4)*90 , 0);
                    }
                    else if (sample <= 0.70f)
                    {
                        level[x, z].plan = terrain.desert;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1,4)*90, 0);
                    }
                    else
                    {
                        level[x, z].plan = terrain.mountain;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1,4)*90, 0);
                    }
                }
                var size = new Vector3(200,100,200);
                level[x, z].obj = Instantiate(level[x, z].plan, new Vector3(x * size.x, 0, z * size.z), level[x, z].plan.transform.rotation);
                level[x, z].obj.transform.rotation = level[x, z].rotation;
                level[x, z].field = level[x, z].obj.GetComponent<TerrainField>();
                level[x, z].field.x = x;
                level[x, z].field.y = z;
                level[x, z].field.level = this;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
