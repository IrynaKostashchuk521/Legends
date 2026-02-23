using TMPro; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ink.Runtime;
using Zenject;
using System.Linq;

public class ClickNext : MonoBehaviour
{
    private Story _CurrentStory;
    private TextAsset _inkJson;


    //Для діалога.
    private GameObject _DialoguePanel;
    private TextMeshProUGUI _Textdialogue;
    private TextMeshProUGUI _NameCharacter;


    //Для button виборів.
    [HideInInspector] public GameObject _ChoiceButtonPanel;

    private GameObject _ChoiceButton;

    private List<TextMeshProUGUI> _ChoiceText = new();

    public bool DialoguePlay { get; private set; }

    private List<Character> characters = new();


    [Inject]
    public void Parameters(DialogueInstaller dialogueInstaller)
    {
        _inkJson = dialogueInstaller.inkJson;
        _DialoguePanel = dialogueInstaller.DialoguePanel;
        _Textdialogue = dialogueInstaller.Textdialogue;
        _NameCharacter = dialogueInstaller.NameCharacter;
        _ChoiceButtonPanel = dialogueInstaller.ChoiceButtonPanel;
        _ChoiceButton = dialogueInstaller.ChoiceButton;
    }

    private void Awake()
    {
        _CurrentStory = new Story(_inkJson.text);
    }

    void Start()
    {
        foreach(var character in FindObjectsOfType<Character>())
        {
            characters.Add(character);
        }
        StartDialog();
    }

    public void StartDialog()
    {
        DialoguePlay = true;
        _DialoguePanel.SetActive(true);
        ContinueStore();
    }

    public void ContinueStore(bool choiceBefore = false)
    {
        if (_CurrentStory.canContinue)
        {
            ShowDialod();
            ShowChoiceButton();
        }
        else if (!choiceBefore)
        {
            ExitDialog();
        }
    }

    private void ShowDialod()
    {
        _Textdialogue.text = _CurrentStory.Continue();
        _NameCharacter.text = (string)_CurrentStory.variablesState["NameCharacter"];

        // 👉 Сховати всіх персонажів
        foreach (var c in characters)
        {
            c.gameObject.SetActive(false);
        }

        int index = characters.FindIndex(character =>
            character.nameCharacter == _NameCharacter.text);

        if (index == -1)
        {
            Debug.LogError("Character not found: " + _NameCharacter.text);
            return;
        }

        int expression = (int)_CurrentStory.variablesState["characterExpression"];

        // 👉 показати потрібного
        characters[index].gameObject.SetActive(true);
        characters[index].ChangeCharacter(expression);
    }



    private void ShowChoiceButton()
    {
        //Choice - Відповідає за те щоб в ink провіряти кількість кнопок.
        List<Choice> currenChioises = _CurrentStory.currentChoices; //Для того щоб видодити ту кількість кнопок яку ми створили.
        _ChoiceButtonPanel.SetActive(currenChioises.Count != 0);
        if (currenChioises.Count <= 0)
        {
            return;
        }

        _ChoiceButtonPanel.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject)); //Для того щоб не було виводу всіх кнопок вибору підряд.
        _ChoiceText.Clear();                                                                                            //Очищення старих кнопок вибору.

        for (int i = 0; i < currenChioises.Count; i++)
        {
            GameObject choice = Instantiate(_ChoiceButton);
            choice.GetComponent<Btn_Action>().Index = i;
            choice.transform.SetParent(_ChoiceButtonPanel.transform);

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = currenChioises[i].text;
            _ChoiceText.Add(choiceText);
        }
    }

    public void ChoiceButtonAction(int choiceIndex)
    {
        _CurrentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStore(true);
    }

    private void ExitDialog()
    {
        DialoguePlay = false;
        _DialoguePanel.SetActive(false);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex <= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}