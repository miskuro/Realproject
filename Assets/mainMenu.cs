using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public void info()
    {
        SceneManager.LoadScene(3);

    }

    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    public void story()
    {
        SceneManager.LoadScene(1);
    }
}
