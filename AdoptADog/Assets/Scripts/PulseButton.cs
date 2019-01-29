using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public delegate void ButtonClickedHandler();

[RequireComponent(typeof(Image))]
public class PulseButton : MonoBehaviour
{
    public Sprite buttonUpImage;
    public Sprite buttonDownImage;
    public Sprite buttonSelectedImage;

    public float pulseTime = 1f;
    public bool Selected { get; set; }

    private bool _buttonFlashState = true;
    private Image _button;

    private float _currentTime = 0f;

    private void Start()
    {
        _button = GetComponent<Image>();
    }

    private void Update()
    {
        if (!Selected) DoPulse();
    }

    private void DoPulse()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > pulseTime)
        {
            _currentTime = 0f;
            ToggleButton();
        }
    }

    private void ToggleButton()
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



}
