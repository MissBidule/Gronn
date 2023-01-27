using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //ATTRIBUTES 
    public SliderUI carbonEmissionUI;
    public SliderUI carbonBudgetUI;
    public SliderUI yearUI;
    public Text fedPeopleUI;
    public Text peopleUI;

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void InitializeUI()
    {
        carbonEmissionUI.maxText.text="(Initiales : "+ForestGameManager.gameInstance.carbonEmission.ToString()+"t/an)";
        carbonEmissionUI.slider.maxValue=ForestGameManager.gameInstance.carbonEmission;

        carbonBudgetUI.maxText.text="/ "+ForestGameManager.gameInstance.carbonBudget.ToString()+"t";
        carbonBudgetUI.slider.maxValue=ForestGameManager.gameInstance.carbonBudget;

        yearUI.maxText.text="2100";
        yearUI.slider.maxValue=100;
    }
    
    private void UpdateUI()
    {
        carbonEmissionUI.currentText.text=ForestGameManager.gameInstance.carbonEmission.ToString()+"t/an";
        carbonEmissionUI.slider.value=ForestGameManager.gameInstance.carbonEmission;

        carbonBudgetUI.currentText.text=ForestGameManager.gameInstance.carbonBudget.ToString()+"t";
        carbonBudgetUI.slider.value=ForestGameManager.gameInstance.carbonBudget;

        yearUI.currentText.text=ForestGameManager.gameInstance.year.ToString();
        yearUI.slider.value=ForestGameManager.gameInstance.year-2000;

        peopleUI.text=ForestGameManager.gameInstance.people.ToString()+" habitants";
        fedPeopleUI.text="Peut en nourrir "+ForestGameManager.gameInstance.fedPeople.ToString();
    }

}

[System.Serializable]
public class SliderUI
{
    public Slider slider;
    public Text maxText;
    public Text currentText;
}