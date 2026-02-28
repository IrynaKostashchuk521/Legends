using UnityEngine;
using TMPro;

public class DialogWindowController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject PanelWithName;       // панель з іменем персонажа
    public GameObject BackgroundPanel;     // фон для повідомлень / наратив

    [Header("Text Fields")]
    public TextMeshProUGUI TextField;
    public TextMeshProUGUI NameField;

    /// <summary>
    /// Показати діалог
    /// </summary>
    /// <param name="text">Текст репліки або наративу</param>
    /// <param name="characterName">Ім'я персонажа або пусто для наративу</param>
    public void ShowDialog(string text, string characterName = "")
    {
        TextField.text = text;

        bool isNarration = string.IsNullOrEmpty(characterName);

        // Перемикаємо панелі
        PanelWithName.SetActive(!isNarration);
        BackgroundPanel.SetActive(isNarration);

        if (!isNarration)
        {
            NameField.text = characterName;
        }
    }

    /// <summary>
    /// Показати додаткове повідомлення над фоном
    /// </summary>
    /// <param name="message">Текст повідомлення</param>
    public void ShowMessageOnBackground(string message)
    {
        BackgroundPanel.SetActive(true);
        TextField.text = message;
    }

    public void HideAll()
    {
        PanelWithName.SetActive(false);
        BackgroundPanel.SetActive(false);
    }
}