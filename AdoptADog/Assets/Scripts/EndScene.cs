    using UnityEngine;

    public class EndScene : MonoBehaviour
    {
        public GameObject playerPrefab;
        private Dog _winDog;
        public RuntimeAnimatorController[] animationControllers;
        private ControllerManager _controllerManager;

        private void Start()
        {
            FindObjectOfType<ControllerManager>().Enabled = true;
            GameObject player = Instantiate(playerPrefab);
            player.name = "Player" + (GameState.WinningPlayer);
            var playerController = player.GetComponent<PlayerController>();
            playerController.playerNumber = GameState.WinningPlayer;
            var animator = player.GetComponent<Animator>();
            animator.runtimeAnimatorController = animationControllers[(int) GameState.WinningPlayer];
            _winDog = player.GetComponent<Dog>();
            player.transform.localPosition = new Vector2(-2.37f, 1.13f);
            player.transform.localScale = new Vector2(0.7f, 0.7f);
        }

        private void Update()
        {
            _winDog.Pose(false, true);
        }
    }
