using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TMP_Text btn_text;

    void Start()
    {
        if (pauseMenu == null)
        {
            Debug.Log("Pause Menu nao encontrado");
        }
    }

    public void ShowPauseMenu()
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.SetActive(false);
            btn_text.text = "Menu";
        }
        else
        {
            pauseMenu.SetActive(true);
            btn_text.text = "Close";
        }
    }
}
