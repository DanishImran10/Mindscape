using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class QuizManager : MonoBehaviour
{
    //////////////////////////////////////////////////////////
    // Audios for correct and wrong answers
    public AudioSource audioSourceForAnswers;
    public AudioClip[] audioClipsForAnswers;
    ///////////////////////////////////////////////////
    string[] options; // string for options
    public Text questionText; // Question ka text
    public Button[] optionButtons; // 4 buttons
    public Text resultText; // Result display text

    private int correctAnswerIndex; // Correct answer ka index

    // audios 
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] audioClips;  // Array to hold audio clips

    //Animations
    public GameObject[] Acts;
    public GameObject RestartButton;
    // public GameObject Act2;
    // public GameObject Act3;
    // public GameObject Act4;

    // others
    private int number_Of_Questions_To_Be_Displayed = 2;
    private int current_Number_Of_Question = 0;
    private int answer;
    int x = 0;// variable that keeps the currect index
    int n = 0; // variable for saving question number that has already been displayed

    void Start()
    {
        RestartButton.SetActive(false);
        PlayAudioByIndex(0);
        StartCoroutine(PlayingAnimations());
        //SetupQuestion(2);
    }

    IEnumerator PlayingAnimations()
    {
        ////////////////////////////////////////////////////////////////////
        // clearing the previous options and button's listeners, if any
        options = new string[] { " ", " ", " ", " " };
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Closure fix
            optionButtons[i].GetComponentInChildren<Text>().text = options[i];
            optionButtons[i].onClick.RemoveAllListeners();
        }
        resultText.gameObject.SetActive(false);
        ////////////////////////////////////////////////////////////////////
        if(n == 0)
        {
           yield return new WaitForSeconds(24.0f);
        }
        //int x = 0; // making it a global for this script check above
        if(current_Number_Of_Question < number_Of_Questions_To_Be_Displayed)
        {
            while(x == n)
            {
                x = Random.Range(1,5);
            }
            current_Number_Of_Question += 1;
            PlayAnimation(x);
            yield return new WaitForSeconds(7.5f);
            if(x==1)
            {
                answer = 3;
            }
            else if(x==2)
            {
                answer = 1;
            }
            else if(x==3)
            {
                answer = 0;
            }
            else if(x==4)
            {
                answer = 2;
            }
            n = x;
            HideAnimation(x);
            SetupQuestion(answer);
        }
        else
        {
            Debug.Log("go to Module 3 now");
        }
    }

    void SetupQuestion(int correctAnswer)
    {
        // Example question and options
        questionText.text = "Choose what suits the best";
        /*string[]*/ options = new string[] { "Cognition Act", "Avoidance Act", "Hyperarousal Act", "Intrusion Act" };
        correctAnswerIndex = correctAnswer;

        // Options buttons setup
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Closure fix
            optionButtons[i].GetComponentInChildren<Text>().text = options[i];
            optionButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }

        // Hide result text initially
        resultText.gameObject.SetActive(false);
    }

    void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            resultText.text = "Correct!";
            resultText.color = Color.green;
            audioSourceForAnswers.clip = audioClipsForAnswers[0];
            audioSourceForAnswers.Play();
        }
        else
        {
            resultText.text = "Wrong!";
            resultText.color = Color.red;
            audioSourceForAnswers.clip = audioClipsForAnswers[1];
            audioSourceForAnswers.Play();
        }

        resultText.gameObject.SetActive(true);
        PlayAudioByIndex(x);
        StartCoroutine(WaitingForNextAnimation());
    }

    IEnumerator WaitingForNextAnimation()
    {
        yield return new WaitForSeconds(8f);
        StartCoroutine(PlayingAnimations());
    }


    // Function to play an audio clip by index
    public void PlayAudioByIndex(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index]; // Assign the audio clip
            audioSource.Play(); // Play the audio
        }
        else
        {
            Debug.LogWarning("Invalid audio index: " + index);
        }
    }

    // Example function to stop the audio
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    private void PlayAnimation(int index)
    {
        Acts[index].SetActive(true);
        RestartButton.SetActive(false);
    }
    private void HideAnimation(int index)
    {
        Acts[index].SetActive(false);
        RestartButton.SetActive(true);
    }
    public void OnRestartAnimationClick()
    {
        PlayAnimation(x);
        StartCoroutine(WaitToHideAnimation());
    }
    IEnumerator WaitToHideAnimation()
    {
        yield return new WaitForSeconds(5.0f);
        HideAnimation(x);
    }
}
