using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreFramework.SceneLoading
{
    public abstract class CoreSceneLoader : BaseView
    {
        protected abstract string BootSceneName { get; }

        public System.Action OnSceneLoadingComplete;
        public System.Action OnSceneUnloadingComplete;

        protected string m_CurrentLoadingLevel = string.Empty;
        protected string m_CurrentUnloadingLevel = string.Empty;

        public CoreSceneLoader()
        {
            OnSceneLoadingComplete += SceneLoadCompleteHandler;
            OnSceneUnloadingComplete += SceneUnloadCompleteHandler;
        }

        #region Load

        public void LoadLevel(string levelName, bool noFade = false)
        {
            m_CurrentLoadingLevel = levelName;

            if (!noFade && SceneLoadingManager.Instance != null)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnLoadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
                Load(levelName);
        }

        private void FadeInFinishedOnLoadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnLoadHandler;
            Load(m_CurrentLoadingLevel);
        }

        private void Load(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (asyncOperation != null)
                asyncOperation.completed += LoadOperationComplete;
            else
                Debug.LogError($"Unable to load level {levelName}");
        }

        private void LoadOperationComplete(AsyncOperation asyncOperation) => OnSceneLoadingComplete?.Invoke();

        protected abstract void SceneLoadCompleteHandler();

        protected abstract void SceneUnloadCompleteHandler();

        #endregion

        #region Unload

        public void UnloadLevel(string levelName, bool noFade = false)
        {
            m_CurrentUnloadingLevel = levelName;

            if (!noFade)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnUnloadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
            {
                ExecuteSceneUnloadTransition();
            }
        }

        private void FadeInFinishedOnUnloadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnUnloadHandler;
            ExecuteSceneUnloadTransition();
        }

        private void ExecuteSceneUnloadTransition()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(BootSceneName));
            Unload(m_CurrentUnloadingLevel);
        }

        private void Unload(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(levelName);
            if (asyncOperation != null)
                asyncOperation.completed += UnloadOperationComplete;
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation) => OnSceneUnloadingComplete?.Invoke();

        #endregion
    }
}
