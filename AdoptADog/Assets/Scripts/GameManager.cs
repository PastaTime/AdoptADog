using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public RuntimeAnimatorController[] animationControllers;
        private int _count = -1;
        public GameObject playerPrefab;

        public List<PlayerReadyButton> buttonList = new List<PlayerReadyButton>();
        private HashSet<int> _selectedControllers = new HashSet<int>();
        private bool _started = false;

        public PlayerReadyButton StartButton;

        private void AddPlayer(int index)
        {
            _selectedControllers.Add(index);
            buttonList[index].SetReady();
        }

        void Update()
        {
            if (_started) return;

            if (buttonList.Count(b => b.PlayerReady()) > 1)
            {
                StartButton.gameObject.SetActive(true);
            }
            else
            {
                StartButton.gameObject.SetActive(false);
            }

            for (int i = 0; i < 4; i++)
            {
                if (Controller.getSingleton().getX(i + 1) && buttonList.Count(b => b.PlayerReady()) > 1)
                {
                    StartGame();
                }

                if (_selectedControllers.Contains(i)) continue;

                if (Controller.getSingleton().getA(i + 1))
                {
                    AddPlayer(i);
                }
            }

        }

        private void StartGame()
        {
            _started = true;

            LoadScene(buttonList.Count(b => b.PlayerReady()));
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                foreach (var index in _selectedControllers)
                {
                    GameObject player = Instantiate(playerPrefab);
                    player.name = "Player" + (index + 1);
                    var playerController = player.GetComponent<PlayerController>();
                    playerController.playerNumber = index + 1;
                    var animator = player.GetComponent<Animator>();
                    animator.runtimeAnimatorController = animationControllers[index];
                }
            };
        }

        public void LoadScene(int count)
        {
            SceneManager.LoadScene("reuben test", LoadSceneMode.Single);
            _count = count;
        }

    }
}