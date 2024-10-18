using Project.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.SceneManagement
{
    public class SceneChanger
    {
        private SavingWrapper savingWrapper;
        private ICoroutineRunner coroutineRunner;

        public SceneChanger(SavingWrapper savingWrapper, ICoroutineRunner coroutineRunner) 
        {
            this.savingWrapper = savingWrapper;
            this.coroutineRunner = coroutineRunner;
        }

        public void LoadGameSceneAndLoadData()
        {
            LoadScene("GameScene", LoadData, true);
        }

        private void LoadData(AsyncOperation operation)
        {
            savingWrapper.Load();
        }

        private void LoadScene(string sceneName, Action<AsyncOperation> onOperationComplete = null, bool useLoadingScreen = false)
        {
            coroutineRunner.StartCoroutine(LoadSceneAsync(sceneName, onOperationComplete, useLoadingScreen));
        }
        
        private IEnumerator LoadSceneAsync(string sceneName, Action<AsyncOperation> onOperationComplete = null, bool useLoadingScreen = false)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            if(onOperationComplete != null )
                operation.completed += onOperationComplete;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if(operation.isDone)
                {
                    yield return new WaitForSeconds(0.5f);      
                    break;
                }
            }
            
            if(onOperationComplete != null )
                operation.completed -= onOperationComplete;
        }
    }
}