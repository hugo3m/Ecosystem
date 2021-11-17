using UnityEngine;

public class LivingEntity : MonoBehaviour {

    public int colourMaterialIndex;
    public Environment environment;
    public Species species;
    public Material material;

    public Coord coord;
    //
    [HideInInspector]
    public int mapIndex;
    [HideInInspector]
    public Coord mapCoord;

    protected bool dead;

    float amountRemaining = 1;
    const float consumeSpeed = 8;

    public virtual void Init (Coord coord, Environment env) {
        this.environment = env;
        this.coord = coord;
        transform.position = environment.tileCentres[coord.x, coord.y];

        // Set material to the instance material
        var meshRenderer = transform.GetComponentInChildren<MeshRenderer> ();
        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
        {
            if (meshRenderer.sharedMaterials[i] == material) {
                material = meshRenderer.materials[i];
                break;
            }
        }
    }

    protected virtual void Die (CauseOfDeath cause) {
        if (!dead) {
            dead = true;
            environment.RegisterDeath (this);
            Destroy (gameObject);
        }
    }

    public virtual float Consume(float amount)
    {
        Die(CauseOfDeath.Eaten);
        return 1;
    }
}