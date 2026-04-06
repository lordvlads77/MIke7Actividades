using System;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private NarrativeText narrativeTextScriptable;
    
    int narrativeIndex = 0;

    private void Start()
    {
        _text.gameObject.SetActive(true);
        _text.text = narrativeTextScriptable.GetText(narrativeIndex);
        LanguageManager.GetInstance().OnLanguageChange += OnLanguageChange;
    }

    private void OnDisable()
    {
        LanguageManager.GetInstance().OnLanguageChange -= OnLanguageChange;
    }

    void OnLanguageChange(LANGUAGES languages)
    {
        _text.text = narrativeTextScriptable.GetText(narrativeIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (narrativeIndex < narrativeTextScriptable.NumberofTexts()-1)
            {
                narrativeIndex++;
                _text.text = narrativeTextScriptable.GetText(narrativeIndex);
            }
            else
            {
                _text.gameObject.SetActive(false);
            }
        }
    }
}
