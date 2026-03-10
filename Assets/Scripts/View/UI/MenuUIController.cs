using System.Linq;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    private Animator animator;
    public GameObject filtersPanel;
    public GameObject panelLayout;
    public GameObject notFoundPanel;
    public GameObject findingPanel;
    public PlaceFilter placeFilter;
    public MenuAgregator menuAgregator;
    public MarkerRenderer markerRenderer;
    public MapView mapView;

    public bool isMenuClosed;
    public bool isNotFoundErrorClosed;
    private void Start()
    {
        animator = GetComponent<Animator>();
        isMenuClosed = true;
        isNotFoundErrorClosed = true;
        placeFilter.onFindedPlace += ShowPlaces;
    }
    public void ToggleOpening()
    {
        animator.Play(isMenuClosed ? "Opening" : "Closing");
        isMenuClosed = !isMenuClosed;
    }
    public void OpenFilters()
    {
        filtersPanel.SetActive(true);
    }
    public void CloseFilters()
    {
        filtersPanel.SetActive(false);
    }
    public void CheckFilters()
    {
        if (placeFilter.ApplyFilters())
        {
            Debug.Log("Найдено " + placeFilter.filteredPlaces.Count + " заведений");
            menuAgregator.FillMainMenu(placeFilter.filteredPlaces);
            markerRenderer.CreateMarkers(placeFilter.filteredPlaces
                .Select(p => LocationTransform.LatLonToUnityPos(p.location, mapView.zoomLevel, mapView.center0))
                .ToList());
            CloseFilters();
        }
        else
        {
            Debug.Log("Не найдены");
            ToogleNotFoundPanel();
        }
    }
    public void ToogleNotFoundPanel()
    {
        notFoundPanel.SetActive(isNotFoundErrorClosed);
        panelLayout.SetActive(isNotFoundErrorClosed);
        isNotFoundErrorClosed = !isNotFoundErrorClosed;
    }
    public void ShowPlaces()
    {
        menuAgregator.FillMainMenu(placeFilter.filteredPlaces);
        markerRenderer.CreateMarkers(placeFilter.filteredPlaces
            .Select(p => LocationTransform.LatLonToUnityPos(p.location, mapView.zoomLevel, mapView.center0))
            .ToList());
        findingPanel.SetActive(false);
        panelLayout.SetActive(false);
        CloseFilters();
    }
    public void ExpandSearchArea()
    {
        StartCoroutine(placeFilter.placeFinder.FindPlacesInRadius(mapView.center, 2.5f));
        StartCoroutine(placeFilter.CheckPlacesInArea(2.5f));
        notFoundPanel.SetActive(false);
        isNotFoundErrorClosed = !isNotFoundErrorClosed;
        findingPanel.SetActive(true);
    }
}
