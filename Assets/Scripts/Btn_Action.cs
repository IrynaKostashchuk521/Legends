using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Btn_Action : MonoBehaviour
{
    public int Index;
    private Button _ButtonDialog;
    private ClickNext _ClickNext;
    private UnityAction _clickAction;
    void Start()
    {
        _ButtonDialog = GetComponent<Button>();
        _ClickNext = FindObjectOfType<ClickNext>();
        _clickAction = new UnityAction(() => _ClickNext.ChoiceButtonAction(Index));
        _ButtonDialog.onClick.AddListener (_clickAction);
    }
}
