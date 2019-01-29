using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayerReadyManager : MonoBehaviour
    {
        private List<PlayerReadyButton> _buttons = new List<PlayerReadyButton>();
        public GameObject startButton;
        private AudioManager _audioManager;

        public void Register(PlayerReadyButton button)
        {
            _buttons.Add(button);
        }

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _audioManager.BackgroundMusic(true);
        }

        private void Update()
        {
            int readyPlayers = _buttons.Count(b => b.PlayerReady);

            if (readyPlayers <= 1) return;

            startButton.SetActive(true);

            for (int i = 0; i < 4; i++)
            {
                if (Controller.GetSingleton().GetXDown(i))
                {
                    SceneManager.LoadScene(Constants.MapSceneName);
                }
            }

        }
    }
}