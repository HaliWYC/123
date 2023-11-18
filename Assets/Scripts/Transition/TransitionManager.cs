using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ShanHai_IsolatedCity.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;

        private CanvasGroup fadeCanvasGroup;

        private bool isFade;

        private void OnEnable()
        {
            EventHandler.transitionEvent += onTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.transitionEvent -= onTransitionEvent;
        }

        private void onTransitionEvent(string sceneToGo, Vector3 pos)
        {
            if(!isFade)
                StartCoroutine(transtion(sceneToGo, pos));
        }

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return StartCoroutine(loadSceneSetActive(startSceneName));
            EventHandler.callAfterSceneLoadedEvent();
        }

        /// <summary>
        /// Translate scene
        /// </summary>
        /// <param name="sceneName">Target scene name</param>
        /// <param name="targetPosition">Target position</param>
        /// <returns></returns>
        private IEnumerator transtion(string sceneName, Vector3 targetPosition)
        {
            EventHandler.callBeforeSceneUnloadEvent();
            yield return fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            yield return loadSceneSetActive(sceneName);

            //Move Player Position
            EventHandler.callMoveToPosition(targetPosition);
            EventHandler.callAfterSceneLoadedEvent();
            yield return fade(0);
            EventHandler.callAllowPlayerInputEvent(true);
        }

        /// <summary>
        /// Load scene and set active
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns></returns>
        private IEnumerator loadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }

        private IEnumerator fade(float targetAlpha)
        {
            isFade = true;
            EventHandler.callAllowPlayerInputEvent(false);
            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}
