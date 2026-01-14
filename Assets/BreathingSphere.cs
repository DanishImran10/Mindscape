using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  
public class BreathingSphere : MonoBehaviour
{
    public GameObject CanvasB;
    public TextMeshProUGUI breathText;  // Assign this in the Inspector
    private Vector3 originalScale = Vector3.one;
    private Vector3 expandedScale = Vector3.one * 2f;
    private Vector3 shrunkScale = Vector3.one * 0.5f;

    private void Start()
    {
        CanvasB.SetActive(true);
        StartCoroutine(BreathingCycle());
    }

    private IEnumerator BreathingCycle()
    {
        int cycles = 3;

        for (int i = 0; i < cycles; i++)
        {
            // Inhale
            breathText.text = "Inhale and hold your breath";
            yield return StartCoroutine(ScaleOverTime(originalScale, expandedScale, 2f));
            
            yield return new WaitForSeconds(3f);

            // Exhale
            breathText.text = "Now exhale";
            yield return StartCoroutine(ScaleOverTime(expandedScale, shrunkScale, 2f));
            
            yield return new WaitForSeconds(1f);
        }

        // Back to original
        yield return StartCoroutine(ScaleOverTime(shrunkScale, originalScale, 1.5f));
        breathText.text = "Now, you are good to go";
        FindObjectOfType<BusSoundManager>().StopAnxietyEffects();

        yield return new WaitForSeconds(3f);
        CanvasB.SetActive(false);
        transform.position = new Vector3(0, -1000, 0); // Move out of view for a while
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0); // start scene
        //Destroy(gameObject);
    }

    private IEnumerator ScaleOverTime(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
    }
}
