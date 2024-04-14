using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberListener : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;

    private void FixedUpdate()
    {
        UpdateText();
    }

    public virtual void UpdateText()
    {

    }
}
