using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public enum ButtonType
    {
        OK = 1 << 0,
        Next = 1 << 1,
        Cancel = 1 << 2,
        Close = 1 << 3
    }

    public event UnityEventHandler<int> OnButtonClick;

    private bool initialized = false;

    private Button ok_button;
    private Button next_button;
    private Button cancel_button;
    private Button close_button;

    private const string textObjectName = "text";

    private void Initialize()
    {
        ok_button = gameObject.GetElement<Button>("ok");
        next_button = gameObject.GetElement<Button>("next");
        cancel_button = gameObject.GetElement<Button>("cancel");
        close_button = gameObject.GetElement<Button>("close");

        if (ok_button == null || next_button == null || cancel_button == null || close_button == null)
        {
            Debug.LogError("Some button hasn't find", this);
            return;
        }

        ok_button.onClick.AddListener(() => OnButtonClick(this, (int)ButtonType.OK));
        next_button.onClick.AddListener(() => OnButtonClick(this, (int)ButtonType.Next));
        cancel_button.onClick.AddListener(() => OnButtonClick(this, (int)ButtonType.Cancel));
        close_button.onClick.AddListener(() => OnButtonClick(this, (int)ButtonType.Close));

        initialized = true;

        gameObject.SetActive(false);
    }

    public void Show(string Text, int buttons = 0)
    {
        if (!initialized)
        {
            Initialize();
        }

        var text = gameObject.GetElement<Text>(textObjectName);

        text.text = Text;

        ok_button.gameObject.SetActive(IsButton(buttons, ButtonType.OK));
        next_button.gameObject.SetActive(IsButton(buttons, ButtonType.Next));
        cancel_button.gameObject.SetActive(IsButton(buttons, ButtonType.Cancel));
        close_button.gameObject.SetActive(IsButton(buttons, ButtonType.Close));

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public static bool IsButton(int buttons, ButtonType button)
    {
        return (buttons & (int)button) == (int)button;
    }
}
