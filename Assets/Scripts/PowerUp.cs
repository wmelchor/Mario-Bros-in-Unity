using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type {
        Coin,
        ExtraLife,
        SuperMushroom,
        FireFlower,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player) {
        switch (type) {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

            case Type.SuperMushroom:
                player.GetComponent<Player>().Grow();
                break;

            case Type.FireFlower:
                player.GetComponent<Player>().GainFirePower();
                break;

            case Type.Starpower:
                player.GetComponent<Player>().Starpower();
                break;
        }

        Destroy(gameObject);
    }
}
