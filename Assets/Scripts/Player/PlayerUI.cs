using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        panel.alpha = 0;
    }

    public void SetText(string content)
    {
        text.text = content;
    }

    public void ShowInteractionUI()
    {
        panel.alpha = 1;
    }

    public void HideInteractionUI()
    {
        panel.alpha = 0;
    }
}
