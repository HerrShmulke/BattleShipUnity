using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text SpeedText;

    void Start()
    {
        Character.OnMove += PlayerOnMove;
    }

    private void OnDestroy()
    {
        Character.OnMove -= PlayerOnMove;
    }

    private void PlayerOnMove(Vector2 velocity)
    {
        SpeedText.text = $"speed: {Mathf.RoundToInt(velocity.magnitude)} m/s";
    }
}
