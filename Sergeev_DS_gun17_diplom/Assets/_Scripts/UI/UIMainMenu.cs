using System.Collections;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Metroidvania.UI
{
    public class UIMainMenu : MonoBehaviour, ISaveAndLoad
    {
        [SerializeField] private string sceneName = "Level_1";
        [Inject] private SaveManager _saveManager;
        [SerializeField] private Button buttonContinue;
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField, Range(1, 10)] private float fadeTimer;
        private string _loadScene;

        private void Start()
        {
            buttonContinue.gameObject.SetActive(_saveManager.CheckForSavedData());
        }

        public void NewGame()
        {
            _saveManager.DeleteSavedData();
            StartCoroutine(LoadSceneWithFade(fadeTimer, sceneName));
        }

        public void Continue()
        {
            _saveManager.LoadGame();
            StartCoroutine(LoadSceneWithFade(fadeTimer, _loadScene));
        }
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        public void LoadData(GameData.GameData gameData)
        {
            _loadScene = gameData.lastScene;
        }
        public void SaveData(ref GameData.GameData gameData)
        {
        }

        private IEnumerator LoadSceneWithFade(float delay, string scene)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(scene);
        }
    }
    
}
