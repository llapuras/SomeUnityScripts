using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlphaTransEffect : MonoBehaviour
{

    public List<GoInfo> GoList;
    private GoInfo go;
    private GoInfo nextgo;
    private Color c;
    private Color nextc;

    /* kyzy_4399希望添加的部分 */
    //这些是必须的基础功能
    public float WaitTimeBeforeFirstLoop = 1f;//第一次循环动画开始之前的等待时间
    public float FadeInTime = 1.5f;//每张image用来渐入的时间
    public float FadeOutTime = 1.5f;//每张image用来渐出的时间
    public float StillLifeTime = 2f;//每张image保持静止显示的时间
    public float ImageReloadTime = 3f;//每次播放下一张imgae的间隔时间——这里定义为StillLiFeTime之后开始计算ImageReloadTime
    //正常情况下ImageReloadTime+FadeInTime应该大于FadeOutTime，否则图片FadeOut不完全
    public int LoopTimes = 1;//循环的次数
    public Transform lib;//图片列表父transform
    public bool loopForever;
    //循环结束以后的指令，是否kill这个物体，或者渐变消失

    private bool  isActive;
    private float minAlpha = 0.0f;
    private float maxAlpha = 1.0f;
    private float curAlpha = 1.0f;
    private float nextAlpha = 0.0f;
    private int i = 0;
    private int looptime = 0;
    private float WaitUntil = 0f;

    public void OnEnable()
    {
        LoadGo();
    }

    // Use this for initialization
    void Start()
    {
        isActive = true;
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

        if (LoopTimes >= 1)
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
                    {
                        i = -1;
                    }
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

                    if (Time.time > WaitUntil + ImageReloadTime)
                    {
                        Debug.Log("4");
                        FadeIn();
                    }
                }

                if (curAlpha >= maxAlpha)//下一物体完全显形
                {
                    if (i == GoList.Count - 1)
                    {
                        if (!loopForever)
                            LoopTimes--;
                    }
                }
            }
        }
        else
        {
            go = GoList[GoList.Count - 1];
            FadeOut();
            if (go.curImg.color.a == 0)
                Kill(lib.gameObject);
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
        looptime = LoopTimes;
    }

    void LoadGo()
    {
        //Load the image list
        GoList = new List<GoInfo>();
        for (int i = 0; i < lib.childCount; i++)
        {
            GoList.Add(new GoInfo(lib.GetChild(i).name.ToString(), lib.transform.GetChild(i).GetComponent<Image>()));
            lib.GetChild(i).gameObject.SetActive(true);
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
        curAlpha += Time.deltaTime / FadeOutTime * (-1);//当前物体逐渐消失   
        curAlpha = Mathf.Clamp(curAlpha, minAlpha, maxAlpha);
        c.a = curAlpha;
        go.curImg.color = c;
    }

    private void StillLife()
    {
        c.a = 1;
        go.curImg.color = c;
    }

    private void Kill(GameObject obj)
    {
        Destroy(obj);
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