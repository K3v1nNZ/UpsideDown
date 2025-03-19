using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UpsideDown.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;
        [SerializeField] private CanvasGroup blackFade;
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private Material shaderMaterial;
        [SerializeField] private CanvasGroup whiteFade;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            blackFade.DOFade(0, 0.5f);
        }

        public void PlayButton()
        {
            mainMenuCanvas.interactable = false;
            mainMenuCanvas.blocksRaycasts = false;
            mainMenuCanvas.DOFade(0, 0.5f);
            StartCoroutine(Animation());
        }

        public void OptionsButton()
        {
            //TODO
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        private IEnumerator Animation()
        {
            shaderMaterial.DOFloat(0.1f, "_Zoom", 5).SetEase(Ease.InCubic);
            shaderMaterial.DOFloat(35, "_Speed", 5).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(4.5f);
            whiteFade.DOFade(1f, 0.3f);
            yield return new WaitForSeconds(0.5f);
            shaderMaterial.SetFloat("_Zoom", 1);
            shaderMaterial.SetFloat("_Speed", 10);
            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync("DebugScene");
            sceneAsync.allowSceneActivation = false;
            yield return new WaitForSeconds(2);
            sceneAsync.allowSceneActivation = true;
        }
    }
}