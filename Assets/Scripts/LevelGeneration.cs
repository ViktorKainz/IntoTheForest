using System.Linq;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        terrain = gameObject.GetComponent<Terrains>();
        GenerateLevel();
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        var size = terrain.plain.GetComponent<Renderer>().bounds.size;
        camera.transform.position = new Vector3(levelSize / 2 * size.x, 900, levelSize / 2 * size.z);
        camera.transform.rotation = Quaternion.Euler (90, 0, 0);
    }

    public void GenerateLevel()
    {
        level = new Field[levelSize, levelSize];
        if (noiseSeed == 0)
        {
            noiseSeed = Random.value * noiseScale;
        }
        foreach (int z in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
        {
            foreach (int x in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
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
                var size = level[x, z].plan.GetComponent<Renderer>().bounds.size;
                level[x, z].obj = Instantiate(level[x, z].plan, new Vector3(x * size.x, 0, z * size.z), level[x, z].plan.transform.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
