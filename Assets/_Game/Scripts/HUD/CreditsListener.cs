using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsListener : NumberListener
{
    public override void UpdateText()
    {
        text.text = "" + GameManager.Numbers.Credits;
    }
}
