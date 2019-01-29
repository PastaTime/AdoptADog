using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PulseButton))]
public class SceneLoadButton : MonoBehaviour
{
    public enum ButtonType
    {
        A, X
    }

    public string sceneName;
    public ButtonType butonType = ButtonType.A;

    private void Update()
    {
        for (int i = 1; i < 5; i++)
        {
            if ((butonType == ButtonType.A && Controller.GetSingleton().GetADown(i))
                 || (butonType == ButtonType.X && Controller.GetSingleton().GetXDown(i)))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}