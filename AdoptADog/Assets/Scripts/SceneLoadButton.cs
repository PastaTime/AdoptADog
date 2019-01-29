using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PulseButton))]
public class SceneLoadButton : MonoBehaviour
{
    public string SceneName;

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Controller.GetSingleton().GetADown(i))
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }
}