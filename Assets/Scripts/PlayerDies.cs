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

        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
