using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("UI").GetComponent<UIcontroller>();
        comrades.AddRange (GameObject.FindGameObjectsWithTag("Player"));
        comrades.AddRange(GameObject.FindGameObjectsWithTag("Comrades"));
        enemies.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
    }

    public List<GameObject> comrades = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> neutrality = new List<GameObject>();
    public UIcontroller uiController;

    public void Remove(CharacterCamp camp,GameObject obj)
    {
        switch (camp)
        {
            case CharacterCamp.enemy:
                enemies.Remove(obj);
                uiController.AddKillNum();
                break;
            case CharacterCamp.comrades:
                comrades.Remove(obj);
                break;
        }
    }

    [Header("生成新的玩家。")]
    public GameObject playerSoldier;
    public void NewPlayer()
    {
        comrades.Add(Instantiate(playerSoldier, transform.position, transform.rotation));
    }



    ///限制数
    public int maxcomNum = 20;
    public int maxenemyNum = 50;

}
