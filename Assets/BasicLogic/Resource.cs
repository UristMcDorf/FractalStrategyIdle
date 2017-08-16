using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : FSIEntity
{
    [SerializeField]
    string displayName = null;
    [SerializeField]
    Sprite icon = null;

    [SerializeField]
    double _amount = 0;
    public double Amount
    {
        get
        {
            return System.Math.Floor(_amount);
        }
    }

    double _income = 0;
    public double Income
    {
        get
        {
            return _income;
        }
    }

    float secondsSinceLast = 0;
    double incomeSinceLast = 0;

    GameObject display;
    Image image;
    Text text;

    static protected string resourceDisplayPanelPrefabPath = "CommonPrefabs/ResourcePanel";
    static protected GameObject resourceDisplayPanelPrefab = null;

	// Use this for initialization
	protected override void Start()
    {
        base.Start();

        if (resourceDisplayPanelPrefab == null)
            resourceDisplayPanelPrefab = Resources.Load<GameObject>(resourceDisplayPanelPrefabPath);
        SetupDisplay();

        UpdateText();
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<ProduceIncomeEvent>(OnProduceIncome);
        EventManager.Instance.AddListener<ProduceImmediateIncomeEvent>(OnProduceImmediateIncome);
	}
	
    void OnDisable()
    {
        if(EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener<ProduceIncomeEvent>(OnProduceIncome);
            EventManager.Instance.RemoveListener<ProduceImmediateIncomeEvent>(OnProduceImmediateIncome);
        }
    }

    void OnProduceIncome(ProduceIncomeEvent eventInst)
    {
        if (eventInst.resourceSysname == this.SysName)
        {
            incomeSinceLast += eventInst.amount;

            if (!display.activeSelf)
                display.SetActive(true);
        }
    }

    void OnProduceImmediateIncome(ProduceImmediateIncomeEvent eventInst)
    {
        if (eventInst.resourceSysname == this.SysName)
        {
            _amount += eventInst.amount;

            if (!display.activeSelf)
                display.SetActive(true);
        }
    }

	// Update is called once per frame
    protected override void Update()
    {
        secondsSinceLast += Time.deltaTime;

        if (secondsSinceLast >= 1)
        {
            _amount += incomeSinceLast;
            _income = incomeSinceLast / secondsSinceLast;

            secondsSinceLast = 0;
            incomeSinceLast = 0;
        }

        UpdateText();
	}

    void UpdateAmountPerSecond(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            _amount += Income;
        }

        UpdateText();
    }

    void UpdateText()
    {
        text.text = displayName + ": " + FSIUtility.NumberToString(Amount) + "\n+" + FSIUtility.NumberToString(Income, 1) + "/second";
    }

    void SetupDisplay()
    {
        GameObject display = Instantiate(resourceDisplayPanelPrefab);

        text = display.GetComponent<ResourcePanel>().text;
        image = display.GetComponent<ResourcePanel>().image;

        image.sprite = icon;

        display.transform.SetParent(UIManager.Instance.ResourceBar.transform, false);

        if (Amount <= 0)
            display.SetActive(false);

        this.display = display;
    }

    public bool TrySpend(double amount)
    {
        if (_amount >= amount)
        {
            _amount -= amount;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return InGameName;
    }
}
