using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeControl : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public int num;

    public OverviewControl overviewControl;

    public void SwitchCamera()
    {
        if (cam1.activeInHierarchy)
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
        }
        else
        {
            cam2.SetActive(false);
            cam1.SetActive(true);
        }
    }

    public void GetNewPerson()
    {
        if(GameManager.instance.possibleWaifus[num])
        {
            overviewControl.placingCharacter = true;
            overviewControl.storedCharacter = GameManager.instance.possibleWaifus[num];
            num++;
        }
    }

    public void ChangeScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
