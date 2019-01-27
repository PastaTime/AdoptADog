using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerReadyButton : MonoBehaviour
{
    public bool buttonSelected = false;
    public Dog dog;
    
    public float pulseTime = 1f;
    
    public Sprite buttonUpImage;
    public Sprite buttonDownImage;
    public Sprite buttonSelectedImage;
    
    private float _currentTime = 0f;
    
    // Used for flashing the button.
    private bool _buttonFlashState = true;
    private Image _button;

    void Start()
    {
        _button = GetComponent<Image>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected)
        {
            return;
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

    public void SetReady()
    {
        buttonSelected = true;
        dog.Posing = true;
        selectButton();
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
            _button.sprite = buttonUpImage;
        }
        else
        {
            _button.sprite = buttonDownImage;
        }
    }

    private void selectButton()
    {
        _button.sprite = buttonSelectedImage;
    }
}
