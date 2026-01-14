using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField] string videoFileName;

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        UnityEngine.Video.VideoPlayer videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        if (videoPlayer)
        {
            // string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "startVideo.mp4");
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
