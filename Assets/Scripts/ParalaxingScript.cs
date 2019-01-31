using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxingScript : MonoBehaviour
{
    // In this script two identical game objects are used that optimaly have a width
    // greate than the width visible by the camera. These two object are then moved
    // from left to right according to the speed factor. After one object exits the 
    // viewable area it is destroyed and a new one is created right to the still visible one

    public GameObject displayedObject;
    public float speedFactor = 0.01f;
    private GameObject nextObject;
    private int respawnPos;
    private GameManager gameManager;

    private float gameObjectWidth;

    // creates object right to the second, still visible object
    private void CreateNextObject()
    {
        Vector3 newObjectPosition = displayedObject.transform.position;
        newObjectPosition.x = displayedObject.transform.position.x + gameObjectWidth;
        nextObject = Object.Instantiate(displayedObject, newObjectPosition, Quaternion.identity);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameObjectWidth = displayedObject.GetComponent<SpriteRenderer>().size.x;
        CreateNextObject();
    }

    // move objects and replace off-screen object with new one
    void Update()
    {
        if (displayedObject.transform.position.x <= -gameObjectWidth)
        {
            Vector3 newObjectPosition = displayedObject.transform.position;
            newObjectPosition.x = gameObjectWidth;
            displayedObject.transform.position = newObjectPosition;

            GameObject swapTempGameObject = displayedObject;
            displayedObject = nextObject;
            nextObject = swapTempGameObject;
        }

        Vector3 newDisplayedObjectPosition = new Vector3(displayedObject.transform.position.x - gameManager.GameSpeed * speedFactor * Time.deltaTime, displayedObject.transform.position.y);
        displayedObject.transform.position = newDisplayedObjectPosition;

        Vector3 newNextObjectPosition = new Vector3(nextObject.transform.position.x - gameManager.GameSpeed * speedFactor * Time.deltaTime, nextObject.transform.position.y);
        nextObject.transform.position = newNextObjectPosition;
    }
}