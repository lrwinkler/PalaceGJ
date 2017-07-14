using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    // pacz w ustawienia tekstury!
    public Texture2D mapBlueprint;
    public bool spawnInLocalCoordinates;
    public MapEntry[] mapInput;

    private Dictionary<string, GameObject> mapDict;
    private Rect mapBounds;
    private StatueManager manager;
    Color aSpecialKindOfYellow;

    void Start()
    {
        GenerateDict();
        GenerateMap();
        mapBounds = new Rect(0, 0, mapBlueprint.width, mapBlueprint.height);
        manager = FindObjectOfType<StatueManager>();
    }

    private void GenerateDict()
    {
        mapDict = new Dictionary<string, GameObject>();
        foreach (MapEntry entry in mapInput)
            mapDict.Add(toColorString(entry.color), entry.prefab);
        aSpecialKindOfYellow = mapInput[0].color;
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapBlueprint.width; x++)
            for (int y = 0; y < mapBlueprint.height; y++)
                spawnCube(x, y);
    }

    private void spawnCube(int x, int y)
    {
        Color cubeCode = mapBlueprint.GetPixel(x, y);
        if (cubeCode.Equals(Color.green)) {
            mapBlueprint.SetPixel(x, y, aSpecialKindOfYellow);
            cubeCode = aSpecialKindOfYellow;
            Debug.Log("ale chuj");
        }
        
        if (mapDict.ContainsKey(toColorString(cubeCode)))
        {
            Vector3 position = new Vector3(x, y, 0);
            GameObject tile = Instantiate(mapDict[toColorString(cubeCode)], this.transform.localPosition + position, Quaternion.identity, this.transform);
            tile.name = string.Format("[{0}:{1}] - {2}", x, y, tile.name);
        }
    }

    private string toColorString(Color color)
    {
        return color.r.ToString("F") + color.g.ToString("F") + color.b.ToString("F");
    }

    public bool canPass(Vector3 point)
    {
        if (mapBounds.Contains(point))
            return mapBlueprint.GetPixel((int)point.x, (int)point.y).r >= 0.5;
        Debug.Log(string.Format("there is nowhere to go anymore [{0}:{1}]", (int)point.x, (int)point.y));
        return false;
    }


	public bool canSee(Vector3 point)
    {
        if (mapBounds.Contains(point))
            return mapBlueprint.GetPixel((int)point.x, (int)point.y).g.Equals(1f);
        Debug.Log(string.Format("there is nothing to see anymore [{0}:{1}]", (int)point.x, (int)point.y));
        return false;
    }

    public void spawnStatue(int quality, Vector3 point, float animationFrame, string animationState)
    {
        GameObject statue = Instantiate(Resources.Load<GameObject>("Statue"), point, Quaternion.identity, this.transform);
		mapBlueprint.SetPixel((int) point.x,(int) point.y, Color.green);
        manager.add(statue);

        Animator statueAnimator = statue.GetComponent<Animator>();

        statueAnimator.CrossFade(animationState, 0f, 0, animationFrame);
        statueAnimator.speed = 0;

        statueAnimator.SetBool("facingUp", false);
        statueAnimator.SetBool("facingDown", false);
        statueAnimator.SetBool("facingLeft", false);
        statueAnimator.SetBool("facingRight", false);

        Transform horizontalSprite = statue.transform.Find("HorizontalAnimation");
        Transform upSprite = statue.transform.Find("UpAnimation");
        Transform downSprite = statue.transform.Find("DownAnimation");



        foreach (Transform child in horizontalSprite)
        {
            child.GetComponent<SpriteRenderer>().color = Color.grey;
        }

        foreach (Transform child in upSprite)
        {
            child.GetComponent<SpriteRenderer>().color = Color.grey;
        }

        foreach (Transform child in downSprite)
        {
            child.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    public void removeStatue(Vector3 point)
    {
		mapBlueprint.SetPixel((int)point.x, (int)point.y, Color.yellow);
		manager.Remove(point);
    }
}
