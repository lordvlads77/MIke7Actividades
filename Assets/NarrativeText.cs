using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New NarrativeText", menuName = "NarrativeText")]
public class NarrativeText : ScriptableObject
{
    [SerializeField] StringArray[] TextToShow;

    public string GetText(int i)
    {
         return TextToShow[i].multiString[LanguageManager.GetInstance().GetCurrentLanguageByte()];
    }
    
    public int NumberofTexts()
    {
        return TextToShow.Length;
    }
}

[Serializable]
public class StringArray
{
    [TextArea]
    public string[] multiString;
}
