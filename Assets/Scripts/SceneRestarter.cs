using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneRestarter : MonoBehaviour
{
    public void SceneRestart()
    {
        Debug.Log("asdsd");
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
