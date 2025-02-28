using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class global_canvas_script : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject moduleList;
    public GameObject Instructions;
    public void OnStartButtonClick()
    {
        moduleList.SetActive(true);
    }
    public void OnInstructionButtonClick()
    {
        Instructions.SetActive(true);
    }
    public void OnCloseClick()
    {
        CloseAllpanels();
    }
    private void CloseAllpanels()
    {
        moduleList.SetActive(false);
        Instructions.SetActive(false);
    }
    public void OnModule1Click()
    {
        StartCoroutine(loadScene(1));
    }
    public void OnModule2Click()
    {
        StartCoroutine(loadScene(2));
    }
    IEnumerator loadScene(int n)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(n);
    }
    public void OnPlayButtonClick()
    {
         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
