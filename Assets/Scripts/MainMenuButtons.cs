using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mm;
    
    public GameObject cm;
    
    void Start() {
        MainMenuButton();
    }

    public void PlayNowButton() {
        
    }
    
    public void CreditsButton() {
        mm.SetActive(false);
        cm.SetActive(true);
    }

    public void MainMenuButton() {
        mm.SetActive(true);
        cm.SetActive(false);
    }

    public void QuitButton() {
        Application.Quit();
    }
}