using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutscene1 : MonoBehaviour
{
    public float TimeLeft = 30f;
    // Start is called before the first frame update
    void Start()
    {
        //TimeLeft -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        Debug.Log(TimeLeft);
        if (TimeLeft <= 0f)
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }
    }
}
