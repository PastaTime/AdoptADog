using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerReadyButton : MonoBehaviour
{
    public AudioManager audioManager;
    public PlayerReadyManager playerManager;
    public KeyCode buttonKey = KeyCode.A;
    public bool buttonSelected = false;
    
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
        playerManager = FindObjectOfType<PlayerReadyManager>();
        playerManager.Register(this);
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
        audioManager.PlayAudio(audioManager.playerReady);
    }
}
