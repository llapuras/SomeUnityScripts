using System.Collections;
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

    public float varifySpeed = 0.5f;
    public float aTime = 5f;//每个物体保持出现的时间
    public float dTime = 5f;
    public int LoopTime = 2; //循环次数

    private float minAlpha = 0.0f;
    private float maxAlpha = .9f;
    private float curAlpha = 1.0f;
    private float nextAlpha = 0.0f;
    private int i = 0;
    private int looptime = 0;

    public Transform lib;

    public void OnEnable()
    {
        LoadGo();
    }

    // Use this for initialization
    void Start()
    {
        InitiializeList(GoList);
    }

    // Update is called once per frame
    public void Update()
    {
        Trans();
    }

    private void InitiializeList(List<GoInfo> li) {
        foreach (GoInfo go in li)
        {
            c = go.curImg.color;
            c.a = 0;
            go.curImg.color = c;
        }
        // don't know why the looptime decrease doublely = - = ???
        looptime = LoopTime * 2;
    }

    void LoadGo()
    {
        //Load the image list
        GoList = new List<GoInfo>();
        for (int i = 0; i < lib.childCount; i++) {
            GoList.Add(new GoInfo(lib.GetChild(i).name.ToString(),lib.transform.GetChild(i).GetComponent<Image>()));
        }
        Debug.Log(GoList.Count);
    }

    private void FadeIn() {
        //nextAlpha = 0;
        nextAlpha += Time.deltaTime * varifySpeed;//下一个物体逐渐现形
        nextAlpha = Mathf.Clamp(nextAlpha, minAlpha, maxAlpha);
        nextc.a = nextAlpha;
        nextgo.curImg.color = nextc;
    }

    private void FadeOut() {
        //curAlpha = 1;
        curAlpha += Time.deltaTime * varifySpeed * (-1);//当前物体逐渐消失   
        curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
        c.a = curAlpha;
        go.curImg.color = c;
    }

    private void StillLife() {
        c.a = 1;
        go.curImg.color = c;
    }

    private void Kill() {

    }

    //Loop the img list
    private void LoopListCheck() {
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

    private void KeepDormantTime() {
        c.a = 0;//设置当前obj保持透明
        go.curImg.color = c;
    }

    private void WaitTime() {

    }

    private void Trans()
    {

        if (looptime >= 1)
        {
            LoopListCheck();

            c = go.curImg.color;
            nextc = go.curImg.color;

            if (Time.time < aTime)//当前物体保持显形
            {
                StillLife();
            }
            else if (Time.time >= aTime)
            {
                if (curAlpha <= minAlpha)//当前物体渐变到不透明时
                {
                    KeepDormantTime();

                    if (i == GoList.Count - 1)
                        i = -1;
                    i++;
                    aTime = Time.time + dTime; //设置新一轮时间限制
                    //设置数据为下一物体做准备
                    curAlpha = 1;
                    nextAlpha = 0;
                }
                else//当前物体逐渐透明，下一物体逐渐现形
                {
                    if (looptime <= 1)
                    {
                        FadeOut();  
                    }
                    else
                    {
                        FadeOut();
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

}

[System.Serializable]
public class GoInfo
{
    public string ID;
    public Image[] imgList;
    public Image curImg;

    private Color co;

    public GoInfo(string id0,Image img)
    {
        ID = id0;
        curImg = img;    
    }

}