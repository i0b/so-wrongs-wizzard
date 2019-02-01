using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameEnded;
    public float GameSpeed = 2.0f;
    public float SpeedIncreaseFactor = 0.1f;
    public float SpawnRate = 6.0f;
    public int TurboFactor = 5;
    public Text ScoreText;
    public Text TimerText;
    public GameObject NormalTowerPrefab;
    public GameObject TrollTowerPrefab;
    public GameObject WizardPrefab;
    public GameObject DementorPrefab;
    public GameObject ItemInvinciblePrefab;
    public GameObject ItemPointsPrefab;
    public GameObject ItemTrollPrefab;
    public GameObject ItemTurboPrefab;
    public GameObject DestroyWidthReferenceGameObject;
    public GameObject PotionTargetArea;
    public GameObject GameOverMenu;
    public GameObject ReplayButton;
    public GameObject CheckButton;
    public Text GameOverScoreText;
    public GameObject HighscoreInput;
    public Text HighscoreTextInputField;
    public GameObject InGameTexts;
    public float TowerSpawnOffset = 2.2f;
    public float ItemDementorSpawnOffset = 3.8f;
    public float ItemTimeout = 10.0f;
    public float ItemMoveSpeed = 80.0f;
    public float EvadeDementorYposition = 3.0f;
    public float DementorWiggleDistance = 0.5f;
    public AudioSource ItemCollectAudioEffect;
    public AudioSource ItemRunoutAudioEffect;
    public ScoreManager ScoreManager;

    public static GameManager Instance;

    public enum GameState
    {
        None,
        Started,
        GameOver
    }

    public enum ActiveItemType
    {
        None,
        Invincible,
        Points,
        Turbo,
        Troll
    }

    private GameState gameState;
    private float lastSpawned;
    private GameObject wizardGameObject;
    private GameObject activeItemObject;
    private float itemTimer;
    private GameObject towerPrefab;
    private bool invincible;
    private float initialGameSpeed;

    private ActiveItemType activeItem;
    public ActiveItemType ActiveItem { get { return activeItem; } }

    private float backgroundWidth;
    public float BackgroundWidth { get { return backgroundWidth; } }

    // defines whether or not a potion is visible on the screen
    private bool potionPresent;
    public bool PotionPresent { set { potionPresent = value; } get { return potionPresent; } }

    private int score = 0;

    public int GetScore() => score;

    public bool GameOver { get { return gameState != GameState.Started; } }

    public float GetSpeed() => GameSpeed;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetGameState(GameState.None);
        initialGameSpeed = GameSpeed;
        backgroundWidth = DestroyWidthReferenceGameObject.GetComponent<SpriteRenderer>().size.x;
    }

    // when a scoring condition is was met the score function should be called
    public void Score()
    {
        if (activeItem == ActiveItemType.Points)
        {
            score += 2;
        }
        else
        {
            score += 1;
        }

        ScoreText.text = score.ToString();
    }

    public void NewGame()
    {
        SetGameState(GameState.Started);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // adds a new highscore value this is called by buttons in the gameOver Menu
    public void NewScore()
    {
        ScoreManager.NewScore(HighscoreTextInputField.text, score);
    }
    
    // changes the internal variables and prepares the views based on the given game state
    public void SetGameState(GameState state)
    {
        switch(state)
        {
            case GameState.None:
                break;
            // -- Start Game ---
            // set all necessary values back to zero, such as score and game speed
            // spawn a wizzard character and set the first tower
            case GameState.Started:
                InGameTexts.SetActive(true);

                GameSpeed = initialGameSpeed;
                gameState = GameState.Started;
                activeItem = ActiveItemType.None;
                wizardGameObject = GameObject.Instantiate(WizardPrefab);
                towerPrefab = NormalTowerPrefab;
                GameObject.Instantiate(towerPrefab);
                lastSpawned = 0;
                itemTimer = 0;
                score = 0;
                invincible = false;
                ScoreText.text = score.ToString();
                break;
            // -- Game Ended due to the character dying --
            // set the view so that the score is shown and the player
            // can restart the game or go back to main menu
            // also: if a new highscore is set alow the input of the player's name
            case GameState.GameOver:
                GameOverMenu.SetActive(true);
                InGameTexts.SetActive(false);

                GameOverScoreText.text = score.ToString();
                HighscoreInput.SetActive(false);

                ResetItemEffect();
                ScoreText.text = "";
                gameState = GameState.GameOver;
                Destroy(wizardGameObject);

                if (score > ScoreManager.GetMinScore())
                {
                    ReplayButton.SetActive(false);
                    CheckButton.SetActive(true);
                    HighscoreInput.SetActive(true);
                }
                else
                {
                    ReplayButton.SetActive(true);
                    CheckButton.SetActive(false);
                    HighscoreInput.SetActive(false);
                }
                break;
        }
    }

    // regularly add new elements to the playing field (coming in from the right) if the
    // game is running

    // handle the position of the potion icon after one has been picked up and handle the turbo potion here
    public void Update()
    {
        if (gameState == GameState.Started)
        {
            Spawn();
        }

        if (activeItem != ActiveItemType.None)
        {
            // move item icon to the top left corner left to the timer text field
            float stepItemObject = ItemMoveSpeed * Time.deltaTime;
            activeItemObject.transform.position = Vector3.MoveTowards(activeItemObject.transform.position, PotionTargetArea.transform.position, stepItemObject);

            // change the game speed after a turbo potion has been picked up
            if (activeItem == ActiveItemType.Turbo)
            {
                float stepWizardObject = ItemMoveSpeed * Time.deltaTime;
                wizardGameObject.transform.position = Vector3.MoveTowards(wizardGameObject.transform.position, Vector3.zero, stepWizardObject);
            }

            itemTimer -= Time.deltaTime;

            // disable the item effect after e.g. 10 seconds
            if (itemTimer <= 0)
            {
                ResetItemEffect();
            }

            // display the amount of time left in the upper right corner (e.g. '9 s')
            else
            {
                TimerText.text = ((int)itemTimer+1).ToString() + " s";
            }
        }
    }

    // undo the item effects, i.e. the game will continue as if the game was without picking up the item
    // the speed is reduced, if it was increased before, the number of poins the player gets is turned back
    // and the width of the towers is set back to normal, also the player is set back to normal size and not
    // invincible after they picked up the respective potion before -- play a sound after that
    private void ResetItemEffect()
    {
        TimerText.text = "";

        switch (activeItem)
        {
            case ActiveItemType.Turbo:
                GameSpeed = GameSpeed / TurboFactor;
                wizardGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                wizardGameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
            case ActiveItemType.Troll:
                towerPrefab = NormalTowerPrefab;
                break;
            case ActiveItemType.Invincible:
                SetInvincible(false);
                break;
        }
        ItemRunoutAudioEffect.Play();
        Destroy(activeItemObject);
        activeItem = ActiveItemType.None;
        potionPresent = false;
    }

    public void AddItem(GameObject prefab, float yPosition)
    {
        Object.Instantiate(prefab, new Vector3(backgroundWidth / 2, yPosition), Quaternion.identity);
    }

    // whenever a new element should be added to the scene (based on a timer) a random element is added
    // right out of the screen space.
    // with a high chance a tower is added, with a lesser chance a dementor or item is added
    // the items' chances are equal, though only a single item can be present on the screen at a given 
    // time; also if a turbo item is active only towers are placed in the center of the screen so the player can
    // move directly through them
    // therefore the chances: 100% tower if turbo; and 50% tower, 50% dementor if a different item is active;
    // then lastly: 50% tower, 50% item or dementor if no item is active with a 50%*25% = 12.5% chance for any item
    private void Spawn()
    {
        lastSpawned += Time.deltaTime;


        if (lastSpawned >= SpawnRate / GameSpeed)
        {
            GameSpeed += SpeedIncreaseFactor;
            int spawnTowerChance = Random.Range(0, 2);

            if (activeItem == ActiveItemType.Turbo)
            {
                AddItem(towerPrefab, 0);
            }
            else if (spawnTowerChance == 0)
            {
                AddItem(towerPrefab, Random.Range(-TowerSpawnOffset, TowerSpawnOffset));
            }
            else
            {
                int spawnDementorChance = Random.Range(0, 2);
                if (spawnDementorChance == 0 || potionPresent)
                {
                    AddItem(DementorPrefab, Random.Range(-ItemDementorSpawnOffset, ItemDementorSpawnOffset));
                }
                else
                {
                    int spawnItem = Random.Range(0, 4);
                    
                    potionPresent = true;

                    switch (spawnItem)
                    {
                        case 0:
                            AddItem(ItemInvinciblePrefab, Random.Range(-ItemDementorSpawnOffset, ItemDementorSpawnOffset));
                            break;
                        case 1:
                            AddItem(ItemPointsPrefab, Random.Range(-ItemDementorSpawnOffset, ItemDementorSpawnOffset));
                            break;
                        case 2:
                            AddItem(ItemTrollPrefab, Random.Range(-ItemDementorSpawnOffset, ItemDementorSpawnOffset));
                            break;
                        case 3:
                            AddItem(ItemTurboPrefab, Random.Range(-ItemDementorSpawnOffset, ItemDementorSpawnOffset));
                            break;
                    }
                }
            }
            
            lastSpawned = 0;
        }
    }

    // play a sound after activating an item and set its timer after removing the components that handled its
    // movement along the x-axes and disabling collision handling
    private void SetActiveItem(ActiveItemType item, GameObject gameObject)
    {
        ItemCollectAudioEffect.Play();
        activeItem = item;
        activeItemObject = gameObject;
        Destroy(activeItemObject.GetComponent<moveAndDestroyElement>());
        Destroy(activeItemObject.GetComponent<Collider2D>());
        itemTimer = ItemTimeout;
    }

    private void SetInvincible(bool invincible)
    {
        this.invincible = invincible;

        if (invincible == true)
        {
            wizardGameObject.transform.localScale /= 2;
        }
        else
        {
            wizardGameObject.transform.localScale *= 2;
        }
    }

    // handle a collision with the wizzard a collidable object. These are: items, towers, dementors and the score area
    // as well as the ground and sky
    // the actual score areas are between the tower gaps and above and below the dementors
    // towers and dementors remove the invicible abiltiy if the respective item was active, usually collisions with them
    // as well collisions with the sky or ground always result in the end of the game
    // after picking up an item its effects are set active
    public void Collision(GameObject wizard, Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "ScoreZone":
                Score();
                Debug.Log("scored!");
                break;
            case "Dementor":
            case "DeadZone":
                if (invincible && wizardGameObject.transform.position.y < 3.7f && wizardGameObject.transform.position.y > -3.7f)
                {
                    ResetItemEffect();
                }
                else if (activeItem != ActiveItemType.Turbo)
                {
                    SetGameState(GameState.GameOver);
                }
                break;
            case "ItemPoints":
                SetActiveItem(ActiveItemType.Points, collision.gameObject);
                break;
            case "ItemInvincible":
                SetInvincible(true);
                SetActiveItem(ActiveItemType.Invincible, collision.gameObject);
                break;
            case "ItemTroll":
                towerPrefab = TrollTowerPrefab;
                SetActiveItem(ActiveItemType.Troll, collision.gameObject);
                break;
            case "ItemTurbo":
                GameSpeed = GameSpeed * TurboFactor;
                wizardGameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                SetActiveItem(ActiveItemType.Turbo, collision.gameObject);
                break;
        }
    }
}
