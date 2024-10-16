using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniCube : MonoBehaviour

{
    [SerializeField] private Rigidbody2D rb;
    public float launchPower = 6f;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(rb.velocity.x, launchPower);
    }


}
