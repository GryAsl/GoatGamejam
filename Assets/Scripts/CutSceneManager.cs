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
    public SpriteRenderer im4;
    public SpriteRenderer im5;
    public SpriteRenderer im6;
    public SpriteRenderer im7;
    public SpriteRenderer im8;
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

        imColor = im3.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f;
            im3.color = imColor;
            yield return null;
        }

        imColor = im2.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f;
            im2.color = imColor;
            yield return null;
        }

        imColor = im1.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f;
            im1.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im4.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im4.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im5.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im5.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im6.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im6.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im7.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im7.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        imColor = im8.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier;
            im8.color = imColor;
            yield return null;
        }


    }
}
