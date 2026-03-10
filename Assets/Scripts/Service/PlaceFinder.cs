using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class PlaceFinder : MonoBehaviour
{
    public List<Place> places;
    public MenuAgregator agregator;

    public bool isFinding;
    private void Start()
    {
        places = new();
    }
    public IEnumerator FindPlacesInRadius(GeoLocation loc, float radius)
    {
        // Для замены запятых в float на точки
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        isFinding = true;

        string query = $@"[out:json][timeout:60];
(
  node[""amenity""~""restaurant|cafe|bar|fast_food|food_court|cinema|night_club|internet_cafe""]
(around:{radius * 1000},{loc.latitude},{loc.longitude});
);
out body;
>;
out skel qt;";

        places.Clear();

        string url = "https://overpass-api.de/api/interpreter?data=" + query;

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    string jsonResult = uwr.downloadHandler.text;
                    ParseResponse(jsonResult);
                    isFinding = false;
                    yield break;
                }

                if (uwr.responseCode == 504)
                {
                    Debug.LogWarning("Превышено время ожидания ответа от сервера. " +
                        "Повторная попытка обратиться: " + attempt + "/" + 3);
                    yield return new WaitForSeconds(2f);
                    continue;
                }
                if (uwr.responseCode == 429)
                {
                    Debug.LogWarning("Слишком много запросов к API. Ждём и пробуем снова. " +
                        "Попытка: " + attempt + "/" + 3);
                    yield return new WaitForSeconds(10f);
                    continue;
                }

                Debug.LogWarning("Ошибка запроса к Overpass: " + uwr.error);
                Debug.LogWarning("Код ответа: " + uwr.responseCode);
                Debug.LogWarning("Тело ответа: " + uwr.downloadHandler.text);
                isFinding = false;
                yield break;
            }
        }
        if (places.Count == 0)
        {
            Debug.LogWarning("Не удалось ничего найти за отведённое количество попыток");
            isFinding = false;
        }
    }
    private void ParseResponse(string json)
    {
        places.Clear();
        json = json.Replace("addr:city", "addr_city").
            Replace("addr:postcode", "addr_post_code").
            Replace("addr:housenumber", "addr_housenumber").
            Replace("addr:street", "addr_street").
            Replace("payment:cash", "payment_cash").
            Replace("payment:credit_cards", "payment_credit_cards").
            Replace("contact:phone", "phone");

        try
        {
            OverpassResponse response = JsonUtility.FromJson<OverpassResponse>(json);

            if (response?.elements == null || response.elements.Length == 0)
            {
                return;
            }

            foreach (var element in response.elements)
            {

                if (element?.type != "node")
                    continue;

                Place place = new Place(element);
                places.Add(place);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка парсинга JSON: {e.Message}");
        }

        agregator.FillMainMenu(places);
    }
}