using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool has_projectile_item_grappled = false;
    private SpriteRenderer pistol_sprender;
    public Sprite pistol_sprite;

    // Equipped Projectile
    private GameObject equipped_projectile;
    private SpriteRenderer sprender;
    private Sprite proj_sprite;
    private ProjectileItem proj_script;

    // Start is called before the first frame update
    void Start()
    {
        pistol_sprender = GameObject.Find("Pistol Sprite").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void set_collision_as_equipped_projectile(GameObject new_projectile)
    {
        Debug.Log("setting collision as equipped projectile");
        has_projectile_item_grappled = true;
        sprender = new_projectile.GetComponent<SpriteRenderer>();
        proj_sprite = sprender.sprite;
        proj_script = new_projectile.GetComponent<ProjectileItem>();
        sprender.enabled = false;
        new_projectile.GetComponent<Collider2D>().enabled = false;
        pistol_sprender.sprite = proj_sprite;
        equipped_projectile = new_projectile;
    }

    public void shootItemFromHere(Transform aimTransform)
    {
        Debug.Log("shooting item");
        has_projectile_item_grappled = false;
        pistol_sprender.sprite = pistol_sprite;
        Vector3 newPos = new Vector3(aimTransform.position.x, aimTransform.position.y, 0);
        equipped_projectile.GetComponent<Transform>().position = newPos;
        sprender.enabled = true;
        equipped_projectile.GetComponent<Collider2D>().enabled = true;
        proj_script.MovementSetup();
        proj_script.has_been_shot = true;
    }
}
