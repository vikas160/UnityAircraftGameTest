using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PlayerInstance : MonoBehaviour
{
    internal Transform PlayerPos { get; private set; }
    internal static PlayerInstance Instance { get; private set; }

    //refrences
    [Header("Alert Audio Source")]
    [SerializeField] private AudioSource PlayerAudioSource = default;
    [SerializeField] private AudioClip AlertSoundSoundClip;

    [Header("Player Aircraft States")]
    internal int PlayerHealth = 500;

    public TextMeshProUGUI ScoreText = default;
    private int Score = 0;
    private void Awake()
    {
        Instance = this;
        PlayerPos = transform;
        PlayerAudioSource = GetComponent<AudioSource>();    
    }

    internal void TakeDmg(int dmgRate)
    {
        PlayerHealth -=  dmgRate;

        if(PlayerHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }


    internal  void PlayerAlertMusic()
    {

          if(!PlayerAudioSource.isPlaying) {

            PlayerAudioSource.PlayOneShot(AlertSoundSoundClip);
         }
           
    }

    internal void StopAlerMusic()
    {
        PlayerAudioSource.Stop();
    }


    internal void UpScore()
    {
        Score += 10;
        ScoreText.text = "Score " + Score;
    }
}
