using System.Collections.Generic;
using UnityEngine;

public class NoiseField : MonoBehaviour
{
    public int height = 50;
    public int width = 50;
    public float scale = 2;
    //Schmovin
    //each increment of T
    public float movementSmoothment = 0.01f;
    public GameObject prefab;
    public bool ZoomCamera;
    
    private Dictionary<Vector2, GameObject> Vectors = new();
    private FastNoise fastNoise;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        //make sure the noise is always in view
        if (ZoomCamera)
        {
            Camera.main.orthographicSize = height / 2;
        }

        //i stole this from a script i made like 2 years ago, your guess as good as mine for what this line does \_(+_+)_/`
        fastNoise = new FastNoise();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //(x - width / 2 + 0.5f, y - height / 2 + 0.5f)
                //moves the noise to the center, we add the 0.5f due to uh, reasons. idk ask the idiot who wrote this, wait a sec-
                GameObject go = Instantiate(prefab, new Vector2(x - width / 2 + 0.5f, y - height / 2 + 0.5f), new Quaternion(0, 0, 0, 0));
                Vectors.Add(new Vector2(x, y), go);
            }
        }
        //feel free to change            V this value
        InvokeRepeating("update", 0.0f, .1f);
    }

    //this variable does absolutely nothing
    //i know it does absolutely nothing
    //absolutely nothing bad could possibly happen if i remove it
    //but everything works just fine so uh, pretend this isnt here
    float _t;
    void update()
    {
        t = _t;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //get the gameobject at the positon x, y
                Vectors.TryGetValue(new Vector2(x, y), out GameObject go);
                //get the noise value at T
                //we are essentially getting a slice of a bigger 3 dimensional noise each time
                float noise = fastNoise.GetSimplex(x * scale, y * scale, t);
                //the noise we got earlier is a decimal number between 0 and 1, we times it by 360 to convert it to an angle then rotate the object by that value
                go.transform.localEulerAngles = new Vector3(0, 0, noise * 360);
            }
        }
        //pleasehelpthislineofcodeisholdingmywifeandchildrenhostageuntilideleteit
        _t += movementSmoothment;
    }
}