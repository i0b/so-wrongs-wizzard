using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class TestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            
            var loadSceneOperation = SceneManager.LoadSceneAsync("FlappyWizard");

            loadSceneOperation.allowSceneActivation = true;

            while (!loadSceneOperation.isDone)
            {
                yield return null;
            }

            GameManager gameManager = GameManager.Instance;

            // prefabs
            Assert.IsNotNull(gameManager.WizardPrefab);
            /*
            Assert.IsNotNull(gameManager.NormalTowerPrefab);
            Assert.IsNotNull(gameManager.TrollTowerPrefab);
            Assert.IsNotNull(gameManager.DementorPrefab);
            Assert.IsNotNull(gameManager.ItemInvinciblePrefab);
            Assert.IsNotNull(gameManager.ItemPointsPrefab);
            Assert.IsNotNull(gameManager.ItemTrollPrefab);
            Assert.IsNotNull(gameManager.ItemTurboPrefab);

            // text areas
            Assert.IsNotNull(gameManager.ScoreText);
            Assert.IsNotNull(gameManager.TimerText);
            Assert.IsNotNull(gameManager.GameOverScoreText);
            Assert.IsNotNull(gameManager.HighscoreTextInputField);

            // other game objects
            Assert.IsNotNull(gameManager.DestroyWidthReferenceGameObject);
            Assert.IsNotNull(gameManager.PotionTargetArea);
            Assert.IsNotNull(gameManager.ScoreManager);

            // buttons
            Assert.IsNotNull(gameManager.ReplayButton);
            Assert.IsNotNull(gameManager.CheckButton);

            // menues and overlays
            Assert.IsNotNull(gameManager.GameOverMenu);
            */
        }
    }
}
