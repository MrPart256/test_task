using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlanetGeneration : MonoBehaviour
{
    public static PlanetGeneration instance;
    //Если верить солнечной системе, то сосед определеяется порядковым числом, а не расстоянием
    [SerializeField] private GameObject planet;
    [SerializeField] [Range(0.5f,2.5f)] private float safeZoneBoundary;
    [SerializeField] [Range(0.1f,0.2f)] private float planetMinSize;
    [SerializeField] [Range(0.3f, 0.5f)] private float planetMaxSize;
    [SerializeField] private int planetsAmount;

    public NavMeshSurface2d surface;

    public List<GameObject> planets;
    private Vector2 worldSize;
    private bool yes = true;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        worldSize = GetWorldSize();

        GeneratePlanets();
        PlanetPlacement();

        SetPlayerPlanet();
        
        SetNeutralPlanets();
        
        
        surface = GetComponent<NavMeshSurface2d>();
        surface.size = new Vector3(worldSize.x,1,worldSize.y);
        surface.BuildNavMeshAsync();

    }
   
    private void SetPlayerPlanet()
    {
        planets[Random.Range(0, planets.Count)].GetComponent<Planet>().SetPlayerPlanet(50);
    }
    
    private void SetNeutralPlanets()
    {
        foreach(GameObject planet in planets)
        {
            if (!planet.CompareTag("PlayerPlanet"))
            {
                planet.GetComponent<Planet>().SetNeutralPlanet(Random.Range(4, 56));
            }
        }
    }
    private void Update()
    {
       
    }
    private void GeneratePlanets()
    {
        for (int i = 0; i < planetsAmount; i++)
        {
            planets.Add(GeneratePlanet());     
        }
    }

    private GameObject GeneratePlanet()
    {
        
       GameObject _planet =  Instantiate(planet,RandomPointInsideBox(worldSize),Quaternion.identity);
       _planet.GetComponent<Planet>().setPlanetSize(Random.Range(planetMinSize, planetMaxSize));
       
        return _planet;
    }

    private void PlanetPlacement() //Она работает, но всегда есть одна планета, которой что-то не нравится
    {
            foreach (GameObject planet in planets)
            {
                GameObject closestPlanet = FindClosestObject(planet, planets);
                GameObject neighbourPlanet = planets[Mathf.Clamp(planets.IndexOf(closestPlanet) - 1, 0, planets.Count - 1)];
                
                float distance = Vector2.Distance(planet.transform.position, closestPlanet.transform.position);
                float radiusSum = (closestPlanet.GetComponent<Planet>().radius + neighbourPlanet.GetComponent<Planet>().radius); 
                if (distance < radiusSum)
                {
                    planet.transform.position = (planet.transform.position - closestPlanet.transform.position).normalized
                        * (radiusSum + (planet.GetComponent<Planet>().radius * 2)) + closestPlanet.transform.position;
                }
            
        }
    }
    
    private GameObject FindClosestObject(GameObject currentObject,List<GameObject> objects)
    {

        GameObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 position = currentObject.transform.position;
        foreach (GameObject obj in objects)
        {
            Vector3 diff = obj.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && obj!=currentObject)
            {
                closestObject = obj;
                distance = curDistance;
            }
        }
        return closestObject;
    }

    private Vector2 RandomPointInsideBox(Vector2 boxSize)
    {
        float x = Random.Range(-(boxSize.x / 2) + safeZoneBoundary,(boxSize.x/2)-safeZoneBoundary);
        float y = Random.Range(-(boxSize.y / 2) + safeZoneBoundary, (boxSize.y / 2) - safeZoneBoundary);
        Vector2 randomPoint = new Vector2(x,y);
        return randomPoint;
    }

    private Vector2 GetWorldSize()
    {
        float aspect = (float)Screen.width / Screen.height;

        float worldHeight = Camera.main.orthographicSize * 2;

        float worldWidth = worldHeight * aspect;

        return new Vector2(worldWidth,worldHeight);
    }

}
