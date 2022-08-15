using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private GameObject Player;
    [SerializeField] private AudioClip[] Level;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioPlayer;
    private float waitTime;
    private bool whetherStop;
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.Play();
        waitTime = audioPlayer.clip.length;
        whetherStop = true;
    }
    void Update()
    {
        if(Player.GetComponent<Player>().gameOver == true && whetherStop){
            audioPlayer.Stop();
        }

        waitTime -= Time.deltaTime;
        if(waitTime <= 0){
            int tmp = System.Convert.ToInt32(Score.text);
            if(tmp <= 30){
                audioPlayer.clip = Level[0];
                audioPlayer.Play();
                waitTime = audioPlayer.clip.length;
            }
            else if(tmp <= 120){
                audioPlayer.clip = Level[1];
                audioPlayer.Play();
                waitTime = audioPlayer.clip.length;
            }
            else if(tmp <= 220){
                audioPlayer.clip = Level[2];
                audioPlayer.Play();
                waitTime = audioPlayer.clip.length;
            }
            else{
                audioPlayer.clip = Level[3];
                audioPlayer.Play();
                waitTime = audioPlayer.clip.length;
            }
        }
        
    }
    public void gameOverPlay()
    {
        whetherStop = false;
        audioPlayer.clip = gameOverSound;
        audioPlayer.Play();
    }
}
