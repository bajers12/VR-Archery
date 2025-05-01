using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    void Update()
    {
        lifeText.text = "Remaining life: " + EnemyTarget.GetCurrentLife();
    }
}
