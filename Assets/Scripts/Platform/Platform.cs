using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Platform : MonoBehaviour
{
    private float startPosX;
    public float startPosY;
    private int random;
    private int dir; //根據象限決定
    public float turningPoint;
    public int mode;
    private float moveSpeed;
    public float trueStartY;
    public int counts;
    void Start()
    {
        counts = 0;
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        trueStartY = startPosY;
        turningPoint = startPosY;
        mode = 0;
        random = Random.Range(0, 15);
        if(transform.tag == "floor")
            random = 0;
        float tmp;
        if(random >= 10 && random <= 12){ // move on x dir
            dir = Random.Range(0, 2);
            if(dir == 0){
                turningPoint = startPosX + -1 * Random.Range(1f, 3.5f);
                dir = -1;
            }
            else
                turningPoint = startPosX + Random.Range(1f, 3.5f);
            mode = 1;
            if(startPosX > turningPoint){
                tmp = startPosX;
                startPosX = turningPoint;
                turningPoint = tmp;
            }
            moveSpeed = Random.Range(0.5f, 2f);
        }
        else if(random >= 13 && random <= 14){ // move on y dir
            dir = Random.Range(0, 2);
            if(dir == 0){
                turningPoint = startPosY -1 * Random.Range(0.75f, 2.5f);
                dir = -1;
            }
            else
                turningPoint = startPosY + Random.Range(0.75f, 2.5f);
            mode = 2;
            if(startPosY > turningPoint){
                tmp = startPosY;
                startPosY = turningPoint;
                turningPoint = tmp;
            }
            moveSpeed = Random.Range(0.5f, 1.5f);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == 1){ // move on x dir
            transform.Translate(dir * moveSpeed * Time.deltaTime, 0, 0);
            if(transform.position.x <= startPosX)//0表示往左走, 1表示往右
                dir = 1;
            else if(transform.position.x >= turningPoint)
                dir = -1;
        }
        else if(mode == 2){// move on y dir
            transform.Translate(0, dir * moveSpeed * Time.deltaTime, 0);
            if(transform.position.y <= startPosY)//0表示往左走, 1表示往右
                dir = 1;
            else if(transform.position.y >= turningPoint)
                dir = -1;
        }
        
        
    }
}