using TMPro;
using UnityEngine;
public class Filter : MonoBehaviour
{
    public GameObject selectedMark;
    public FilterOption option;
    public bool isSelected;
    private void Start()
    {
        isSelected = false;
    }
    public void OnClick()
    {
        isSelected = !isSelected;
        selectedMark.SetActive(isSelected);
        GetComponent<TMP_Text>().color = isSelected ? new Color(243f/255, 1, 132f/255) : Color.white;
    }
}