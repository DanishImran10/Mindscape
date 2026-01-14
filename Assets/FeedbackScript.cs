using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Auth;


public class FeedbackScript : MonoBehaviour
{
    private string userId;
    private DatabaseReference dbReference;
    private List<string> selectedAnswers = new List<string>();

    [System.Serializable]
    public class FeedbackQuestion
    {
        public string questionText;
        public string[] options;
        public AudioClip questionAudio; // Optional audio
    }

    public List<FeedbackQuestion> questions;
    private int currentQuestionIndex;

    public Text questionText;
    public TextMeshProUGUI[] optionTexts;
    public Button[] optionButtons;
    public Text feedbackText;
    public Button nextButton;

    public TMP_InputField commentInputField; // For open-ended feedback
    public GameObject commentPanel; // Enable only for the last question

    public AudioSource audioSource;

    private bool answered = false;
    public Button submitButton;

    void Start()
    {
        userId = PlayerPrefs.GetString("userId");
        dbReference = FirebaseDatabase.GetInstance("https://mindscape0820-default-rtdb.firebaseio.com/").RootReference;
        Debug.Log("db ref" + dbReference);
        LoadFeedbackQuestions();
        currentQuestionIndex = 0;
        ShowQuestion();
        nextButton.onClick.AddListener(NextQuestion);
    }
    void SelectAnswer(int index)
    {
        if (answered) return;
        answered = true;

        FeedbackQuestion q = questions[currentQuestionIndex];
        string selectedOption = q.options[index];

        selectedAnswers.Add(selectedOption); // Local tracking

        // Immediate Firebase update for this answer
        string questionKey = $"Q{currentQuestionIndex + 1}";
        dbReference.Child("users").Child(userId).Child("feedback").Child(questionKey).SetValueAsync(selectedOption);

        feedbackText.text = "Thank you for your feedback!";
        feedbackText.color = Color.blue;

        nextButton.gameObject.SetActive(true);
    }
    public void SubmitComment()
    {
        string comment = commentInputField.text;

        // Save comment and timestamp only
        Dictionary<string, object> commentData = new Dictionary<string, object>();
        commentData["Comment"] = comment;
        commentData["timestamp"] = ServerValue.Timestamp;

        var task = dbReference.Child("users").Child(userId).Child("feedback").UpdateChildrenAsync(commentData);

        StartCoroutine(HandleSubmitResult(task));
    }
    IEnumerator HandleSubmitResult(System.Threading.Tasks.Task task)
    {
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Error submitting comment: " + task.Exception);
        }
        else
        {
            Debug.Log("Comment submitted successfully!");
            SceneManager.LoadScene(0); // Go to main scene
        }
    }


    void EndFeedback()
    {
        string comment = commentInputField.text;

        Dictionary<string, object> feedbackData = new Dictionary<string, object>();

        for (int i = 0; i < questions.Count; i++)
        {
            feedbackData[$"Q{i + 1}"] = selectedAnswers[i];
        }

        feedbackData["Comment"] = comment;
        feedbackData["timestamp"] = ServerValue.Timestamp;

        var task = dbReference.Child("users").Child(userId).Child("feedback").SetValueAsync(feedbackData);

        StartCoroutine(HandleFeedbackSubmission(task));

        questionText.text = "Feedback Submitted. Thank you!";
        feedbackText.text = "";
        commentPanel.SetActive(false);
        nextButton.gameObject.SetActive(false);
        StartCoroutine(gotoStartScene());
    }

    IEnumerator HandleFeedbackSubmission(System.Threading.Tasks.Task task)
    {
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Error saving feedback: " + task.Exception);
        }
        else
        {
            Debug.Log("Feedback submitted successfully!");
        }
    }


    void LoadFeedbackQuestions()
    {
        questions = new List<FeedbackQuestion>()
        {
            new FeedbackQuestion {
                questionText = "How would you rate your overall experience?",
                options = new string[] { "Excellent", "Good", "Average", "Bad","Poor" }
            },
            new FeedbackQuestion {
                questionText = "Was the app easy to navigate?",
                options = new string[] { "Very Easy", "Easy", "Neutral", "Difficult", "Very Difficult" }
            },
            new FeedbackQuestion {
                questionText = "How did you find the gaze-based interaction?",
                options = new string[] { "Very Intuitive", "Somewhat Intuitive", "Neutral", "Confusing", "Very Difficult" }
            },
            new FeedbackQuestion {
                questionText = "Was the AR/VR environment comfortable and smooth?",
                options = new string[] { "Very Comfortable", "Comfortable", "Neutral", "Slightly Uncomfortable", "Very Uncomfortable" }
            },
            new FeedbackQuestion {
                questionText = "Did the app help you better understand PTSD?",
                options = new string[] { "Strongly Agree", "Agree", "Neutral", "Disagree", "Strongly Disagree" }
            }
        };
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
        commentPanel.SetActive(false);

        FeedbackQuestion q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        if (q.questionAudio != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = q.questionAudio;
            audioSource.Play();
        }

        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (i < q.options.Length)
            {
                optionTexts[i].transform.parent.gameObject.SetActive(true);
                optionTexts[i].text = q.options[i];
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
            }
            else
            {
                optionTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    // void SelectAnswer(int index)
    // {
    //     if (answered) return;

    //     answered = true;

    //     feedbackText.text = "Thank you for your feedback!";
    //     feedbackText.color = Color.blue;

    //     nextButton.gameObject.SetActive(true);
    // }

    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            ShowCommentInput(); // Final screen
        }
    }

    void ShowCommentInput()
    {
        questionText.text = "✍ Feedback\n\nWhat did you like the most or what would you improve in MindScape?\nPlease share any suggestions, issues, or comments:";
        feedbackText.text = "";

        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);

        commentPanel.SetActive(true);
        nextButton.gameObject.SetActive(false); // Hide Next

        submitButton.gameObject.SetActive(true);
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(SubmitComment);
    }


    // void EndFeedback()
    // {
    //     // Here you could save the comments (commentInputField.text) to file, database, or log
    //     Debug.Log("User comments: " + commentInputField.text);

    //     questionText.text = "Feedback Submitted. Thank you!";
    //     feedbackText.text = "";
    //     commentPanel.SetActive(false);
    //     nextButton.gameObject.SetActive(false);
    //     StartCoroutine(gotoStartScene());
    // }

    IEnumerator gotoStartScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
}
