using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterAgregator : MonoBehaviour
{
    public GameObject filterContainer;
    public GameObject filterPrefab;

    public PlaceFilter placeFilter;
    private void Start()
    {
        List<FilterContainer> contList = new();
        foreach(FilterType value in Enum.GetValues(typeof(FilterType)))
        {
            if (value == FilterType.other)
                continue; // Пока что пропускаем

            GameObject cont = Instantiate(filterContainer, transform);

            cont.transform.Find("Name").GetComponent<TMP_Text>().text = TranslatorEnum.filtercontainer[value];
            Transform filtersList = cont.transform.Find("FiltersList");

            FilterContainer filtCont = filtersList.GetComponent<FilterContainer>();
            filtCont.type = value;
            List<Filter> filterList = filtCont.filters;
            contList.Add(filtCont);

            foreach (FilterOption option in FilterTypeToOption.t2o[value])
            {
                GameObject opt = Instantiate(filterPrefab, filtersList);
                opt.GetComponent<TMP_Text>().text = TranslatorEnum.filter[option];
                opt.GetComponent<Filter>().option = option;
                filterList.Add(opt.GetComponent<Filter>());
            }
        }
        placeFilter.filterContainers = contList;
        StartCoroutine(RebuildLayout((RectTransform)transform));
    }
    private IEnumerator RebuildLayout(RectTransform transform)
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    }
}
