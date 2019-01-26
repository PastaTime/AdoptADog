using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerStartButton : MonoBehaviour
{
    public PlayerStartManager manager;
    public KeyCode buttonKey = KeyCode.A;
    public bool buttonSelected = false;
    
    public float pulseTime = 1f;
    private float _currentTime = 0f;
    
    
    // Used for flashing the button.
    private bool _buttonFlashState = true;
    private Text _buttonText;

    void Start()
    {
        _buttonText = GetComponent<Text>();
        manager = FindObjectOfType<PlayerStartManager>();
        manager.Register(this);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected)
        {
            return;
        }
        
        if (Input.GetKeyDown(buttonKey))
        {
            buttonSelected = true;
            selectButton();
        }
        else
        {
            pulseButton();
        }
    }

    public bool PlayerReady()
    {
        return buttonSelected;
    }
    

    private void pulseButton()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > pulseTime)
        {
            _currentTime = 0f;
            toggleButton();
        }
    }

    private void toggleButton()
    {
        _buttonFlashState = !_buttonFlashState;

        if (_buttonFlashState)
        {
            _buttonText.text = "Press A";
        }
        else
        {
            _buttonText.text = "-------";
        }
    }

    private void selectButton()
    {
        _buttonText.text = "Selected.";
    }
}
