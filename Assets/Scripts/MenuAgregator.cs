using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuAgregator : MonoBehaviour
{
    public GameObject placePrefab;
    public void FillMainMenu(List<Place> places)
    {
        foreach(Transform child in transform)
            Destroy(child.gameObject);

        foreach(Place place in places)
        {
            GameObject placeCard = Instantiate(placePrefab, transform);
            
            placeCard.transform.Find("SubCategory").Find("Text").
                gameObject.GetComponent<TMP_Text>().text = TranslatorEnum.subcategory[place.subcategory];
            placeCard.transform.Find("PlaceName").
                gameObject.GetComponent<TMP_Text>().text = place.name;
        }
    }
}
