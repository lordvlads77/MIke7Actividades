using System;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager _instance;

    public static LanguageManager GetInstance()
    {
        return _instance;
    }
    
    [SerializeField] private LANGUAGES currentLanguage;
    public Action<LANGUAGES> OnLanguageChange;

    private void Awake()
    {
        _instance = this;
    }

    public LANGUAGES GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public byte GetCurrentLanguageByte()
    {
        return (byte)currentLanguage;
    }

    public void ChangeLanguage(LANGUAGES language)
    {
        currentLanguage = language;
        OnLanguageChange?.Invoke(currentLanguage);
        Debug.Log("Current Language:" + currentLanguage);
    }

    private void ChangeLanguage(bool right)
    {
        if (right)
        {
            if (currentLanguage < LANGUAGES.END-1)
            {
                currentLanguage++;
            }
            else
            {
                currentLanguage = 0;
            }
        }
        else
        {
            if (currentLanguage <= 0)
            {
                currentLanguage = LANGUAGES.END - 1;
            }
            else
            {
                currentLanguage--;
            }
        }
        ChangeLanguage(currentLanguage);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLanguage(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLanguage(false);
        }
    }
}

public enum LANGUAGES
{
    ENGLISH,
    SPANISH,
    PORTUGUESE,
    END
}
