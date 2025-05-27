using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionUI : MonoBehaviour
{
    public static TargetSelectionUI Instance;

    public GameObject menuPanel;
    private List<TextMeshProUGUI> options;
    private int currentIndex = 0;
    private Action<int> onOptionSelected;
    public List<TextMeshProUGUI> inimigoTexts;
    private Action<int> onInimigoSelecionado;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        options = new List<TextMeshProUGUI>(menuPanel.GetComponentsInChildren<TextMeshProUGUI>());
        Disable();
    }

    public void Show(List<string> optionLabels, Action<int> onSelected)
    {
        onOptionSelected = onSelected;

        for (int i = 0; i < options.Count; i++)
        {
            options[i].gameObject.SetActive(i < optionLabels.Count);
            if (i < optionLabels.Count)
                options[i].text = optionLabels[i];
        }

        currentIndex = 0;
        UpdateVisual();
        Enable();
    }

    void Update()
    {
        if (!enabled || options == null || options.Count == 0) return;

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
            onOptionSelected?.Invoke(currentIndex);
            Disable();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            BattleUIManager.Instance.CancelarSelecao();
            Disable();
        }
    }

    void UpdateVisual()
    {
        PointerController.Instance.PlaySelectSound();
        for (int i = 0; i < options.Count; i++)
        {
            options[i].color = (i == currentIndex) ? Color.yellow : Color.white;
        }

        PointerController.Instance.ApontarPara(options[currentIndex].GetComponent<RectTransform>(), new Vector2(-68f, -5f));
    }

    public void Enable()
    {
        this.enabled = true;
    }

    public void Disable()
    {
        PointerController.Instance.PlaySelectSound();
        this.enabled = false;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].color = Color.white;
        }
    }

    public void InicializarInimigos(List<string> nomesInimigos)
    {
        for (int i = 0; i < inimigoTexts.Count; i++)
        {
            if (i < nomesInimigos.Count)
            {
                inimigoTexts[i].text = nomesInimigos[i];
            }
            else
            {
                inimigoTexts[i].text = "";
            }
        }
    }
}

