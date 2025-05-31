using UnityEngine;
using UnityEngine.UI;

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
            if (!musicPlayer.isPlaying)
            {
                if(musicPlayer.clip.name == gameMusic1.name)
                {
                    musicPlayer.clip = gameMusic2;
                    musicPlayer.Play();
                }
                else
                {
                    musicPlayer.clip = gameMusic1;
                    musicPlayer.Play();
                }

            }
        }

    }

    public void ChangeMusic(AudioClip newMusic)
    {
        musicPlayer.clip = newMusic;
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
