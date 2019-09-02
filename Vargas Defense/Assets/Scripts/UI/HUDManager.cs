using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI resourcesLabel;
    public TextMeshProUGUI playerLifeLabel;
    public TextMeshProUGUI goalLifeLabel;

    [Header("References from Gameplay")]
    public PlayerStats player;
    public Buildable goal;
    public FPSBuilderManager builder;

    private void Update() {
        resourcesLabel.text = builder.currentResources.ToString();
        playerLifeLabel.text = player.currentLife.ToString();
        goalLifeLabel.text = goal.currentLife.ToString();
    }
}
