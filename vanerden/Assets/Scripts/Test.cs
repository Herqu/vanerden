using Cinemachine;
using UnityEngine;

public class Test : MonoBehaviour
{


    public void Update()
    {
        transform.Translate(Vector2.up* Time.deltaTime );
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }


}
