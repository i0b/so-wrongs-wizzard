using UnityEngine;

public class WizardScript: MonoBehaviour
{
    public float tapForce = 250;
    public float tiltSmooth = 5;
    private GameManager gameManager;
    //public Vector3 startPosition;

    private Quaternion downRotation;
    private Quaternion forwardRotation;

    public static WizardScript Instance;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        downRotation = Quaternion.Euler(0, 0, -50);
        forwardRotation = Quaternion.Euler(0, 0, 20);
    }

    void Update()
    {
        // user input
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            Tap();
        }

        // debug keys: number keys 1-4 create a potion, number key 5 creates a dementor
        if (!gameManager.PotionPresent)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameManager.AddItem(gameManager.ItemInvinciblePrefab, 0.0f);
                gameManager.PotionPresent = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gameManager.AddItem(gameManager.ItemPointsPrefab, 0.0f);
                gameManager.PotionPresent = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gameManager.AddItem(gameManager.ItemTrollPrefab, 0.0f);
                gameManager.PotionPresent = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                gameManager.AddItem(gameManager.ItemTurboPrefab, 0.0f);
                gameManager.PotionPresent = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gameManager.AddItem(gameManager.DementorPrefab, 0.0f);
        }
    }

    // rotate the wizard forward regularly
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wizard colided with " + collision.gameObject.tag);
        gameManager.Collision(gameObject, collision);
    }

    // Applies upwards directed force to the wizard
    public void Tap()
    {
        if (gameManager.GameOver == false)
        {
            transform.rotation = forwardRotation;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * tapForce, ForceMode2D.Force);
        }
    }
}