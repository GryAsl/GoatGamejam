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

    float skip = 1f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skip = 10000f;
        }
    }

    public IEnumerator CutScene1()
    {
        cam1.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        Color imColor = im1.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im1.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im2.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im2.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im3.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im3.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(endTimer / skip);

        imColor = im3.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im3.color = imColor;
            yield return null;
        }

        imColor = im2.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im2.color = imColor;
            yield return null;
        }

        imColor = im1.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im1.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im4.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im4.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im5.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im5.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im6.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im6.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im7.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im7.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im8.color;
        while (imColor.a <= 1f)
        {
            imColor.a += Time.deltaTime * speedMultiplier * skip;
            im8.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitTime / skip);

        imColor = im8.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im8.color = imColor;
            yield return null;
        }

        imColor = im7.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im7.color = imColor;
            yield return null;
        }

        imColor = im6.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im6.color = imColor;
            yield return null;
        }

        imColor = im5.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im5.color = imColor;
            yield return null;
        }
        imColor = im4.color;
        while (imColor.a >= 0f)
        {
            imColor.a -= Time.deltaTime * speedMultiplier * 5f * skip;
            im4.color = imColor;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(waitTime / skip);
        cam1.gameObject.SetActive(false);

        GameObject.Find("GameManager").GetComponent<GameManager>().CutsceneDone();
    }
}
