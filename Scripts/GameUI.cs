using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]private Text SpeedText = null;

    public void PlayerMoveListener(Vector2 velocity)
    {
        SpeedText.text = $"speed: {Mathf.RoundToInt(velocity.magnitude)} m/s";
    }
}
