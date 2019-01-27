using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public RuntimeAnimatorController[] animationControllers;
        public GameObject playerPrefab;

        public List<PlayerReadyButton> buttonList = new List<PlayerReadyButton>();
        private HashSet<int> _selectedControllers = new HashSet<int>();
        private bool _started = false;

        public PlayerReadyButton StartButton;

        public string MapSceneName = "Map";
        public string EndSceneName = "End";
        public string ReadySceneName = "Ready";
        public string StartSceneName = "Start";

        private int _winningPlayer;

        private string _sceneName;

        public static GameManager Instance { get; private set; }
        private AudioManager _audioManager;


        private void AddPlayer(int index)
        {
            _selectedControllers.Add(index);
            buttonList[index].SetReady();
        }

        void Update()
        {
            if (_sceneName == ReadySceneName)
            {
                if (buttonList != null && buttonList.Count(b => b.PlayerReady()) > 1)
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

            if (_sceneName == StartSceneName)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Controller.getSingleton().getA(i + 1))
                    {
                        Ready();
                    }

                }
            }

            if (_sceneName == EndSceneName)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Controller.getSingleton().getA(i + 1))
                    {
                        Ready();
                    }

                }
            }
        }

        private void LoadSceneByName(string name)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            SceneManager.LoadScene(name);
        }


        public void Ready()
        {
            LoadSceneByName(ReadySceneName);
        }

        private void StartGame()
        {
            _started = true;

            LoadSceneByName(MapSceneName);
        }

        public void FinishGame(int playerNum)
        {
            _audioManager.BackgroundMusic(false);
            _audioManager.PlayAudio(_audioManager.playerVictory);
            _winningPlayer = playerNum;
            StartCoroutine(EndRoutine());
        }

        private IEnumerator EndRoutine()
        {
            yield return new WaitForSeconds(1);
            LoadSceneByName(EndSceneName);
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            _sceneName = StartSceneName;

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                _sceneName = scene.name;
                _audioManager = FindObjectOfType<AudioManager>();

                if (scene.name == ReadySceneName)
                {
                    buttonList.Clear();
                    _audioManager.BackgroundMusic(true);
                }

                if (scene.name == MapSceneName)
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
                }

                if (scene.name == EndSceneName)
                {
                    Controller.getSingleton().enabled = true;
                    GameObject player = Instantiate(playerPrefab);
                    player.name = "Player" + (_winningPlayer);
                    var playerController = player.GetComponent<PlayerController>();
                    playerController.playerNumber = _winningPlayer;
                    var animator = player.GetComponent<Animator>();
                    animator.runtimeAnimatorController = animationControllers[_winningPlayer - 1];
                    animator.SetBool("Posing", true);
                    player.transform.localPosition = new Vector2(-2.37f, 1.13f);
                    player.transform.localScale = new Vector2(0.7f, 0.7f);
                }

                if (scene.name == ReadySceneName)
                {
                    StartButton = FindObjectsOfType<PlayerReadyButton>().Single(b => b.name == "StartButton");
                    buttonList.AddRange(FindObjectsOfType<PlayerReadyButton>()
                        .Where(b => b.name.StartsWith("Player")));
                    buttonList.Sort((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
                }

            };
        }

    }
}