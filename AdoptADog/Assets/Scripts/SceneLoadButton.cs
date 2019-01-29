using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

[RequireComponent(typeof(PulseButton))]
public class SceneLoadButton : MonoBehaviour
{
    public enum ButtonType
    {
        A, X
    }

    public string sceneName;
    public ButtonType butonType = ButtonType.A;

    private ControllerManager _controllerManager;

    private void Start()
    {
        _controllerManager = FindObjectOfType<ControllerManager>();
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if ((butonType == ButtonType.A && _controllerManager.GetADown((PlayerIndex) i))
                 || (butonType == ButtonType.X && _controllerManager.GetXDown((PlayerIndex) i)))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}