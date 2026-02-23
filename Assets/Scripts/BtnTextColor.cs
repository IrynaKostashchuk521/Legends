using UnityEngine;
using TMPro;

public class BtnTextColor : MonoBehaviour
{
    public TMP_Text text;

    public void OnHover()
    {
        text.color = Color.black;
    }

    public void OnExit()
    {
        text.color = Color.white;
    }
}

