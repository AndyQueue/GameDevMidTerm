using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDies : MonoBehaviour
{
    public bool isDead = false;

    public void Dies()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
