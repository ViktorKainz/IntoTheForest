using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SpawnFigureCanvas : MonoBehaviour
    {
        public Button closeBt;
        public Button queenBt;
        public Button knightBt;
        public Button pawnBt;

        public GameObject castle;
        
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
            closeBt.onClick.AddListener(() => gameObject.SetActive(false));
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
    }
}