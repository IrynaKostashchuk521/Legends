using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DialogueInstaller : MonoBehaviour
{
    public TextAsset inkJson;

    //Для діалога.
    public GameObject DialoguePanel;
    public TextMeshProUGUI Textdialogue;
    public TextMeshProUGUI NameCharacter;


    //Для button виборів.
    public GameObject ChoiceButtonPanel;
    public GameObject ChoiceButton;
}
