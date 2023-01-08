using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonImage : MonoBehaviour
{
    private Image _image;
    protected Image Image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }

    private Button _button;
    protected Button Button { get { if (_button == null) _button = GetComponentInParent<Button>(); return _button; } }

    private Color defaultColor;

    private void Start()
    {
        defaultColor = Image.color;
    }

    private void Update()
    {
        if (Button)
            Image.color = Button.interactable ? defaultColor : defaultColor.Multiply(alpha: Button.colors.disabledColor.a);
    }
}

