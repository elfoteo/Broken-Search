using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Health")]
public class PlayerDataSO: ScriptableObject
{
    [SerializeField]private float health;
    public float PlayerHealth { get => health; set{ OnHealthChanged.Invoke();

            health = value;
        } }

    public UnityEvent OnHealthChanged;
}
