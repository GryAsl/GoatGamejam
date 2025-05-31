using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CutSceneManager : MonoBehaviour
{
    public float speedMultiplier;
    public float waitTime;

    [Header("CutScene 1")]
    public Camera cam1;
    public SpriteRenderer im1;
    public SpriteRenderer im2;
    public SpriteRenderer im3;
    public float endTimer;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CutScene1()
    {
        cam1.gameObject.SetActive (true);

        yield return new WaitForSecondsRealtime(1f);
        Color imColor = im1.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im1.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im2.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im2.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im3.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im3.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(endTimer);
    }
}
