using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    private float health = 100;
    private int xpForNextLevel = 0;
    private int currentXp = 0;
    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        xpForNextLevel = GetXpForLevel(currentLevel);
    }

    private int GetXpForLevel(int currentLevel)
    {
        double a = 1.2F;
        double b = 0;
        double c = 5;
        return (int)(a * currentLevel * (currentLevel * .1) + b * currentLevel + c);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentXp >= xpForNextLevel)
        {
            currentXp -= xpForNextLevel;
            currentLevel++;
            xpForNextLevel = GetXpForLevel(currentLevel);
        }
    }
}
