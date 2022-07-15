using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SeedDisplay : MonoBehaviour
{
    private TextMeshProUGUI seedGUI = null;
    [SerializeField] private Seed currSeed = null;
    // Start is called before the first frame update
    private void Awake()
    {
        seedGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        seedGUI.text = currSeed.GameSeed;
    }
}
