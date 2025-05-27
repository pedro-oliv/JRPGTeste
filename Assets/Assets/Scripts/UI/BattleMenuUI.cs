using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public List<TextMeshProUGUI> options;
    private int currentIndex = 0;
    private Action<int> onOptionSelected;
    private bool isActive = true;

    private void Start()
    {
        options = new List<TextMeshProUGUI>(menuPanel.GetComponentsInChildren<TextMeshProUGUI>());
        currentIndex = 0;
        UpdateVisual();
    }

    public void Show(Action<int> callback)
    {
        onOptionSelected = callback;
        currentIndex = 0;
        isActive = true;
        UpdateVisual();
    }

    public void Deactivate()
    {
        isActive = false;
    }

    void Update()
    {
        if (!isActive || options == null || options.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % options.Count;
            UpdateVisual();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + options.Count) % options.Count;
            UpdateVisual();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            ConfirmOption();
        }
    }

    void UpdateVisual()
    {
        PointerController.Instance.PlaySelectSound();
        if (options == null || options.Count == 0) return;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].color = (i == currentIndex) ? Color.yellow : Color.white;
        }

        PointerController.Instance.ApontarPara(options[currentIndex].GetComponent<RectTransform>(), new Vector2(-62f, -5f));
    }

    void ConfirmOption()
    {
        PointerController.Instance.PlaySelectSound();
        isActive = false;
        onOptionSelected?.Invoke(currentIndex);
    }

    public void Activate()
    {
        isActive = true;
    }

}


