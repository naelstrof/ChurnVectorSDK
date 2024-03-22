using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour {
    [SerializeField] private Image circle;
    public void SetProgress(float progress) {
        circle.fillAmount = progress;
    }

    public void SetColor(Color newColor) {
        circle.color = newColor;
    }
}
