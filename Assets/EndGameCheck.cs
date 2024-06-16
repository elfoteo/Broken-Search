using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndGameCheck : MonoBehaviour
{
    public GameObject boss;
    public GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.IsDestroyed())
        {
            Destroy(wall);
        }
    }
}
