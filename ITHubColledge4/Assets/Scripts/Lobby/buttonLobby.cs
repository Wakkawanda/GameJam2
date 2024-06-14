using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class buttonLobby : MonoBehaviour
{
    [SerializeField] private Button play, autors, exit;
    public void OnEnable(){

        play.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        exit.onClick.AddListener(Application.Quit);
        autors.onClick.AddListener(AutorsClick);


    }

    private void AutorsClick(){
        play.gameObject.SetActive(false);
        autors.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }
}
