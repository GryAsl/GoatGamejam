using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    public AudioSource musicPlayer;

    public AudioClip menuMusic;
    public AudioClip gameMusic1;
    public AudioClip gameMusic2;

    public Slider musicSlider;

    public bool inGame;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {
            if (musicPlayer != null && !musicPlayer.isPlaying)
            {
                if (musicPlayer.clip != null && gameMusic1 != null)
                {
                    if (musicPlayer.clip.name == gameMusic1.name)
                        ChangeMusic(gameMusic2);
                    else
                        ChangeMusic(gameMusic1);
                }
            }
        }
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        musicPlayer.clip = newMusic;
        musicPlayer.Play();
        StartCoroutine(RadioJumpShakeEffect());

    }

    private IEnumerator RadioJumpShakeEffect()
    {
        float jumpHeight = 0.2f;
        float shakeAmount = 0.15f;
        float sideShakeAmplitude = 0.2f; // Sağ-sol genliği
        float duration = 0.5f;
        float frequency = 20f;

        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            
            // Yukarı-aşağı zıplama (sinüs eğrisiyle)
            float y = Mathf.Sin(Mathf.PI * progress) * jumpHeight;
            
            // Yanlara (x) yine bir sinüs hareketi
            float x = Mathf.Sin(progress * Mathf.PI * 4f) * sideShakeAmplitude;
            
            // Hafif random titreme (x/y'de) ekle
            float shakeX = (Random.value - 0.5f) * shakeAmount;
            float shakeY = (Random.value - 0.5f) * shakeAmount * 0.5f;

        transform.localPosition = originalPos + new Vector3(
            x + shakeX,
            y + shakeY,
            0);

        elapsed += Time.deltaTime;
        yield return null;
    }
    
    transform.localPosition = originalPos;
}

    


    public void ChangeMusicToInGame()
    {
        musicPlayer.loop = false;
        musicPlayer.clip = gameMusic1;
        musicPlayer.Play();
    }

    public void ChangeMusicVolume()
    {
        musicPlayer.volume = musicSlider.value;
    }

}