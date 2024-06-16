using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class buttonLobby : MonoBehaviour
{
    [SerializeField] private Button play, autors, exit, autorsexit;
    [SerializeField] private GameObject creaters;
    public void OnEnable(){
        creaters.gameObject.SetActive(false);

        play.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        exit.onClick.AddListener(Application.Quit);
        autors.onClick.AddListener(AutorsClick);
        autorsexit.onClick.AddListener(AutorsExit);


    }

    private void AutorsClick(){
        play.gameObject.SetActive(false);
        autors.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        creaters.gameObject.SetActive(true);
    }

    private void AutorsExit(){
        play.gameObject.SetActive(true);
        autors.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
        creaters.gameObject.SetActive(false);
    }
}
