using System;
using System.Collections;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static TerrainField;
using Button = UnityEngine.UI.Button;

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
        public GameObject roundError;
        public GameObject costError;
        
        public Boolean allowSpawn = true;

        public GameObject castle;
        
        public static int pointsGreen = 1;
        public static int pointsRed = 0;
        
        private Vector3 size = new Vector3(200, 100, 200);
        private int oldRound = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            closeBt.onClick.AddListener(() => gameObject.SetActive(false));
            queenBt.onClick.AddListener(() => spawnRandomAroundCastle(queenPrefab, 5));
            knightBt.onClick.AddListener(() => spawnRandomAroundCastle(knightPrefab, 3));
            pawnBt.onClick.AddListener(() => spawnRandomAroundCastle(pawnPrefab, 1));
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (oldRound != round)
            {
                oldRound = round;
                allowSpawn = true;
            }
        }

        void spawnRandomAroundCastle(GameObject spawnFigure, int cost)
        {
            Debug.Log("Green: " + pointsGreen + ", Red: " + pointsRed);
            if (round % 2 == 1)
            {
                if (pointsGreen < cost)
                {
                    showCostError();
                    return;
                }
                pointsGreen -= cost;
                UpdateCurrentPlayerInfo();
            }
            else
            {
                if (pointsRed < cost)
                {
                    showCostError();
                    return;
                }
                pointsRed -= cost;
                UpdateCurrentPlayerInfo();
            }
            
            if (allowSpawn)
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

                allowSpawn = false;
                roundError.SetActive(true);
                gameObject.SetActive(false);
            }
            
        }

        public void showCostError()
        {
            costError.SetActive(true);
            StartCoroutine(closeCostErrorAfterTime(3));
        }
        
        public IEnumerator closeCostErrorAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            costError.SetActive(false);
        }
    }
}