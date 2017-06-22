using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class AnimationControl : MonoBehaviour
{

    [SerializeField]
    string[] animationNames;
    [SerializeField]
    Text animationNameText;
    [SerializeField]
    RuntimeAnimatorController[] animationContoller;

    int animationNumber = 0;
    int playingAnimationNumber = 0;

    void Start()
    {
        SetText();
        SetAnimator();
        PlayAnimation();
    }

    void SetText()
    {
        animationNameText.text = animationNames[animationNumber];
    }

    void SetAnimator()
    {
        List<Transform> lists = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            lists.Add(transform.GetChild(i));
        }

        for (int j = 0; j < lists.Count; j++)
        {
            for (int i = 0; i < lists.Count - 1; i++)
            {
                Transform temp = null;
                int num1 = GetNumFromText(lists[i].name);
                int num2 = GetNumFromText(lists[i + 1].name);
                if (num1 > num2)
                {
                    temp = lists[i];
                    lists[i] = lists[i + 1];
                    lists[i + 1] = temp;
                }
            }
        }

        for (int i = 0; i < lists.Count; i++)
        {
            lists[i].GetComponent<Animator>().runtimeAnimatorController = animationContoller[i];
        }

        Debug.Log(lists[0].name);
    }

    void PlayAnimation()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.SetBool(animationNames[playingAnimationNumber], false);
            anim.SetBool(animationNames[animationNumber], true);
        }

        Debug.Log(animationNames[playingAnimationNumber]);
        playingAnimationNumber = animationNumber;

    }

    public void OnButtonNext()
    {
        animationNumber++;
        if (animationNumber > animationNames.Length - 1) animationNumber = 0;
        SetText();
        PlayAnimation();
    }

    public void OnButtonBack()
    {
        animationNumber--;
        if (animationNumber < 0) animationNumber = animationNames.Length - 1;
        SetText();
        PlayAnimation();
    }

    int GetNumFromText(string text)
    {
        Regex re = new Regex(@"[^0-9]");
        return int.Parse(re.Replace(text, ""));
    }
}
