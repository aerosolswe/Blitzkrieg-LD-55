using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreListener : NumberListener
{
    public override void UpdateText()
    {
        text.text = "" + GameManager.Numbers.Score;
    }
}
