using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    [Header("当新建了一个soldier")]

    public Controller chara;
    public int bloodNum;
    public int maxBulletNum = 40;
    public int bulletNum;
    public Text bloodText;
    public Text bulletText;
    public Scrollbar bulletScrollBar;
    public Transform bloodUI;
    public GameObject HeartPrefab;

    public void ChangeChara(Controller newChara)
    {
        chara = newChara;
    }

    private void Start()
    {

        bloodNum = chara.bloodNum;
        bloodText.text = bloodNum.ToString();
        foreach (Transform UIicon in bloodUI)
        {
            if (UIicon.tag == "UIIcon")
            {
                Destroy(UIicon.gameObject);
            }
        }
        for (int i = 0; i < bloodNum; i++)
        {
            Instantiate(HeartPrefab, bloodUI).tag = "UIIcon";

        }


        bulletNum = chara.bulletNum;
        bulletText.text = bulletNum.ToString();
        float ratio = (float)bulletNum / (float)maxBulletNum;
        bulletScrollBar.value = ratio;
    }

    private void Update()
    {



        if(chara.bloodNum != bloodNum)
        {

            bloodNum = chara.bloodNum;
            bloodText.text = bloodNum.ToString();
            foreach(Transform UIicon in bloodUI)
            {
                if(UIicon.tag== "UIIcon")
                {
                    Destroy(UIicon.gameObject);
                }
            }
            for (int i = 0; i < bloodNum; i++)
            {
                Instantiate(HeartPrefab, bloodUI);

            }
        }

        if (chara.bulletNum != bulletNum)
        {

            bulletNum = chara.bulletNum;
            bulletText.text = bulletNum.ToString();
            float ratio = bulletNum / maxBulletNum;
            bulletScrollBar.value = ratio;
            
        }



    }

    public void AddKillNum()
    {
       Instantiate(skullPrefab,skullUI).transform.SetSiblingIndex(0);
        killNum++;
        killText.text = killNum.ToString();
    }

    public Transform skullUI;
    public int killNum = 0;
    public Text killText;
    public GameObject skullPrefab;




}
