using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UpsideDown.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;
        private static readonly int Zoom = Shader.PropertyToID("_Zoom");
        private static readonly int Speed = Shader.PropertyToID("_Speed");
        [SerializeField] private CanvasGroup blackFade;
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private CanvasGroup optionsCanvas;
        [SerializeField] private Material shaderMaterial;
        [SerializeField] private CanvasGroup whiteFade;
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private Slider masterVolumeSlider;

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
            shaderMaterial.SetFloat(Zoom, 1);
            shaderMaterial.SetFloat(Speed, 10);
            
            if (!PlayerPrefs.HasKey("MasterVol"))
            {
                PlayerPrefs.SetFloat("MasterVol", 0.5f);
            }

            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol");
            masterAudioMixer.SetFloat("MasterVol", Mathf.Log10(masterVolumeSlider.value) * 20);
            
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
            mainMenuCanvas.interactable = false;
            mainMenuCanvas.blocksRaycasts = false;
            mainMenuCanvas.DOFade(0, 0.3f);
            optionsCanvas.DOFade(1, 0.3f).OnComplete(() =>
            {
                optionsCanvas.interactable = true;
                optionsCanvas.blocksRaycasts = true;
            });
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void MasterAudioSlider()
        {
            masterAudioMixer.SetFloat("MasterVol", Mathf.Log10(masterVolumeSlider.value) * 20);
            PlayerPrefs.SetFloat("MasterVol", masterVolumeSlider.value);
        }

        public void OptionsBackButton()
        {
            PlayerPrefs.Save();
            optionsCanvas.interactable = false;
            optionsCanvas.blocksRaycasts = false;
            optionsCanvas.DOFade(0, 0.3f);
            mainMenuCanvas.DOFade(1, 0.3f).OnComplete(() =>
            {
                mainMenuCanvas.interactable = true;
                mainMenuCanvas.blocksRaycasts = true;
            });
        }

        private IEnumerator Animation()
        {
            shaderMaterial.DOFloat(0.1f, Zoom, 5).SetEase(Ease.InCubic);
            shaderMaterial.DOFloat(35, Speed, 5).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(4.5f);
            whiteFade.DOFade(1f, 0.3f);
            yield return new WaitForSeconds(0.5f);
            shaderMaterial.SetFloat(Zoom, 1);
            shaderMaterial.SetFloat(Speed, 10);
            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync("DebugScene");
            sceneAsync.allowSceneActivation = false;
            yield return new WaitForSeconds(2);
            sceneAsync.allowSceneActivation = true;
        }
    }
}