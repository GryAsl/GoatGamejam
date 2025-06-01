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
                    ChangeMusic(gameMusic2);
                }
                else
                {
                    ChangeMusic(gameMusic1);
                }

            }
        }

    }

    public void ChangeMusic(AudioClip newMusic)
    {
        musicPlayer.clip = newMusic;
        musicPlayer.Play();
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
