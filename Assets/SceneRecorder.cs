#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.IO; // Required for path and directory

public class SceneRecorder : MonoBehaviour
{
    private RecorderController recorderController;
    private bool isRecording = false;

    void Start()
    {
        SetupRecorder();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isRecording)
            {
                recorderController.PrepareRecording();
                recorderController.StartRecording();
                Debug.Log("🎥 Recording started.");
                isRecording = true;
            }
            else
            {
                recorderController.StopRecording();
                Debug.Log("🛑 Recording stopped.");
                isRecording = false;
            }
        }
    }

    void SetupRecorder()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);

        var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "MyVideoRecorder";
        videoRecorder.Enabled = true;

        videoRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;

        videoRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        videoRecorder.AudioInputSettings.PreserveAudio = true;

        // Generate save path in project root folder
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string recordingsDir = Path.Combine(Application.dataPath, "../Recordings");
        Directory.CreateDirectory(recordingsDir); // ensure folder exists
        string outputPath = Path.Combine(recordingsDir, $"{sceneName}_{timestamp}");

        videoRecorder.OutputFile = outputPath;
        Debug.Log("📁 Recording will be saved to: " + outputPath + ".mp4");

        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;
    }
}
#endif
