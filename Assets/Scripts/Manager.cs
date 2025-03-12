using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    [SerializeField] private List<Module> modulePrefabs;
    private List<Module> modules;

    private void Awake()
    {
        Instance = this;
        modules = new List<Module>();
        modulePrefabs.ForEach(modulePrefab =>
        {
            Module module = Instantiate(modulePrefab);
            modules.Add(module);
            module.gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        foreach (Module module in modules)
        {
            module.transform.SetParent(UI.Instance.transform);
            module.transform.localPosition = Vector3.zero;
            module.transform.localScale = Vector3.one;
        }
    }

    public List<Module> GetModules => modules;
}
