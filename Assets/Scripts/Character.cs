using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public List<Sprite> characters;
    public string nameCharacter;

    public void ChangeCharacter(int currentExpression)
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogError($"Character {nameCharacter} has no sprites!");
            return;
        }

        if (currentExpression < 0 || currentExpression >= characters.Count)
        {
            Debug.LogError($"Wrong expression index {currentExpression} for {nameCharacter}. Count = {characters.Count}");
            return;
        }

        GetComponent<Image>().sprite = characters[currentExpression];
    }
}
