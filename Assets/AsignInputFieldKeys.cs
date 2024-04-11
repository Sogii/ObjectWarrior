using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AsignInputFieldKeys : MonoBehaviour
{
    //Header
    public TextMeshProUGUI Header;

    //OpenAI inputfield
    public TMP_InputField OpenAIInputField;

    //Adobe inputfield
    public TMP_InputField AdobeKeyInputField;
    public TMP_InputField AdobeSecretInputField;




    public void StartButton()
    {
        if (OpenAIInputField.text == "" || AdobeKeyInputField.text == "" || AdobeSecretInputField.text == "")
        {
            Header.text = "Please fill in all the fields";
        }
        else
        {
            PlayerPrefs.SetString("OpenAIKey", OpenAIInputField.text);
            PlayerPrefs.SetString("AdobeKey", AdobeKeyInputField.text);
            PlayerPrefs.SetString("AdobeSecret", AdobeSecretInputField.text);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("InsertAPIKeysMenu");
        }
    }
}
