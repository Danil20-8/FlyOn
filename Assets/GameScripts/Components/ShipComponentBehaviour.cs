using UnityEngine;
using System.Collections;
using Assets.GameModels;
using Assets.Global;
using Assets.GameModels.Phisical;
using MyLib;
using MyLib.Modern;
using System.Linq;

public class ShipComponentBehaviour : MyMonoBehaviour
{

    SystemLink link;
    public ShipDriver driver { get; private set; }
    protected TacklePlaceBehaviour tacklePlace;
    Renderer[] self;
    CombatBehaviour combat;
    public TempTransform _tempTransform;
    public TempTransform tempTransform { get { return _tempTransform; } }
    protected bool updated = false;

    public Vector3 localPosition;
    public Quaternion localRotation;

    protected void Awake()
    {
        foreach (var t in GetComponentsInChildren<Transform>())
            t.gameObject.layer = Constants.shipItemLayer;
        _tempTransform = AddMyComponent<TempTransform>();
    }
    protected void Start()
    {
        tempTransform.enabled = false;

        combat = gameObject.GetComponent<CombatBehaviour>();
        self = GetComponentsInChildren<Renderer>();

        OnStart();
    }
    protected virtual void OnStart()
    {

    }

    Material material;
    public void TakeDamage(float damage)
    {
        link.SetDamage(damage);
        var t_material = HealthMaterials.GetMaterial(link.health);
        if(material != t_material)
        {
            material = t_material;
            for (int i = 0; i < 0; i++)
                self[i].material = material;
        }
    }
    void SetLink(SystemLink link)
    {
        this.link = link;
        link.SetMaster(this);
        OnSetLink(link.item);
    }
    protected virtual void OnSetLink(SystemComponent component)
    {

    }

    public void SetDriver(ShipDriver driver)
    {
        this.driver = driver;
    }

    public static ShipComponentBehaviour InstantiateBehaviour(SystemLink link, TacklePlaceBehaviour tackle)
    {
        var c = (ShipComponentBehaviour)Instantiate(GameResources.GetShipComponent(link.item.name), tackle.transform.position, tackle.transform.rotation);

        c.tacklePlace = c.gameObject.AddComponent<TacklePlaceBehaviour>();
        c.tacklePlace.Init(tackle);

        c.SetLink(link);
        return c;
    }

}
