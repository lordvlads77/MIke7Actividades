using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageText : MonoBehaviour
{
    private Text text;
    [SerializeField] string[] texts;
    bool setup = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        text = GetComponent<Text>();
        text.text = texts[LanguageManager.GetInstance().GetCurrentLanguageByte()];
        LanguageManager.GetInstance().OnLanguageChange += OnLanguageChange;
        setup = true;
    }

    void OnEnable()
    {
        if (!setup)
        {
            return;
        }
        text.text = texts[LanguageManager.GetInstance().GetCurrentLanguageByte()];
        LanguageManager.GetInstance().OnLanguageChange += OnLanguageChange;
    }

    void OnDestroy()
    {
        LanguageManager.GetInstance().OnLanguageChange -= OnLanguageChange;
    }

    void OnLanguageChange(LANGUAGES language)
    {
        text.text = texts[(byte)language];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
