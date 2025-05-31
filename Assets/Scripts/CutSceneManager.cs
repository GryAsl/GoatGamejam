using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CutSceneManager : MonoBehaviour
{
    public float speedMultiplier;
    public float waitTime;
    public SpriteRenderer im1;
    public SpriteRenderer im2;
    public SpriteRenderer im3;
    void Start()
    {
        StartCoroutine(CutScene1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CutScene1()
    {
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

        yield return null;
    }
}
