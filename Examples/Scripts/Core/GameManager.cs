using System.Collections;
using UnityEngine;

namespace CoreFramework.Examples
{
    public class GameManager : Singleton<GameManager>
    {
        public SceneLoader SceneLoader { get; private set; }

        public void Initialize()
        {
            StartCoroutine(InitializationDelay());
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneLoader = new SceneLoader();
        }

        private IEnumerator InitializationDelay()
        {
            yield return new WaitForSeconds(1);

            ConnectionResultSuccess();
        }

        private void ConnectionResultSuccess()
        {
            SceneLoader.LoadLevel(SceneLoader.MAIN_SCENE_NAME);
        }
    }
}
