using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject explodeBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player" || collision.tag == "Comrades" || collision.tag == "Enemy")
        {
            Instantiate(explodeBody, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }

        
    }


}
