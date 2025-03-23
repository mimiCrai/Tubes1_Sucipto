using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Stalker : Bot
{   
    // double Tolerance = 5.0;
    bool startCircle = true;
    bool followingEnemy = false;
    int timesScanned = 5;

    static void Main(string[] args)
    {
        new Stalker().Start();
    }

    Stalker() : base(BotInfo.FromFile("Stalker.json")) { }

    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.FromArgb(0x07, 0x8D, 0x70);   // dark green
        TurretColor = Color.FromArgb(0x26, 0xCE, 0xAA); // green
        RadarColor = Color.FromArgb(0x98, 0xE8, 0xC1);  // light green
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // white
        ScanColor = Color.FromArgb(0x7B, 0xAD, 0xE2);   // light blue
        TracksColor = Color.FromArgb(0x50, 0x49, 0xCC); // indigo
        GunColor = Color.FromArgb(0x3D, 0x1A, 0x78);    // blue

        while (IsRunning)
        {   

            double baseRadius = ArenaHeight < ArenaWidth ? ArenaHeight : ArenaWidth;
            double radius = (baseRadius / 2) - 100;
            double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;
            double distance = (Math.Sqrt(Math.Pow(X - centerX, 2) + Math.Pow(Y - centerY, 2))) - radius;

            // jika bot berada di luar daerah patroli, masuk ke dalam daerah terlebih dahulu
            if (distance > 0)
            {
                Console.WriteLine("calling moveToCenter()");
                moveToCenter();
            }

            // jika bot sebelumnya belum membuat lingkaran, maka bot disiapkan terlebih dahulu
            if (startCircle) {
                ClearEvents();
                Console.WriteLine("preparing making circle");
                double currOrientation = BearingTo(centerX, centerY);

                if (currOrientation < 0) 
                {
                    TurnLeft(90 + currOrientation);
                }
                else
                {
                    TurnRight(90 - currOrientation);
                }
            }

            // jika bot sedang mengikuti musuh dan dalam batas penembakan, lakukan scan
            if (followingEnemy && timesScanned > 0)
            {
                scanInFront();
            }

            // jika bot tidak sedang mengikuti musuh, lakukan patroli lingkaran di tengah arena
            else 
            {
                makeCircle();
                timesScanned = 5;
            }
        }
    }

    // jika bot me-scan bot musuh, bereskan patroli dan mulai ikuti musuh
    public override void OnScannedBot(ScannedBotEvent e)
    {  
        double bodyDegreeToEnemy = BearingTo(e.X, e.Y);
        double distanceToEnemy = DistanceTo(e.X, e.Y);

        if (!followingEnemy) 
        {
            // pengikutan musuh dilakukan untuk musuh yang relatif lurus dengan bot
            if (Math.Abs(bodyDegreeToEnemy) < 30)
            {      
                Console.WriteLine("Bot found at (" + e.X + ", " + e.Y + ")");
                ClearEvents(); 
                Forward(70);
                followingEnemy = true;
            }
        }

        // lakukan penembakan pada musuh yang sedang diikuti
        else 
        {
            Console.WriteLine("preparing to shoot");
            ClearEvents();
            TurnLeft(bodyDegreeToEnemy);

            // atur besar peluru berdasarkan jarak dengan musuh
            Fire(countFirePower(distanceToEnemy));
        }
    }

    // fungsi untuk menghitung besar peluru berdasarkan jarak dengan musuh
    public double countFirePower(double distanceToEnemy) 
    {
        if (distanceToEnemy > 100)
        {
            return 1.1;
        }

        if (distanceToEnemy < 50) 
        {
            return 3;
        }

        // 50 <= distanceToEnemy <= 100
        return (distanceToEnemy / 25) - 0.99;

    }

    // fungsi untuk melakukan scan pada musuh
    public void scanInFront()
    {
        setRadarToFront(); 

        // memastikan bot tidak diam di tempat untuk waktu yang lama
        if (timesScanned % 2 == 0) 
        {
            Forward(15);
        }
        else 
        {
            Back(15);
        }
        TurnRadarRight(50);
        TurnRadarLeft(100);
        timesScanned--;  
    }

    // memastikan radar mengarah searah dengan bot
    public void setRadarToFront()
    {
        double botAngle = BearingTo(0, 0);
        double radarAngle = RadarBearingTo(0, 0);
        TurnRadarLeft(radarAngle - botAngle);
    }

    // melakukan pergerakan masuk ke dalam daerah patroli
    public void moveToCenter() 
    {
        startCircle = true; followingEnemy = false;
        double baseRadius = ArenaHeight < ArenaWidth ? ArenaHeight : ArenaWidth;
        double radius = (baseRadius / 2) - 100;
        double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;
        
        // mengubah orientasi bot ke tengah arena
        double bodyAngleToCenter = BearingTo(centerX, centerY);
        Console.WriteLine("Going to the center, turning: " + bodyAngleToCenter + " degrees");
        TurnLeft(bodyAngleToCenter);

        // memajukan bot ke dalam daerah patroli
        double distance = (Math.Sqrt(Math.Pow(X - centerX, 2) + Math.Pow(Y - centerY, 2))) - (radius - 50);
        double radarAngleToCenter = RadarBearingTo(centerX, centerY);
        SetTurnRadarLeft(radarAngleToCenter);
        Forward(distance);
    }

    // melakukan patroli dalam lintasan berbentuk lingkaran
    public void makeCircle()
    {
        startCircle = false; followingEnemy = false;
        double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;
        double distance = (Math.Sqrt(Math.Pow(X - centerX, 2) + Math.Pow(Y - centerY, 2)));
        
        // menyesuaikan arah membentuk lingkaran (ke kiri atau ke kanan) berdasarkan orientasi bot
        if (BearingTo(centerX, centerY) < 0)
        {
            Console.WriteLine("Making circles rn, going right");
            SetTurnRadarRight(17);
            SetTurnRight(3);
        }
        else 
        {
            Console.WriteLine("Making circles rn, going left");
            SetTurnRadarLeft(17);
            SetTurnLeft(3);
        }
        SetForward(20);
        Go();
    }

    // jika bot terkena serangan peluru, segera melakukan pergerakan ke tengah arena
    public override void OnHitByBullet(HitByBulletEvent bulletHitBotEvent)
    {
        
        double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;

        Console.WriteLine("Ouch! a bullet hit me!");
        ClearEvents();
        SetTurnRadarRight(BearingTo(centerX, centerY));
        TurnRight(BearingTo(centerX, centerY));
        Forward(100);
    }

    // jika bot menabrak bot musuh, segera melakukan pergerakan ke tengah arena
    public override void OnHitBot(HitBotEvent botHitBotEvent)
    {

        double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;

        ClearEvents();
        SetTurnRadarRight(BearingTo(centerX, centerY));
        TurnRight(BearingTo(centerX, centerY));
        Forward(100);
    }

    // jika bot menabrak dinding, segera melakukan pergerakan ke tengah arena
    public override void OnHitWall(HitWallEvent e)
    {
        ClearEvents();
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
        double centerX = ArenaWidth / 2; double centerY = ArenaHeight / 2;
        SetTurnRadarRight(BearingTo(centerX, centerY));
        TurnRight(BearingTo(centerX, centerY));
        Forward(100);
    }

    /* Read the documentation for more events and methods */
}
