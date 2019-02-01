using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class PlayModeTestScript
    {
        [UnityTest]
        public IEnumerator GameManagerInitiated()
        {
            SceneManager.LoadScene(0);
            //yield return new WaitForSeconds(0.5f);
            yield return null;

            GameManager gameManager = GameManager.Instance;

            // prefabs
            Assert.IsNotNull(gameManager.WizardPrefab);
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

            // audio
            Assert.IsNotNull(gameManager.ItemCollectAudioEffect);
            Assert.IsNotNull(gameManager.ItemRunoutAudioEffect);

            // sanity check for wrong initializations
            Assert.GreaterOrEqual(gameManager.GameSpeed, 0);
            Assert.Greater(gameManager.SpeedIncreaseFactor, 0);
            Assert.Greater(gameManager.SpawnRate, 0);
            Assert.Greater(gameManager.TurboFactor, 0);
            Assert.Less(gameManager.ItemDementorSpawnOffset, gameManager.TowerSpawnOffset);
            Assert.Greater(gameManager.ItemTimeout, 0);
            Assert.Greater(gameManager.ItemMoveSpeed, 0);
            Assert.Greater(gameManager.EvadeDementorYposition, 1);
        }

        [UnityTest]
        public IEnumerator ScoreManagerInitiated()
        {
            SceneManager.LoadScene(0);
            //yield return new WaitForSeconds(0.5f);
            yield return null;

            ScoreManager scoreManager = GameObject.Find("ScoreManagerObject").GetComponent<ScoreManager>() as ScoreManager;
            
            Assert.IsNotNull(scoreManager);

            Assert.IsNotNull(scoreManager.FirstPlayerName);
            Assert.IsNotNull(scoreManager.FirstPlayerScore);
            Assert.IsNotNull(scoreManager.SecondPlayerName);
            Assert.IsNotNull(scoreManager.SecondPlayerScore);
            Assert.IsNotNull(scoreManager.ThirdPlayerName);
            Assert.IsNotNull(scoreManager.ThirdPlayerScore);
            Assert.IsNotNull(scoreManager.FourthPlayerName);
            Assert.IsNotNull(scoreManager.FourthPlayerScore);
            Assert.IsNotNull(scoreManager.FifthPlayerName);
            Assert.IsNotNull(scoreManager.FifthPlayerScore);
            Assert.IsNotNull(scoreManager.NameInput);
        }

        [UnityTest]
        public IEnumerator AllObjectseSetEnabledDisabled()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameManager gameManager = GameManager.Instance;

            Assert.IsTrue(GameObject.Find("MainMenu").activeInHierarchy);
            Assert.IsTrue(GameObject.Find("Environment").activeInHierarchy);
            Assert.IsTrue(GameObject.Find("Background").activeInHierarchy);
            // if the following game objects have not been activated they have not been created, yet
            Assert.IsNull(GameObject.Find("HelpMenu"));
            Assert.IsNull(GameObject.Find("GameOverMenu"));
            Assert.IsNull(GameObject.Find("HighscoreMenu"));
        }

        [UnityTest]
        public IEnumerator ScoreSavedProperly()
        {
            SceneManager.LoadScene(0);
            yield return null;
            
            ScoreManager scoreManager = ScoreManager.Instance;
            scoreManager.ResetScore();

            string nameString = "TestStringInput";
            int scoreValue = 424242;

            scoreManager.NewScore(nameString, scoreValue);
            scoreManager.UpdateScoreboard();

            Assert.IsTrue(scoreManager.FirstPlayerName.text.CompareTo(nameString) == 0);
            Assert.IsTrue(scoreManager.FirstPlayerScore.text.CompareTo(scoreValue.ToString()) == 0);
            
            scoreManager.ResetScore();
        }

        [UnityTest]
        public IEnumerator GameOverFunctionInGameManager()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameManager gameManager = GameManager.Instance;

            Assert.IsTrue(gameManager.GameOver);

            GameObject.Find("PlayButton").GetComponent<Button>().onClick.Invoke();
            yield return null;

            Assert.IsFalse(gameManager.GameOver);
        }


        //                            VISUAL TESTS                            //

        [UnityTest]
        public IEnumerator CollectItemInvincible()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameObject.Find("PlayButton").GetComponent<Button>().onClick.Invoke();

            GameManager gameManager = GameManager.Instance;

            Object.Instantiate(gameManager.ItemInvinciblePrefab, new Vector3(1.0f, 0.0f), Quaternion.identity);
            gameManager.PotionPresent = true;

            WizardScript tapController = WizardScript.Instance;
            yield return null;

            yield return new WaitForSeconds(0.5f);
            tapController.Tap();

            for (int i=0; i<15; i++)
            {
                yield return new WaitForSeconds(0.95f);
                tapController.Tap();
            }
        }

        [UnityTest]
        public IEnumerator CollectItemTurbo()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameObject.Find("PlayButton").GetComponent<Button>().onClick.Invoke();

            GameManager gameManager = GameManager.Instance;

            Object.Instantiate(gameManager.ItemTurboPrefab, new Vector3(2.0f, 0.0f), Quaternion.identity);
            gameManager.PotionPresent = true;

            WizardScript tapController = WizardScript.Instance;
            yield return null;

            yield return new WaitForSeconds(0.5f);
            tapController.Tap();

            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.95f);
                tapController.Tap();
            }
        }

        [UnityTest]
        public IEnumerator CollectItemTroll()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameObject.Find("PlayButton").GetComponent<Button>().onClick.Invoke();

            GameManager gameManager = GameManager.Instance;

            Object.Instantiate(gameManager.ItemTrollPrefab, new Vector3(1.0f, 0.0f), Quaternion.identity);
            gameManager.PotionPresent = true;

            WizardScript tapController = WizardScript.Instance;
            yield return null;

            yield return new WaitForSeconds(0.5f);
            tapController.Tap();

            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(0.95f);
                tapController.Tap();
            }
        }

        [UnityTest]
        public IEnumerator CollectItemPoints()
        {
            SceneManager.LoadScene(0);
            yield return null;

            GameObject.Find("PlayButton").GetComponent<Button>().onClick.Invoke();

            GameManager gameManager = GameManager.Instance;

            Object.Instantiate(gameManager.ItemPointsPrefab, new Vector3(1.0f, 0.0f), Quaternion.identity);
            gameManager.PotionPresent = true;

            WizardScript tapController = WizardScript.Instance;
            yield return null;

            yield return new WaitForSeconds(0.5f);
            tapController.Tap();

            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(0.95f);
                tapController.Tap();
            }
        }
    }
}
