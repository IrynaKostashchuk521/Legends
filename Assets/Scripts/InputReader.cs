using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Ink.Runtime;
using Unity.VisualScripting;

public class InputReader : MonoBehaviour, Controls.IDialogActions
{
    Controls _inputActions;
    ClickNext _clickNext;
    private void OnEnable() 
    {
        _clickNext = FindObjectOfType<ClickNext>();

        if (_inputActions != null)
        {
            return;
        }

        _inputActions = new Controls();
        _inputActions.Dialog.SetCallbacks(this);
        _inputActions.Dialog.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Dialog.Disable();
    }

    void Controls.IDialogActions.OnNextPhrase(InputAction.CallbackContext context)
    {
        if(context.canceled && _clickNext.DialoguePlay)
        {
            _clickNext.ContinueStory(_clickNext.ChoiceButtonPanel.activeInHierarchy); //Історія не буде закінчуватись до поки панека з кнопками буде активна.
        }
    }
}
