using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] Animator cartAnim;
    private GameObject buttonHolder;
    private GameObject screenFadeOb;
    private Animator screenFadeAnim;

    private void Awake()
    {
        cartAnim.GetComponent<MenuCart>().menuManager = this;
        buttonHolder = canvas.transform.GetChild(0).gameObject;
        screenFadeOb = canvas.transform.GetChild(1).gameObject;
        screenFadeAnim = screenFadeOb.GetComponent<Animator>();
        buttonHolder.SetActive(false);
        cartAnim.gameObject.SetActive(false);
        screenFadeAnim.SetTrigger("FadeIn");
        SetButtons();
        StartCoroutine(WaitUntilStart());
    }

    private void SetButtons()
    {
        buttonHolder.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ButtonAction("Start"));
        buttonHolder.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ButtonAction("Settings"));
        buttonHolder.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ButtonAction("Exit"));
    }

    private IEnumerator WaitUntilStart()
    {
        yield return new WaitForSeconds(3f);
        screenFadeOb.SetActive(false);
        cartAnim.gameObject.SetActive(true);
        cartAnim.SetTrigger("Initialize");
    }

    public void ButtonAction(string animTrigger)
    {
        buttonHolder.SetActive(false);
        cartAnim.SetTrigger(animTrigger);
    }

    public void ShowButtons()
    {
        buttonHolder.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
