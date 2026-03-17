using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MapRenderer : MonoBehaviour
{
    public MapView mapView;
    public GameObject tilePrefab;

    private Dictionary<Vector2Int, GameObject> activeTiles = new();
    private Stack<GameObject> tilesPool = new();

    private Dictionary<(int zoom, int x, int y), Texture2D> textureCache = new();

    private string mapTilesPath;
    private void Awake()
    {
        mapView = GetComponent<MapView>();
        mapView.onTileCreate += CreateTile;
        mapView.onTileHide += HideTile;
        mapView.onTileRemove += RemoveTile;

        mapTilesPath = Path.Combine(Application.persistentDataPath, "MapTiles");
        if(!Directory.Exists(mapTilesPath))
            Directory.CreateDirectory(mapTilesPath);
    }
    private void CreateTile(Vector2Int pos)
    {
        StartCoroutine(LoadTile(pos));
    }
    private IEnumerator LoadTile(Vector2Int tilePos)
    {
        var key = (mapView.zoomLevel, tilePos.x, tilePos.y);

        if (textureCache.TryGetValue(key, out var tex))
        {
            ApplyTile(tilePos, tex);
            yield break;
        }

        string fileName = $"{key.zoomLevel}_{key.x}_{key.y}.png";
        string filePath = Path.Combine(mapTilesPath, fileName);
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            textureCache[key] = tex;
            ApplyTile(tilePos, tex);
            yield break;
        }

        string url = $"https://tile.openstreetmap.org/{mapView.zoomLevel}/{tilePos.x}/{tilePos.y}.png";

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Не удалось загрузить тайл: " + url);
            }
            else
            {
                tex = DownloadHandlerTexture.GetContent(uwr);
                textureCache[key] = tex;

                File.WriteAllBytes(filePath, tex.EncodeToPNG());

                ApplyTile(tilePos, tex);
            }
        }
    }
    private void ApplyTile(Vector2Int tilePos, Texture2D tex)
    {
        GameObject tileObj = GetPooledObject();
        tileObj.transform.SetParent(transform);
        tileObj.SetActive(true);

        Vector2 pos = mapView.center0;
        tileObj.transform.position = new Vector2(2.56f * (tilePos.x - (int)pos.x), -2.56f * (tilePos.y - (int)pos.y));
        tileObj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
            tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        activeTiles[tilePos] = tileObj;
    }
    private GameObject GetPooledObject()
    {
        if(tilesPool.Count > 0)
            return tilesPool.Pop();

        return Instantiate(tilePrefab);
    }
    private void RemoveTile(Vector2Int tilePos)
    {
        if(activeTiles.TryGetValue(tilePos, out GameObject tile))
            Destroy(tile);

        activeTiles.Remove(tilePos);
    }
    private void HideTile(Vector2Int tilePos)
    {
        if (!activeTiles.TryGetValue(tilePos, out GameObject tile))
            return;

        tile.SetActive(false);
        tilesPool.Push(tile);
        activeTiles.Remove(tilePos);
    }
}
