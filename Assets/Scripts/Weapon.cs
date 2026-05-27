using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;

    public void Use()
    {
        Debug.Log("Dùng " + weaponName);
    }
}