using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class button : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    private bool pressed = false;
    public Animator anim;
    public UnityEvent Onpressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            anim.SetTrigger("press");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("miniCube"))
        {
            StartCoroutine(disableButton());
            Onpressed.Invoke();
            pressed = true;
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        boxCollider.enabled = true;
        pressed = false;
    }

    private IEnumerator disableButton()
    {
        yield return new WaitForSeconds(1f);
        boxCollider.enabled = false;
    }


}
