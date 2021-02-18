using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text EnemiesRemainingText;
    [SerializeField] private Text HpText;

    // Start is called before the first frame update

    public void UpdateEnemiesRemainingText(int count)
    {
        Text[] texts = FindObjectsOfType<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "Enemies Remaining")
            {
                EnemiesRemainingText = text;
            }
        }
        EnemiesRemainingText.text = "Enemies Remaining: " + count;
    }
    public void UpdateHp(float hp)
    {
        Text[] texts = FindObjectsOfType<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "Hp Text")
            {
                HpText = text;
            }
        }
        HpText.text = "HP :" + hp.ToString();
        Debug.Log("HP Update");
    }
}
