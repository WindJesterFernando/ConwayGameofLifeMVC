using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    int generationNumber = 1;
    const float TimeToWaitForNextGeneration = 0.5f;

    float elapsedTimeSinceLastGeneration = 0;

    [SerializeField]
    TMP_Text generationNumberText;

    void Start()
    {
        
    }

    void Update()
    {
        elapsedTimeSinceLastGeneration += Time.deltaTime;

        if(elapsedTimeSinceLastGeneration >= TimeToWaitForNextGeneration)
        {
            generationNumber++;
            elapsedTimeSinceLastGeneration -= TimeToWaitForNextGeneration;

            generationNumberText.text = "Generation #" + generationNumber;

        }
    }
}


public class Tile 
{

}
