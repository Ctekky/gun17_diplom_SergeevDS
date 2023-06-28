using System.Collections;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Metroidvania.UI
{
    public class UIEndMenu : MonoBehaviour, ISaveAndLoad
    {
        [SerializeField] private string sceneName = "Level_1";
        [SerializeField] private TextMeshProUGUI text;
        [Inject] private SaveManager _saveManager;
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField, Range(1, 10)] private float fadeTimer;
        private string _loadScene;
        private float _playingTime;

        private void Start()
        {
            text.text = "Thanks for playing. Your play time: " + Mathf.Round(_playingTime / 60) + " minutes";
        }

        public void NewGame()
        {
            _saveManager.DeleteSavedData();
            StartCoroutine(LoadSceneWithFade(fadeTimer, sceneName));
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private IEnumerator LoadSceneWithFade(float delay, string scene)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(scene);
        }

        public void LoadData(GameData.GameData gameData)
        {
            _playingTime = gameData.playingTime;
            text.text = "Thanks for playing. Your play time: " + Mathf.Round(_playingTime / 60) + " minutes";
        }

        public void SaveData(ref GameData.GameData gameData)
        {
        }
    }
}