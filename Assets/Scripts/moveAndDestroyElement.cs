using UnityEngine;


public class moveAndDestroyElement : MonoBehaviour
{
    private GameManager gameManager;
    private float originalYposition;
    private bool dementorMoveUp;

    void Start()
    {
        dementorMoveUp = true;
        gameManager = GameManager.Instance;
        originalYposition = gameObject.transform.position.y;
    }

    void Update()
    {
        if (gameObject.tag == "Dementor")
        {
            if (gameManager.ActiveItem == GameManager.ActiveItemType.Turbo)
            {
                Vector3 dementorPosition = transform.position;
                float stepDementorObject = 3 * Time.deltaTime;

                if (dementorPosition.y >= 0 && dementorPosition.y != gameManager.EvadeDementorYposition)
                {
                    dementorPosition.y = gameManager.EvadeDementorYposition;
                    transform.position = Vector3.MoveTowards(transform.position, dementorPosition, stepDementorObject);
                }
                else if (dementorPosition.y >= 0 && dementorPosition.y != -gameManager.EvadeDementorYposition)
                {
                    dementorPosition.y = -gameManager.EvadeDementorYposition;
                    transform.position = Vector3.MoveTowards(transform.position, dementorPosition, stepDementorObject);
                }
            }

            else
            {
                if (dementorMoveUp && (transform.position.y <= originalYposition - gameManager.DementorWiggleDistance))
                {
                    dementorMoveUp = false;
                }
                else if (!dementorMoveUp && (transform.position.y >= originalYposition + gameManager.DementorWiggleDistance))
                {
                    dementorMoveUp = true;
                }

                Vector3 dementorPosition = transform.position;
                float stepDementorObject = 1 * Time.deltaTime;

                if (dementorMoveUp)
                {
                    dementorPosition.y = originalYposition - gameManager.DementorWiggleDistance;
                    transform.position = Vector3.MoveTowards(transform.position, dementorPosition, stepDementorObject);
                }
                else
                {
                    dementorPosition.y = originalYposition + gameManager.DementorWiggleDistance;
                    transform.position = Vector3.MoveTowards(transform.position, dementorPosition, stepDementorObject);
                }
            }
        }
        // Tower
        else if (gameObject.tag == "ScoreZone")
        {
            if (gameManager.ActiveItem == GameManager.ActiveItemType.Turbo)
            {
                Vector3 treePosition = transform.position;
                float stepTreeObject = 3 * Time.deltaTime;

                if (treePosition.y != 0)
                {
                    treePosition.y = 0;
                    transform.position = Vector3.MoveTowards(transform.position, treePosition, stepTreeObject);
                }
            }
        }

        if (gameManager.GameOver == false)
        {
            Vector3 currentPosition = gameObject.transform.position;

            if (currentPosition.x < -gameManager.BackgroundWidth / 2)
            {
                if (tag == "ItemPoints" || tag == "ItemInvincible" || tag == "ItemTurbo" || tag == "ItemTroll")
                {
                    gameManager.PotionPresent = false;
                }

                Destroy(gameObject);
            }

            gameObject.transform.position = new Vector3(currentPosition.x - gameManager.GameSpeed * Time.deltaTime, currentPosition.y);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}