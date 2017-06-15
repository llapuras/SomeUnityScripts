﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransEffect : MonoBehaviour
{

    public List<GoInfo> GoList;
    private GoInfo go;
    private GoInfo nextgo;
    private Color c;
    private Color nextc;

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public float WaitTimeBeforeFirstLoop = 0f;//第一次循环动画开始之前的等待时间
    public float FadeInTime = 0f;//每张image用来渐入的时间
    public float FadeOutTime = 0f;//每张image用来渐出的时间
    public float StillLifeTime = 2f;//每张image保持静止显示的时间
    public float ImageReloadTime = 3f;//每次播放下一张imgae的间隔时间!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!未实现
    public int   LoopTimes = 2; //循环次数
    //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！

    private float minAlpha = 0.0f;
    private float maxAlpha = 1.0f;
    private float curAlpha = 1.0f;
    private float nextAlpha = 0.0f;
    private int i = 0;
    private int looptime = 0;
    private float WaitUntil = 0f;

    public Transform lib;

    public void OnEnable()
    {
        LoadGo();
    }

    // Use this for initialization
    void Start()
    {
        
        WaitUntil = StillLifeTime + WaitTimeBeforeFirstLoop;
        InitiializeList(GoList);

        c = GoList[0].curImg.color;
        c.a = 1;
        GoList[0].curImg.color = c;
    }

    // Update is called once per frame
    public void Update()
    {
        Trans();
    }


    private void Trans()
    {

        if (looptime >= 1)
        {
            LoopListCheck();

            c = go.curImg.color;
            nextc = go.curImg.color;

            if (Time.time < WaitUntil)//当前物体保持显形
            {
                Debug.Log("1");
                StillLife();
            }
            else
            {
                if (nextAlpha >= maxAlpha)//当前物体变现时
                {
                    Debug.Log("2");
                    if (i == GoList.Count - 1)
                        i = -1;
                    i++;
                    WaitUntil = Time.time + StillLifeTime; //设置新一轮时间限制
                    //设置数据为下一物体做准备
                    curAlpha = 1;
                    nextAlpha = 0;
                }
                else//当前物体逐渐透明，下一物体逐渐现形
                {
                    Debug.Log("3");
                    FadeOut();
                    //Debug.Log(Time.time);
                    Debug.Log(WaitUntil + ImageReloadTime-Time.time);
                    if (Time.time > WaitUntil + ImageReloadTime)
                    {
                        Debug.Log("cww");
                        FadeIn();
                    }
                }

                if (curAlpha >= maxAlpha)//下一物体完全显形
                {
                    if (i == GoList.Count - 1)
                    {
                        looptime--;
                    }
                }
            }

        }

    }

    private void InitiializeList(List<GoInfo> li)
    {
        foreach (GoInfo go in li)
        {
            c = go.curImg.color;
            c.a = 0;
            go.curImg.color = c;
        }

        // don't know why the looptime decrease doublely = - = ???
        looptime = LoopTimes ;
    }

    void LoadGo()
    {
        //Load the image list
        GoList = new List<GoInfo>();
        for (int i = 0; i < lib.childCount; i++)
        {
            GoList.Add(new GoInfo(lib.GetChild(i).name.ToString(), lib.transform.GetChild(i).GetComponent<Image>()));
        }
    }

    private void FadeIn()
    {
        //nextAlpha = 0;
        nextAlpha += Time.deltaTime / FadeInTime;//下一个物体逐渐现形
        nextAlpha = Mathf.Clamp(nextAlpha, minAlpha, maxAlpha);
        nextc.a = nextAlpha;
        nextgo.curImg.color = nextc;
    }

    private void FadeOut()
    {
        //curAlpha = 1;
        curAlpha += Time.deltaTime /FadeOutTime * (-1);//当前物体逐渐消失   
        curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
        c.a = curAlpha;
        go.curImg.color = c;
    }

    private void StillLife()
    {
        c.a = 1;
        go.curImg.color = c;
    }

    private void Kill()
    {

    }

    //Loop the img list
    private void LoopListCheck()
    {
        if (i >= GoList.Count - 1)
        {
            go = GoList[i];
            nextgo = GoList[0];
        }
        else
        {
            go = GoList[i];
            nextgo = GoList[i + 1];
        }
    }

    private void KeepDormantTime()
    {
        c.a = 0;//设置当前obj保持透明
        go.curImg.color = c;
    }

    private void WaitTime()
    {

    }


}

[System.Serializable]
public class GoInfo
{
    public string ID;
    public Image[] imgList;
    public Image curImg;

    private Color co;

    public GoInfo(string id0, Image img)
    {
        ID = id0;
        curImg = img;
    }

}