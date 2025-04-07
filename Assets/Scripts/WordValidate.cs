using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordValidate : MonoBehaviour
{
    [SerializeField]
    public TextAsset WordFile;
    [SerializeField]
    private HashSet<string> wordsList = new HashSet<string>();
    void Start()
    {
        GetDataFromTextFile();
        GameManager.Instance.CheckForWordValidate += isValidateWord;
    }

    public void GetDataFromTextFile()
    {
        string[] words = WordFile.text.Split('\n');
        foreach (string word in words)
        {
            wordsList.Add(word.Trim().ToUpper());
        }
    }

    public bool isValidateWord(string word)
    {
        return wordsList.Contains(word);
    }
}
