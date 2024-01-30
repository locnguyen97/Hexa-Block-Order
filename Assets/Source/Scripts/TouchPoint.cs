using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour
{
    private bool isSelected = false;
    public string name;
    public void SetData(Color ic)
    {
        isSelected = false;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = ic;
        name = ic.ToString();
        SetUnCollected();
    }

    public void SetCollected()
    {
        if(isSelected) return;
        isSelected = true;
        transform.localScale = Vector3.one*1.1f;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    public void SetUnCollected()
    {
        isSelected = false;
        transform.localScale = Vector3.one*1.3f;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}