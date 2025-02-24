
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HangmanController : MonoBehaviour
{
    [SerializeField] GameObject letterContainer;
    [SerializeField] GameObject keyboardButton;
    [SerializeField] GameObject keyboardContainer;
    [SerializeField] GameObject wordContainer;
    [SerializeField] GameObject[] hangmanStages;
    [SerializeField] TextAsset possibleWords;
    [SerializeField] public Animator Animator;




    private string word;
    private int inCorrectGuesses, correctGuesses;
    private void Start()
    {
        InitializeButtons();
        InitializeGame();
    }


    private void InitializeGame()
    {
        //bütün oyunu resetlerken

        inCorrectGuesses = 0;
        correctGuesses = 0;
        foreach (Button child in keyboardContainer.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }

        foreach (Transform child in wordContainer.transform)
        {

            Destroy(child.gameObject);
        }

        foreach (GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }

        word = GenerateWord().ToUpper();

        foreach (char letter in word)
        {
            Instantiate(letterContainer, wordContainer.transform);
        }
    }

    private string GenerateWord()
    {
        string[] wordlist = possibleWords.text.Split("\n");
        string line = wordlist[Random.Range(0, wordlist.Length)];
        return line.Substring(0, line.Length - 1);

    }
    private void InitializeButtons()
    {
        for (int i = 65; i <= 90; i++)
        {
            CreateButtons(i);
        }
    }

    private void CreateButtons(int i)
    {
        GameObject temp = Instantiate(keyboardButton, keyboardContainer.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
        temp.GetComponent<Button>().onClick.AddListener(delegate { CheckLetter(((char)i).ToString()); });
    }

    private void CheckLetter(string inputLetter)
    {
        bool letterInWord = false;
        for (int i = 0; i < word.Length; i++)
        {
            if (inputLetter == word[i].ToString())
            {
                letterInWord = true;
                correctGuesses++;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }

        }

        if (letterInWord == false)
        {

            inCorrectGuesses++;
            hangmanStages[inCorrectGuesses - 1].SetActive(true);

            if (inCorrectGuesses == 5)
            {
                hangmanStages[4].SetActive(true); // StageFive aktif
                                                  // StageFive altýndaki bacak animatorunu bul
                Animator righttLegAnimator = hangmanStages[4].transform.Find("sag").GetComponent<Animator>();
                righttLegAnimator.SetTrigger("swing");
            }
            if (inCorrectGuesses == 6)
            {

                hangmanStages[5].SetActive(true); // StageSix aktif
                                                  // StageSix altýndaki bacak animatorunu bul
                Animator leftLegAnimator = hangmanStages[5].transform.Find("sol").GetComponent<Animator>();
                leftLegAnimator.SetTrigger("swing");
            }


        }

        CheckOutCome();
    }

    private void CheckOutCome()
    {
        if (correctGuesses == word.Length)
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;
            }

            Invoke("InitializeGame", 5f);
        }

        if (inCorrectGuesses == hangmanStages.Length)
        {

            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();

            }
            Invoke("InitializeGame", 3f);
        }
    }
}
