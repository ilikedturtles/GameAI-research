//This class handles seeding our procedural generation based on the given input string

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string GameSeed = "Default";
    public int CurrentSeed = 0;

    public bool randomSeed = false;
    private string randString;
    public int seedLength = 5;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // Start is called before the first frame update
    private void Awake()
    {
        if(randomSeed)
        {
            genRandomString(seedLength);
        }
        CurrentSeed = GameSeed.GetHashCode();
        Random.InitState(CurrentSeed);
    }

    // Generates a random string with a given size.    
    private void genRandomString(int length)
    {
        // Choosing the size of string
        string str = "";
        for (int i = 0; i < length; ++i)
        {
            // Appending the letter to string.
            str = str + (chars[Random.Range(0, chars.Length)]);
        }

        GameSeed = new string(str);
    }

    //check for inputs
    private void Update()
    {
        if(seedLength != GameSeed.Length)
        {
            seedLength = GameSeed.Length;
        }
    }
}
