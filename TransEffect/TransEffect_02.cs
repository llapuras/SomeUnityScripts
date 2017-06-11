using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransEffect : MonoBehaviour
{
    public Transform lib;
    public List<GoInfo> GoList;
    public float varifySpeed = 0.5f;
    public float aTime = 5f;//每个物体保持出现的时间
    public float dTime = 5f;//第一张图片第一轮循环时出现时间

    private float minAlpha = 0.0f;
    private float maxAlpha = .9f;
    private float curAlpha = 1.0f;
    private float nextAlpha = 0.0f;
    private int i = 0;



    public void OnEnable()
    {
        LoadGo();
    }

    // Use this for initialization
    void Start()
    {
        //初始化全List隐形
        foreach (GoInfo go in GoList)
        {
                Color c = go.curImg.color;
                c.a = 0;
                go.curImg.color = c;
            
        }
    }

    // Update is called once per frame
    public void Update()
    {
        Trans();
    }


    void LoadGo()
    {
        //添加图片列表
        GoList = new List<GoInfo>();
        for (int i = 0; i < lib.childCount; i++) {
            GoList.Add(new GoInfo(lib.GetChild(i).name.ToString(),lib.transform.GetChild(i).GetComponent<Image>()));
        }
        Debug.Log(GoList.Count);
    }

private void Trans()
    {

        Debug.Log(i);
        GoInfo go;
        GoInfo nextgo;

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


        Color c = go.curImg.color;
        Color nextc = go.curImg.color;

        if (Time.time < aTime)//当前物体保持显形
        {
            c.a = 1;
            go.curImg.color = c;
        }
        else if (Time.time >= aTime)
        {
            curAlpha += Time.deltaTime * varifySpeed * (-1);//当前物体逐渐消失
            nextAlpha += Time.deltaTime * varifySpeed;//下一个物体逐渐现形

            if (curAlpha <= minAlpha)//当前物体渐变到不透明时
            {
                c.a = 0;//设置当前obj保持透明
                go.curImg.color = c;

                if (i == GoList.Count - 1)
                    i = -1;
                i++;
                
                //设置数据为下一物体做准备
                curAlpha = 1;
                nextAlpha = 0;
            }

            else//当前物体逐渐透明，下一物体逐渐现形
            {
                curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
                nextAlpha = Mathf.Clamp(nextAlpha, minAlpha, maxAlpha);
                c.a = curAlpha;
                nextc.a = nextAlpha;
                go.curImg.color = c;
                nextgo.curImg.color = nextc;

            }

            if (curAlpha >= maxAlpha)//下一物体完全显形
            {
                aTime = Time.time + dTime; //设置新一轮时间限制
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