using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private bool button1 = false;
    private bool button2 = false;
    private bool button3 = false;
    private bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        open2();
        if (moving)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }

        if (button3)
        {
            moving = true;
        }

    }

    public void destroy()
    {
        moving = true;
        StartCoroutine(noMoreGoingDown());
    }

    private IEnumerator noMoreGoingDown()
    {
        yield return new WaitForSeconds(3f);
        moving = false;
    }


    public void open2()
    {

        if (button1 && button2)
        {
            moving = true;
            StartCoroutine(noMoreGoingDown());
        }
    }

    public void button1pressed()
    {
        button1 = true;
    }

    public void button2pressed()
    {
        button2 = true;
    }

    public void button3pressed()
    {
        button3 = true;
    }
}
