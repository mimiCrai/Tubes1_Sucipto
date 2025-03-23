using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Sniper2 : Bot
{   
    private bool runWall = false;
    private int maju = 0;
    private bool shoot = false;
    private bool startCheck = false;
    private ScannedBotEvent en;
    private ScannedBotEvent enSHOOT;
    private double minX, minY, minDist = double.PositiveInfinity;
    // double MaxRadarTurnRate;
    // bool Interruptible = 0;

    static void Main(string[] args)
    {
        new Sniper2().Start();
    }

    Sniper2() : base(BotInfo.FromFile("Sniper2.json")) { }

    
    public override void Run()
    {

        BodyColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        TurretColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        RadarColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        ScanColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        TracksColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        GunColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        AdjustGunForBodyTurn = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn = true;
        minX = ArenaWidth / 2;
        minY = ArenaHeight / 2;
        minDist = DistanceTo(minX,minY);
        while (IsRunning)
        {
            if(startCheck){
                double dist = DistanceTo(en.X, en.Y);
                if(dist < minDist || EnemyCount == 1){
                    minDist = dist;
                    minX = en.X;
                    minY = en.Y;
                }
            }
            if(shoot){
                TEMBAK();
            }
            SetTurnRadarRight(double.PositiveInfinity);
            minDist = DistanceTo(minX,minY);
            if(minDist < 250) // dikejarr
            {
                SetTurnLeft(BearingTo(minX,minY));
                SetBack(100);
                if(nearWall()){
                    double choice = -90;
                    if(Y < 100){
                        if(BearingTo(X,0) > 0){
                            choice = 90;
                        }
                    }else if(Y > ArenaHeight - 100){
                        if(BearingTo(X,ArenaHeight) > 0){
                            choice = 90;
                        }
                    }
                    if(X < 100){
                        if(BearingTo(0,Y) > 0){
                            choice = 90;
                        }
                    }else if(X > ArenaWidth - 100){
                        if(BearingTo(ArenaWidth,Y) > 0){
                            choice = 90;
                        }
                    }
                    if(choice < 0 && BearingTo(minX,minY) > 0){
                        SetTurnLeft(choice);
                    }else if(choice > 0 && BearingTo(minX,minY) < 0){
                        SetTurnLeft(choice);
                    }
                    SetBack(60);
                }
            }else{
                SetStop();
                maju++;
                if(Math.Abs(maju)>=75){
                    SetForward(0);
                }else if(maju>=50){
                    SetForward(80);
                    if(nearWall()){
                        stabilize();
                    }
                }else if(maju>=25){
                    SetBack(0);
                }else{
                    SetBack(80);
                    if(nearWall()){
                        stabilize();
                    }
                }
                if(maju==100){maju*=0;}
                SetTurnLeft(BearingTo(minX,minY) + 90);
            }
            if(runWall){
                stabilize();
            }
            Go();
        }
    }

    private void stabilize(){
        Console.WriteLine("Stabilizing...");
        double newX2 = X;
        if(X < 100){
            newX2 = 100;
        }else if(X > ArenaWidth - 100){
            newX2 = ArenaWidth - 100;
        }

        double newY2 = Y;
        if(Y < 100){
            newY2 = 100;
        }else if(Y > ArenaHeight - 100){
            newY2 = ArenaHeight - 100;
        }
        double angleRun = BearingTo(newX2,newY2);
        while(angleRun > 180){
            angleRun -= 360;
        }
        while(angleRun < -180){
            angleRun += 360;
        }
        SetTurnLeft(angleRun);
        SetForward(100);
        MaxSpeed = 10;
        if(X>=100 && X<=ArenaWidth-100 && Y>=100 && Y<=ArenaHeight-100){
            Console.WriteLine("Safe!");
            runWall = false;
            MaxSpeed = 100;
        }
    }
    private bool nearWall(){
        return X < 150 || X > ArenaWidth - 150 || Y < 150 || Y > ArenaHeight - 150;
    }

    private void TEMBAK(){
        double dist = DistanceTo(enSHOOT.X, enSHOOT.Y);
        
        double firePow = 0;
        if(GunHeat == 0){
            if(dist < 50){
                if(Energy > 30){
                    firePow = 20;
                }else if(Energy>20){
                    firePow = 5;
                }else if(Energy > 10){
                    firePow = 2;
                }else{
                    firePow = 1;
                }
            }
            else if(dist < 150)
            {
                firePow = Math.Min(3, Energy/7);
                if(Energy < 20){
                    firePow = Math.Min(firePow , Energy/10);
                }
                else if(Energy < 10){
                    firePow = 0;
                }
            }
            else
            {   
                if(dist < 250){
                    firePow = Math.Min(3, Energy/17);
                }else if(dist<400){
                    firePow = Math.Min(2, Energy/40);
                }else{
                    firePow = Math.Min(1, Energy/45);
                }
                if(Energy < 10){
                    firePow = 0;
                }
            }
            double newX = enSHOOT.X, newY = enSHOOT.Y, time, gunAngle;
            time = DistanceTo(enSHOOT.X, enSHOOT.Y) / CalcBulletSpeed(firePow);
            newX = enSHOOT.X + enSHOOT.Speed * time * Math.Cos(ToRadians(enSHOOT.Direction));
            newY = enSHOOT.Y + enSHOOT.Speed * time * Math.Sin(ToRadians(enSHOOT.Direction));
            gunAngle = GunBearingTo(newX,newY);
            while (gunAngle > 180){
                gunAngle -= 360;
            }
            while (gunAngle < -180){
                gunAngle += 360;
            }
            SetTurnGunLeft(gunAngle);
            if(Math.Abs(GunTurnRemaining) < 2.5){
                
                SetFire(firePow);
                shoot = false;
            }

        }
    }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine("I see a bot at " + e.X + ", " + e.Y);
        en = e;
        if(!shoot){
            enSHOOT = e;
            shoot = true;
        }
        startCheck = true;
    }



    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("I hit a bot at " + e.X + ", " + e.Y);
        minDist = DistanceTo(e.X, e.Y);
        minX = e.X;
        minY = e.Y;
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("HitWall!");
        runWall = true;
    }
    private double ToRadians(double degrees) => degrees * Math.PI / 180;



}
