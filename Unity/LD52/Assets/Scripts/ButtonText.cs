using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class ButtonText : MonoBehaviour
{
    private TMP_Text _tmp;
    protected TMP_Text Tmp { get { if (_tmp == null) _tmp = GetComponent<TMP_Text>(); return _tmp; } }

    private Button _button;
    protected Button Button { get { if (_button == null) _button = GetComponentInParent<Button>(); return _button; } }

    private Color defaultColor;

    private void Start()
    {
        defaultColor = Tmp.color;
    }

    private void Update()
    {
        if (Button)
            Tmp.color = Button.interactable ? defaultColor : defaultColor.Multiply(alpha: Button.colors.disabledColor.a);
    }
}
