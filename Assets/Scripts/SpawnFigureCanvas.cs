using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static TerrainField;

namespace DefaultNamespace
{
    public class SpawnFigureCanvas : MonoBehaviour
    {
        public Button closeBt;
        public Button queenBt;
        public Button knightBt;
        public Button pawnBt;

        public GameObject queenPrefab;
        public GameObject knightPrefab;
        public GameObject pawnPrefab;

        public GameObject castle;
        
        private Vector3 size = new Vector3(200, 100, 200);
        
        // Start is called before the first frame update
        void Start()
        {
            closeBt.onClick.AddListener(() => gameObject.SetActive(false));
            queenBt.onClick.AddListener(() => spawnRandomAroundCastle(queenPrefab));
            knightBt.onClick.AddListener(() => spawnRandomAroundCastle(knightPrefab));
            pawnBt.onClick.AddListener(() => spawnRandomAroundCastle(pawnPrefab));
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void spawnRandomAroundCastle(GameObject spawnFigure)
        {
            Vector2 castlePosition =
                new Vector2(castle.GetComponent<TerrainField>().x, castle.GetComponent<TerrainField>().y);
                Field[,] level = castle.GetComponent<TerrainField>().level.getLevel();
                
                var spawnPos = LevelGeneration.randomSpawnOffset(castlePosition, level);
                if (!spawnPos.Equals(new Vector2(-1, -1)))
                {
                    TerrainField f = level[(int) spawnPos.x, (int) spawnPos.y].field;
                    spawnFigure.GetComponent<GameFigure>().enemy = round % 2 == 0;
                    f.figure = Instantiate(spawnFigure, new Vector3(f.x * size.x, 0, f.y * size.z), Quaternion.Euler(0, 0, 0));
                }
        }
    }
}