using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBagCollider : MonoBehaviour
{

    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private MeshRenderer meshToChange;

    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void Start()
    {
        GameManager.instance.scoreManager.scoreText = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        var loot = other.GetComponent<LootPickup>();

        if (loot != null)
        {
            GameManager.instance.scoreManager.score += loot.lootResource.value;
            GameManager.instance.scoreManager.UpdateUI();

            meshToChange.material = highlightMaterial;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Treasure"))
        {
            meshToChange.material = defaultMaterial;
        }
    }
}
