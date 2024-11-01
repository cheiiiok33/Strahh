using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text noteTextUI;
    public GameObject notePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (notePanel == null || noteTextUI == null)
        {
            Debug.LogError("Note Panel or Note Text UI not assigned in UIManager!");
        }

        if (notePanel != null)
        {
            notePanel.SetActive(false);
        }
    }

    public void DisplayNoteText(string text)
    {
        Debug.Log("Displaying note text: " + text); // Для отладки
        if (noteTextUI != null && notePanel != null)
        {
            noteTextUI.text = text;
            notePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Note UI components not properly set up!");
        }
    }

    public void HideNoteText()
    {
        if (notePanel != null)
        {
            notePanel.SetActive(false);
        }
    }
}