using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFade : MonoBehaviour
{
    public List<Renderer> objectRenderers; // Multiple 3D Objects ke Renderers
    public float fadeDuration = 1.5f; // Fade hone ka time

    private List<Material> objectMaterials = new List<Material>();

    void Start()
    {
        // Har object ka material store karo
        foreach (Renderer objRenderer in objectRenderers)
        {
            if (objRenderer != null)
            {
                objectMaterials.Add(objRenderer.material);
            }
            else
            {
                Debug.Log("no obj renderer");
            }
        }
        FadeOutAll();
    }

    public void FadeOutAll()
    {
        foreach (Material mat in objectMaterials)
        {
            if (mat != null)
            {
                StartCoroutine(FadeOutObject(mat));
            }
            else
            {
                Debug.Log("No material ");
            }
        }
    }

    IEnumerator FadeOutObject(Material objectMaterial)
    {
        float elapsedTime = 0f;
        Color initialColor = objectMaterial.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // Alpha 0 means invisible

        while (elapsedTime < fadeDuration)
        {
            objectMaterial.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectMaterial.color = targetColor; // Ensure full fade
    }
}
