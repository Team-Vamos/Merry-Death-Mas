using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField]
    private Text txt;

    void Start()
    {
        var dropdown = transform.GetComponent<TMP_Dropdown>();

        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("Santa Tell Me");
        items.Add("Jingle Bell Rock");
        items.Add("All I Want For Christmas Is You");
        items.Add("Snowman");
        items.Add("Last Chirstmas");

        foreach (var item in items)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        txt.text = dropdown.options[index].text;

        BGM_Manager.Instance.TurnMusic(index);
    }
}
