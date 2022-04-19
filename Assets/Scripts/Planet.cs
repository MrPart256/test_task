using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private int maxBattleshipAmount = 99;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject battleshipPrefab;
    [SerializeField]private TextMeshPro text;
    private int battleshipCount;
    
    public float radius;
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void setPlanetSize(float planetRadius)
    {
        radius = planetRadius;
        Vector2 scale = new Vector3(planetRadius * 2, planetRadius * 2);
        transform.localScale = scale;
    }

    #region PlayerPlanet
    public void SetPlayerPlanet(int battleShipAmount)
    {

        gameObject.tag = "PlayerPlanet";
        battleshipCount = battleShipAmount;
        StartCoroutine(CreateBattleship(1));
       
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.blue;
    }

   


    
    #endregion

    #region NeutralPlanet
    public void SetNeutralPlanet(int battleShipAmount)
    {
        gameObject.tag = "NeutralPlanet";

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.gray;

        battleshipCount = -battleShipAmount;
        text.text = Mathf.Abs(battleshipCount).ToString();
    }



    #endregion

    private IEnumerator CreateBattleship(int i)
    {
        while (true)
        {
            if (Mathf.Abs(battleshipCount) < Mathf.Abs(maxBattleshipAmount))
            {
                battleshipCount += i;
            }
            text.text = Mathf.Abs(battleshipCount).ToString();
            yield return new WaitForSeconds(0.2f);
        }

    }

    public void SendBattleships(Transform target)
    {
        Debug.Log("dddd");
        battleshipCount /= 2;
        for (int i = 0; i < battleshipCount; i++)
        {
            Debug.Log("asdd");
            float angle = i * Mathf.PI * 15f / battleshipCount / 2;
            Vector3 newPos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Instantiate(battleshipPrefab, transform.position + newPos, Quaternion.identity).GetComponent<Battleship>().MoveToTarget(target);

        }
    }
    public void RecieveBattleShip(int i)
    {

        battleshipCount += i;
        text.text = Mathf.Abs(battleshipCount).ToString();
        if (battleshipCount >= 0 && gameObject.tag != "PlayerPlanet")
        {
            SetPlayerPlanet(0);
        }
    }
    public void SelectPlanet()
    {
        outline.SetActive(true);
    }
    public void DeselectPlanet()
    {
        outline.SetActive(false);
    }
}
