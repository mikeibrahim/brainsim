using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleList : MonoBehaviour
{
    [SerializeField] private Button moduleButton;
    private void Start()
    {
        Manager.Instance.GetModules.ForEach(module =>
        {
            Button button = Instantiate(moduleButton, transform);
            button.GetComponentInChildren<TMP_Text>().text = module.name;
            button.onClick.AddListener(() => { module.gameObject.SetActive(!module.gameObject.activeSelf); });
        });
    }
}
