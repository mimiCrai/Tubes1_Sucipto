using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class NearWall : Bot
{   private bool turnGun = false;
    private bool clockwise = true;
    private int nearWallDistance = 50;
    static void Main(string[] args)
    {
        new NearWall().Start();
    }

    NearWall() : base(BotInfo.FromFile("NearWall.json")) { }

    
    public override void Run()
    {

        BodyColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        TurretColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        RadarColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        BulletColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        ScanColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        TracksColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        GunColor = Color.FromArgb(0xF0, 0xF0, 0xF0);

        TurnGunLeft(90);
        const double tolerance = 5.0;
        var bear = 0.0;
        var radBear = 0.0;
        clockwise = true;
        
        while (IsRunning)
        { 
            if(turnGun)
            {
                turnGun = false;
                TurnGunLeft(180);
            }
            if(Y < ArenaHeight/2)
            {
                if(X < ArenaWidth/2)
                {
                    if(clockwise)
                    {
                        bear = BearingTo(ArenaWidth - nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, nearWallDistance));
                    }else{
                        bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                }
                else
                {
                    if(clockwise)
                    {
                        bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                    }else{
                        bear = BearingTo(nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, nearWallDistance));
                    }
                }
            }
            else
            {
                if(X < ArenaWidth/2)
                {
                    if(clockwise)
                    {
                        bear = BearingTo(nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, nearWallDistance));
                    }else{
                        bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                }
                else
                {
                    if(clockwise)
                    {
                        bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                    }else{
                        bear = BearingTo(ArenaWidth - nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, nearWallDistance));
                    }
                }
            }
            
        }
        
        
        
    }


    public override void OnScannedBot(ScannedBotEvent e)
    {
        Fire(5);
    }



    public override void OnHitBot(HitBotEvent e)
    {
        if(clockwise)
        {
            clockwise = false;
        }else
        {
            clockwise = true;
        }
        if(!turnGun)
        {
            turnGun = true;
        }else
        {
            turnGun = false;
        }
    }


}
