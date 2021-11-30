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
}
public class LevelGeneration : MonoBehaviour
{
    private Field[,] level;
    private Terrains terrain;
    public int levelSize;
    public float noiseScale = 1.1f;
    public float noiseSeed;
    public int numberCastles;

    
    // Start is called before the first frame update
    void Start()
    {
        terrain = gameObject.GetComponent<Terrains>();
        GenerateLevel();
        Vector3 size = terrain.plain.GetComponent<Renderer>().bounds.size;
        Vector3 center = new Vector3(levelSize / 2f * size.x, 900, levelSize / 2f * size.z);
        Camera.main.transform.position = center;
        Camera.main.transform.rotation = Quaternion.Euler (60, 0, 0);
        center.y = 0;
        Vector3 b = center;
        b *= 2.5f;
        Camera.main.GetComponent<CameraMovement>().bounds = new Bounds(center, b);
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
                    }
                    else if (sample <= 0.45f)
                    {
                        level[x, z].plan = terrain.plain;
                    }
                    else if (sample <= 0.60f)
                    {
                        level[x, z].plan = terrain.forest;
                    }
                    else if (sample <= 0.70f)
                    {
                        level[x, z].plan = terrain.desert;
                    }
                    else
                    {
                        level[x, z].plan = terrain.mountain;
                    }
                }
                var size = new Vector3(200,100,200);
                //ToDo add working rotation on y-axis dont change prefab
                // level[x, z].plan.transform.rotation = Quaternion.Euler(new Vector3(level[x, z].plan.transform.rotation.x, Random.Range(1,4)*90 , level[x, z].plan.transform.rotation.z));
                // level[x, z].obj = Instantiate(level[x, z].plan, new Vector3(x * size.x, 0, z * size.z), Quaternion.Euler(new Vector3(level[x, z].plan.transform.rotation.x, Random.Range(1,4)*90 , level[x, z].plan.transform.rotation.z)));
                level[x, z].obj = Instantiate(level[x, z].plan, new Vector3(x * size.x, 0, z * size.z), level[x, z].plan.transform.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
