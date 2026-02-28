using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public List<Sprite> characters;  // спрайти для емоцій
    public string nameCharacter;     // ім'я персонажа

    public void ChangeCharacter(int currentExpression)
    {
        if (characters == null || characters.Count == 0) return;

        if (currentExpression < 0 || currentExpression >= characters.Count)
        {
            Debug.LogWarning($"Wrong expression index {currentExpression} for {nameCharacter}. Count = {characters.Count}");
            currentExpression = 0;  // показуємо перший спрайт, щоб не падало
        }

        GetComponent<Image>().sprite = characters[currentExpression];
    }
}