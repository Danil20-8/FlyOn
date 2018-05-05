using UnityEngine;
using System.Collections;
using Assets.Other;
using System.Linq;
using MyLib;
using MyLib.Algoriphms;
using Assets.Global;
public class PlayerDriver : ShipDriver {

    ShipKeyInput ski;
    WatcherBehaviour watcher;
    Camera camera;
    bool everyTimeFire = false;
    bool everyTimeRotate = true;

    Vector2 aimPoint = new Vector2(.5f, .5f);
    PlayerUIManager ui;
    PlayerUIBehaviour uiBehaviour;

    ShipController lockedEnemy = null;

    bool freeFly = false;

    bool lockCamera = false;

    public PlayerDriver(ShipTeam team)
        :base(team)
    { }

    public override void OnKill(ShipController ship)
    {
        uiBehaviour.AddFrag();
    }

    protected override void OnSetShip()
    {
        BattleBehaviour.AddEvent(() => { ship.team.myTeam = true;

            if (ui == null)
            {
                ui = GameObject.FindObjectOfType<PlayerUIManager>();
                ui.GetRadar().SetDriver(this);

                uiBehaviour = ScreenManager.GetScreen<PlayerUIBehaviour>();
            }
            var shipSystem = ship.shipSystem;

            ski = new ShipKeyInput(shipSystem);

            //Get camera from watcher
            watcher = GameObject.FindObjectOfType<WatcherBehaviour>();
            watcher.target = ship.transform;
            camera = watcher.GetCamera();


            //reset
            ui.SetAimPosition(aimPoint);
            everyTimeFire = false;
            everyTimeRotate = true;
            lockedEnemy = null;

            //SetAim(new Vector2(.5f, .7f));

            BattleBehaviour.current.audioPlayer.SetListener(ship.transform); // audioPlayer is not initialized on start
        });
    }

    public override void SlowUpdate()
    {
        base.SlowUpdate();

        ski.Update();
        if (Input.GetKeyDown(KeyCode.F12))
            ui.ShowHide();
        if (Input.GetMouseButtonDown(3))
        {
            if (lockCamera)
                watcher.sensetive = Vector2.one;
            lockCamera = !lockCamera;
        }
        if (lockCamera)
        {
            SetAim(new Vector2(Mathf.Clamp(aimPoint.x + UInput.RotateX() * .02f / Screen.width * Screen.height, .1f, .9f), Mathf.Clamp(aimPoint.y + UInput.RotateY() * -.02f / Screen.height * Screen.width, .1f, .9f)));
            watcher.sensetive = new Vector2(Mathf.Abs(aimPoint.x - .5f) / .4f, Mathf.Abs(aimPoint.y - .5f) / .4f);
            watcher.sensetive = Vector2.Scale(watcher.sensetive, watcher.sensetive);
        }

        bFire = UInput.Fire() != 0 || everyTimeFire;
        bRotate = UInput.Fire2() != 0 || everyTimeRotate;

        watcher.Update(); // rotate camera manually because else it does bad
        ui.Update(); // Same reason as ^
        moveDirection = camera.transform.rotation;

        firePoints.Clear();

        var point = new Vector3(Screen.width * aimPoint.x, Screen.height * aimPoint.y);
        
        var ray = camera.ScreenPointToRay(point);
        Debug.DrawRay(ship.tempTransform.position, BattleBehaviour.current.navigation.GetMovePoint(ship.tempTransform.position, ship.tempTransform.position + ray.direction * 10000) - ship.tempTransform.position, Color.blue);
        RaycastHit hit;
        if (Physics.SphereCastAll(ray, 10f, 100000).WithMin(h => (h.point - camera.transform.position).sqrMagnitude, h => !ShipIdentification.IsThisShip(h.transform, ship), out hit))
        {
            float distance = Vector3.Distance(hit.point, ship.transform.position);
            if(UInput.Fire() != 0)
                firePoints.AddPoint(hit.point);
            var si = hit.transform.GetComponentInParent<ShipIdentification>();
            if(si != null)
            {
                var s = si.ship;

                if (!s.alive)
                    ui.GetAim().SetTarget(AimTarget.Neutral, distance);
                else if (s.team != ship.team)
                {
                    ui.GetAim().SetTarget(AimTarget.Enemy, distance);
                    if (Input.GetKeyDown(KeyCode.Space))
                        lockedEnemy = s;
                }
                else
                    ui.GetAim().SetTarget(AimTarget.Friend, distance);
            }
            else
                ui.GetAim().SetTarget(AimTarget.Neutral, distance);
        }
        else
        {
            ui.GetAim().SetTarget(AimTarget.None, 0);
        }

        if (lockedEnemy != null && lockedEnemy.alive)
            firePoints.AddPoint(lockedEnemy.tempTransform.position);
        uiBehaviour.targetView.SetTargets(firePoints.GetFirePoints().Select(p => p.point));

        SetAim();


        if (Input.GetKeyDown(KeyCode.LeftControl))
            everyTimeFire = !everyTimeFire;
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            everyTimeRotate = !everyTimeRotate;
    }

    void SetAim()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            aimPoint.x = .5f; aimPoint.y = .5f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            aimPoint.x = aimPoint.x < .9f ? aimPoint.x + .1f : aimPoint.x;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            aimPoint.x = aimPoint.x > 0.1f ? aimPoint.x - .1f : aimPoint.x;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            aimPoint.y = aimPoint.y < .9f ? aimPoint.y + .1f : aimPoint.y;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            aimPoint.y = aimPoint.y > 0.1f ? aimPoint.y - .1f : aimPoint.y;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            aimPoint.x = aimPoint.x > 0.1f ? aimPoint.x - .1f : aimPoint.x;
            aimPoint.y = aimPoint.y < .9f ? aimPoint.y + .1f : aimPoint.y;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            aimPoint.x = aimPoint.x < .9f ? aimPoint.x + .1f : aimPoint.x;
            aimPoint.y = aimPoint.y < .9f ? aimPoint.y + .1f : aimPoint.y;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            aimPoint.x = aimPoint.x > 0.1f ? aimPoint.x - .1f : aimPoint.x;
            aimPoint.y = aimPoint.y > 0.1f ? aimPoint.y - .1f : aimPoint.y;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            aimPoint.x = aimPoint.x < .9f ? aimPoint.x + .1f : aimPoint.x;
            aimPoint.y = aimPoint.y > 0.1f ? aimPoint.y - .1f : aimPoint.y;
        }
        else
            return;
        ui.SetAimPosition(aimPoint);
    }
    void SetAim(Vector2 pos)
    {
        aimPoint = pos;
        ui.SetAimPosition(aimPoint);
    }
    protected override void OnRotate(float speed)
    {
        if (speed > 0)
        {
            watcher.MoveToOrigin(speed); // rotate camera to the base position
        }
    }

    public override void OnDead()
    {
        camera.GetComponentInParent<CameraShaker>().Stop();

        BattleBehaviour.AddEvent(watcher.ReleaseCamera, 1);

        BattleBehaviour.current.audioPlayer.SetListener(null);

        ScreenManager.currentManager.MoveTo("DeadView");
    }
    const float shake_boost = 25f;
    public override void OnDamaged(float damage)
    {
        if (ship.alive && damage != float.PositiveInfinity)
            camera.GetComponentInParent<CameraShaker>().Shake(new Vector3(Random.value, Random.value, Random.value).normalized * (damage * shake_boost / shipClass));
    }
}
