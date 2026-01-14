using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActivityManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] options;
        public int correctAnswerIndex;
        public AudioClip questionAudio; // Audio clip for this question
    }

    public List<Question> questions;
    private int currentQuestionIndex;

    public Text questionText;
    public Text[] optionTexts;
    public Button[] optionButtons;
    public Text feedbackText;
    public Button nextButton;

    public AudioSource audioSource; // Assign this in Inspector

    private bool answered = false;

    void Start()
    {
        currentQuestionIndex = 0;
        ShowQuestion();
        nextButton.onClick.AddListener(NextQuestion);
        // LoadQuestions();
        // currentQuestionIndex = 0;
        // ShowQuestion();
        // nextButton.onClick.AddListener(NextQuestion);
    }

    void LoadQuestions()
    {
        questions = new List<Question>()
        {
            new Question
            {
                questionText = "Which of the following can act as a PTSD trigger?",
                options = new string[]
                {
                    "A specific smell",
                    "A loud noise",
                    "A place associated with trauma",
                    "All of the above"
                },
                correctAnswerIndex = 3
                // Assign audio via Inspector
            },
            new Question
            {
                questionText = "What happens during a PTSD flashback?",
                options = new string[]
                {
                    "The person imagines the traumatic event but knows it isn’t real",
                    "The person feels as if they are reliving the traumatic event",
                    "The person gets sad but doesn’t recall details",
                    "The person instantly forgets the trauma"
                },
                correctAnswerIndex = 1
            },
            new Question
            {
                questionText = "Why does the brain form PTSD triggers?",
                options = new string[]
                {
                    "To make the person remember trauma",
                    "Because memories during trauma are stored without full context",
                    "As a way to prevent future trauma",
                    "Due to genetics"
                },
                correctAnswerIndex = 1
            },
            new Question
            {
                questionText = "Which trigger is more likely to cause a severe PTSD reaction?",
                options = new string[]
                {
                    "A faint smell that reminds a person of the trauma",
                    "Revisiting the exact location of the trauma",
                    "Seeing a general object that resembles something from the past",
                    "Listening to calming music"
                },
                correctAnswerIndex = 1
            },
            new Question
            {
                questionText = "What is one common way to manage a PTSD trigger?",
                options = new string[]
                {
                    "Avoiding all reminders forever",
                    "Using grounding techniques",
                    "Ignoring it until it goes away",
                    "Reacting aggressively to regain control"
                },
                correctAnswerIndex = 1
            }
        };
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        // Play the audio clip
        if (q.questionAudio != null && audioSource != null)
        {
            Debug.Log("audio stopping");
            audioSource.Stop();
            audioSource.clip = q.questionAudio;
            audioSource.Play();
            Debug.Log("audio playing");
        }

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = q.options[i];
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }
    }

    void SelectAnswer(int index)
    {
        if (answered) return;

        answered = true;
        bool isCorrect = index == questions[currentQuestionIndex].correctAnswerIndex;

        feedbackText.text = isCorrect ? "Correct!" : "Wrong!";
        feedbackText.color = isCorrect ? Color.green : Color.red;

        nextButton.gameObject.SetActive(true);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Count)
            ShowQuestion();
        else
            EndQuiz();
    }

    void EndQuiz()
    {
        questionText.text = "Quiz Complete!";
        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
        if (audioSource != null) audioSource.Stop();
        StartCoroutine(gotoStartScene());
    }

    IEnumerator gotoStartScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
}









// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class ActivityManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class Question
//     {
//         public string questionText;
//         public string[] options;
//         public int correctAnswerIndex;
//     }

//     public List<Question> questions;
//     private int currentQuestionIndex;

//     public Text questionText;
//     public Text[] optionTexts;
//     public Button[] optionButtons;
//     public Text feedbackText;
//     public Button nextButton;

//     private bool answered = false;

//     void Start()
//     {
//         LoadQuestions();
//         currentQuestionIndex = 0;
//         ShowQuestion();
//         nextButton.onClick.AddListener(NextQuestion);
//     }

//     void LoadQuestions()
//     {
//         questions = new List<Question>()
//         {
//             new Question
//             {
//                 questionText = "Which of the following can act as a PTSD trigger?",
//                 options = new string[]
//                 {
//                     "A specific smell",
//                     "A loud noise",
//                     "A place associated with trauma",
//                     "All of the above"
//                 },
//                 correctAnswerIndex = 3
//             },
//             new Question
//             {
//                 questionText = "What happens during a PTSD flashback?",
//                 options = new string[]
//                 {
//                     "The person imagines the traumatic event but knows it isn�t real",
//                     "The person feels as if they are reliving the traumatic event",
//                     "The person gets sad but doesn�t recall details",
//                     "The person instantly forgets the trauma"
//                 },
//                 correctAnswerIndex = 1
//             },
//             new Question
//             {
//                 questionText = "Why does the brain form PTSD triggers?",
//                 options = new string[]
//                 {
//                     "To make the person remember trauma",
//                     "Because memories during trauma are stored without full context",
//                     "As a way to prevent future trauma",
//                     "Due to genetics"
//                 },
//                 correctAnswerIndex = 1
//             },
//             new Question
//             {
//                 questionText = "Which trigger is more likely to cause a severe PTSD reaction?",
//                 options = new string[]
//                 {
//                     "A faint smell that reminds a person of the trauma",
//                     "Revisiting the exact location of the trauma",
//                     "Seeing a general object that resembles something from the past",
//                     "Listening to calming music"
//                 },
//                 correctAnswerIndex = 1
//             },
//             new Question
//             {
//                 questionText = "What is one common way to manage a PTSD trigger?",
//                 options = new string[]
//                 {
//                     "Avoiding all reminders forever",
//                     "Using grounding techniques",
//                     "Ignoring it until it goes away",
//                     "Reacting aggressively to regain control"
//                 },
//                 correctAnswerIndex = 1
//             }
//         };
//     }

//     void ShowQuestion()
//     {
//         answered = false;
//         feedbackText.text = "";
//         nextButton.gameObject.SetActive(false);

//         Question q = questions[currentQuestionIndex];
//         questionText.text = q.questionText;

//         for (int i = 0; i < optionTexts.Length; i++)
//         {
//             optionTexts[i].text = q.options[i];
//             int index = i;
//             optionButtons[i].onClick.RemoveAllListeners();
//             optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
//         }
//     }

//     void SelectAnswer(int index)
//     {
//         if (answered) return;

//         answered = true;
//         bool isCorrect = index == questions[currentQuestionIndex].correctAnswerIndex;

//         feedbackText.text = isCorrect ? "Correct!" : "Wrong!";
//         feedbackText.color = isCorrect ? Color.green : Color.red;

//         nextButton.gameObject.SetActive(true);
//     }

//     void NextQuestion()
//     {
//         currentQuestionIndex++;
//         if (currentQuestionIndex < questions.Count)
//             ShowQuestion();
//         else
//             EndQuiz();
//     }

//     void EndQuiz()
//     {
//         questionText.text = "Quiz Complete!";
//         foreach (var btn in optionButtons)
//             btn.gameObject.SetActive(false);
//         feedbackText.text = "";
//         nextButton.gameObject.SetActive(false);
//     }
// }
