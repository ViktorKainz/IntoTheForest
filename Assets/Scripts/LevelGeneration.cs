using System;
using System.Collections.Generic;
using System.Linq;
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
    public Vector3 size = new Vector3(200, 100, 200);
    public GameObject player;
    public TerrainField selected;

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
        int minDist = levelSize / numberCastles + levelSize / 10;
        Boolean insert;
        for (int i = 0; i < numberCastles; i++)
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
                    level[x, z].rotation = Quaternion.Euler(new Vector3(0, Random.Range(1, 4) * 90, 0));
                    Vector2 castleLoc = new Vector2(x, z);
                    castleCoord.Add(castleLoc);
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
                        level[x, z].rotation = Quaternion.Euler(0, Random.Range(1, 4) * 90, 0);
                    }
                    else if (sample <= 0.45f)
                    {
                        level[x, z].plan = terrain.plain;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1, 4) * 90, 0);
                    }
                    else if (sample <= 0.60f)
                    {
                        level[x, z].plan = terrain.forest;
                        level[x, z].rotation = Quaternion.Euler(0, Random.Range(1, 4) * 90, 0);
                    }
                    else if (sample <= 0.70f)
                    {
                        level[x, z].plan = terrain.desert;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1, 4) * 90, 0);
                    }
                    else
                    {
                        level[x, z].plan = terrain.mountain;
                        level[x, z].rotation = Quaternion.Euler(-90, Random.Range(1, 4) * 90, 0);
                    }
                }
                level[x, z].obj = Instantiate(level[x, z].plan, new Vector3(x * size.x, 0, z * size.z),
                    level[x, z].plan.transform.rotation);
                level[x, z].obj.transform.rotation = level[x, z].rotation;
                level[x, z].field = level[x, z].obj.GetComponent<TerrainField>();
                level[x, z].field.x = x;
                level[x, z].field.y = z;
                level[x, z].field.level = this;
            }
        }

        spawnPlayers(castleCoord);
    }

    void spawnPlayers(List<Vector2> castleLoc)
    {
        float maxDistance = 0;
        Vector2 spawnAtCastle1 = new Vector2(0, 0);
        Vector2 spawnAtCastle2 = new Vector2(0, 0);
        foreach (Vector2 castle1 in castleLoc)
        {
            foreach (Vector2 castle2 in castleLoc)
            {
                float currentDistance = Vector2.Distance(castle1, castle2);
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    spawnAtCastle1 = castle1;
                    spawnAtCastle2 = castle2;
                }
            }
        }

        GameObject castleFlag =
            level[(int) spawnAtCastle1.x, (int) spawnAtCastle1.y].obj.transform.Find("Flag").gameObject;
        castleFlag.GetComponent<Renderer>().material.color = Color.red;

        castleFlag = level[(int) spawnAtCastle1.x, (int) spawnAtCastle1.y].obj.transform.Find("Flag").gameObject;
        castleFlag.GetComponent<Renderer>().material.color = Color.green;

        spawnAtCastle1 = randomSpawnOffset(spawnAtCastle1);

        level[(int) spawnAtCastle1.x, (int) spawnAtCastle1.y].field.figure = player;

        spawnAtCastle2 = randomSpawnOffset(spawnAtCastle2);
        level[(int) spawnAtCastle2.x, (int) spawnAtCastle2.y].field.figure = player;
    }

    private Vector2 randomSpawnOffset(Vector2 position)
    {
        foreach (int option in Enumerable.Range(0, 4).OrderBy(x => Random.Range(0, 4)))
        {
            switch (option)
            {
                case 0:
                    if (position.x > 0) return new Vector2(position.x - 1, position.y);
                    continue;
                case 1:
                    if (position.y > 0) return new Vector2(position.x, position.y - 1);
                    continue;
                case 2:
                    if (position.x < levelSize - 1) return new Vector2(position.x + 1, position.y);
                    continue;
                case 3:
                    if (position.y < levelSize - 1) return new Vector2(position.x, position.y + 1);
                    continue;
            }
        }

        return position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}