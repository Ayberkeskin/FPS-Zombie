using UnityEngine;

[CreateAssetMenu(fileName ="Weapon",menuName ="Weapons/New Weapon",order =0)]
public class WeaponData : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimatorOverrideController animOverride;
    [SerializeField] int damage;
    [SerializeField] float attackRate;
    [SerializeField] Vector3 positionOffset=Vector3.zero;
    [SerializeField] Vector3 scaleOffset=Vector3.zero;

    private GameObject weaponClone;

    public GameObject WeaponPrefab { get => weaponPrefab; }
    public int Damage { get => damage;  }
    public float AttackRate { get => attackRate;  }


    public void SpawnNewWeapon(Transform parent,Animator anim)
    {
        if (weaponPrefab!=null)
        {
            weaponClone = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity, parent);
            weaponClone.transform.position = parent.position;
            weaponClone.transform.rotation = parent.rotation;
            weaponClone.transform.localScale = weaponClone.transform.localScale + scaleOffset;
            weaponClone.transform.localPosition = Vector3.zero + positionOffset;

        }
        if (animOverride!=null)
        {
            anim.runtimeAnimatorController = animOverride;  
        }

    }
    public void Drop()
    {
        Destroy(weaponClone);
    }
}
