using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    [SerializeField] private DialogueInstaller dialogueInstaller;
    public override void InstallBindings()
    {
        BindDialogueInstaller();
    }

    private void BindDialogueInstaller()
    {
        Container.Bind<DialogueInstaller>().FromInstance(dialogueInstaller).AsSingle();
    }
}
