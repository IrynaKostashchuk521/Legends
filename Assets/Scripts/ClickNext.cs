using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using System.Linq;

public class ClickNext : MonoBehaviour
{
    private Story _CurrentStory;
    private TextAsset _inkJson;

    [Header("Panels")]
    public GameObject DialoguePanel;      
    public GameObject BackgroundPanel;    
    public TextMeshProUGUI TextField;
    public TextMeshProUGUI NameField;

    [Header("Choices")]
    public GameObject ChoiceButtonPanel;
    public GameObject ChoiceButtonPrefab;

    private List<Character> characters = new();
    private List<TextMeshProUGUI> _ChoiceText = new();

    public bool DialoguePlay { get; private set; }

    [Header("Background Sprites")]
    public Image BackgroundImage; 
    public Sprite forestSprite;   
    public Sprite roomSprite;
    public Sprite numderSprite;
    public Sprite music_roonSprite;
    public Sprite homeSprite;
    public Sprite dark_house;
    public Sprite campSprite;
    public Sprite dark_campSprite;
    public Sprite officeSprite;
    public Sprite boardsSprite;


    [Inject]
    public void Parameters(DialogueInstaller dialogueInstaller)
    {
        _inkJson = dialogueInstaller.inkJson;
        DialoguePanel = dialogueInstaller.DialoguePanel;
        BackgroundPanel = dialogueInstaller.BackgroundDialogPanel;
        TextField = dialogueInstaller.Textdialogue;
        NameField = dialogueInstaller.NameCharacter;
        ChoiceButtonPanel = dialogueInstaller.ChoiceButtonPanel;
        ChoiceButtonPrefab = dialogueInstaller.ChoiceButton;
    }

    private void Awake()
    {
        _CurrentStory = new Story(_inkJson.text);
    }

    void Start()
    {
        characters = FindObjectsByType<Character>(FindObjectsSortMode.None).ToList();
        StartDialog();
    }

    public void StartDialog()
    {
        DialoguePlay = true;
        ContinueStory();
    }

    public void ContinueStory(bool choiceBefore = false)
    {
        if (_CurrentStory.canContinue)
        {
            ShowDialog();
            ShowChoiceButton();
        }
        else if (!choiceBefore)
        {
            ExitDialog();
        }
    }

    private void ShowDialog()
    {
        string text = _CurrentStory.Continue();

        HandleTags();

        string currentName = (string)_CurrentStory.variablesState["NameCharacter"];
        bool isNarration = string.IsNullOrEmpty(currentName);

        DialoguePanel.SetActive(!isNarration);
        BackgroundPanel.SetActive(isNarration);

        foreach (var c in characters)
            c.gameObject.SetActive(false);

        if (isNarration)
        {
            var bgText = BackgroundPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (bgText != null)
                bgText.text = text; 
            return;
        }

        NameField.text = currentName; 
        TextField.text = text;        

        int index = characters.FindIndex(c => c.nameCharacter == currentName);
        if (index == -1)
        {
            Debug.LogWarning($"Character not found: {currentName}");
            return;
        }

        int expression = (int)_CurrentStory.variablesState["characterExpression"];
        characters[index].gameObject.SetActive(true);
        characters[index].ChangeCharacter(expression);
    }

    private void ShowChoiceButton()
    {
        List<Choice> currenChioises = _CurrentStory.currentChoices;

        ChoiceButtonPanel.SetActive(currenChioises.Count != 0);

        if (currenChioises.Count <= 0)
            return;

        ChoiceButtonPanel.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        _ChoiceText.Clear();

        for (int i = 0; i < currenChioises.Count; i++)
        {
            GameObject choice = Instantiate(ChoiceButtonPrefab);

            choice.GetComponent<Btn_Action>().Index = i;

            choice.transform.SetParent(ChoiceButtonPanel.transform, false);

            choice.transform.localScale = Vector3.one;

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.transform.localScale = Vector3.one;

            choiceText.text = currenChioises[i].text;
            _ChoiceText.Add(choiceText);
        }
    }

    public void ChoiceButtonAction(int choiceIndex)
    {
        _CurrentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory(true);
    }

    private void HandleTags()
    {
        foreach (string tag in _CurrentStory.currentTags)
        {
            if (tag == "forest") BackgroundImage.sprite = forestSprite;
            if (tag == "room") BackgroundImage.sprite = roomSprite;
            if (tag == "numder") BackgroundImage.sprite = numderSprite;
            if (tag == "music_room") BackgroundImage.sprite = music_roonSprite;
            if (tag == "home") BackgroundImage.sprite = homeSprite;
            if (tag == "dark_house") BackgroundImage.sprite = dark_house;
            if (tag == "camp") BackgroundImage.sprite = campSprite;
            if (tag == "dark_camp") BackgroundImage.sprite = dark_campSprite;
            if (tag == "office") BackgroundImage.sprite = officeSprite;
            if (tag == "boards") BackgroundImage.sprite = boardsSprite;
        }
    }

    private void ExitDialog()
    {
        DialoguePlay = false;
        DialoguePanel.SetActive(false);
        BackgroundPanel.SetActive(false);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex <= SceneManager.sceneCount)
            SceneManager.LoadScene(nextSceneIndex);
    }
}