using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public int shots;
    public Text shotCounter;
    public Text levelClear;
    public bool levelIsClear;

    // Start is called before the first frame update
    void Start()
    {
        shots = 0;
        levelIsClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelClear()
    {
        levelClear.enabled = true;
        levelIsClear = true;
    }

    public void IncrementShotCount()
    {
        //  Increment shot counter.
        shots++;
        shotCounter.GetComponent<Text>().text = "Retries: " + shots;
    }

}
