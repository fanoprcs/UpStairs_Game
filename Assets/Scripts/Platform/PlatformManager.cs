using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlatformManager : MonoBehaviour
{
    [SerializeField] private float shiftSpeed = 6f;
    [SerializeField] GameObject player; 
    [SerializeField] private TextMeshProUGUI Score; 
    [SerializeField] private GameObject[] platforms; 
    [SerializeField] private GameObject tool;
    private int nextToolSpawn;
    private int countSpawn;
    [SerializeField] GameObject[] plateformTypes;
    [SerializeField] private GameObject toolTypes;
    private int index;
    private Rigidbody2D rb;
    private int scoreVal;
    void Start(){
        index = 0;
        scoreVal = 0;
        countSpawn = 0;
        tool = null;
        rb = player.GetComponent<Rigidbody2D>();
        nextToolSpawn = Random.Range(60, 100);
    }
    void Update(){
        if(player.transform.position.y >= 8f){
            for(index = 0; index < platforms.Length; index++){
                float originY = platforms[index].transform.position.y;
                platforms[index].transform.Translate(0, -shiftSpeed * Time.deltaTime, 0);
                var p = platforms[index].GetComponent<Platform>();
                float tmp;
                float shift = originY - platforms[index].transform.position.y;
                p.startPosY -= shift;
                p.trueStartY -= shift;
                if(p.mode == 2){//y dir
                    p.turningPoint -= shift;
                    tmp = p.turningPoint;//if y dir move, let check point = most high point
                }
                else
                    tmp = p.trueStartY;
                if(tmp <= 0f){
                    Destroy(platforms[index]);
                    countSpawn++;
                    scoreVal++;
                    Score.text = scoreVal.ToString();
                    spawnPlatform(p.trueStartY + 17f);
                }
            }
            if(tool != null)
                tool.transform.Translate(0, -shiftSpeed * Time.deltaTime, 0);
            player.transform.Translate(0, -5f*Time.deltaTime, 0);
        }
        if(countSpawn >= nextToolSpawn){
            GameObject newTool = Instantiate(toolTypes, transform);
            float randomx = Random.Range(2.5f, 9.5f);
            float randomy = Random.Range(16f, 20f);
            newTool.transform.position = new Vector3(randomx, randomy, 0);
            tool = newTool;
            countSpawn = 0;
            nextToolSpawn = Random.Range(60, 100);
        }
    }
    public void spawnPlatform(float y){
        int random = Random.Range(1, plateformTypes.Length + 9);
        if(random >= 1 && random < 7)
            random = 0;
        else if(random == 7 || random == 8)
            random = 1;
        else if(random == 9 || random == 10)
            random = 2;
        else if(random == 11 || random == 12)
            random = 3;
        GameObject platform = Instantiate(plateformTypes[random], transform);
        float randomx = Random.Range(3.1f, 8.9f);
        platform.transform.position = new Vector3(randomx, y, 0);
        platforms[index] = platform;
        
    }   
}
