using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CircleImpact : Bot
{   
    bool followingEnemy, startingCircle, goingClockwise;
    int turnToShoot = 20;
    int firstHundredTurns, bouncingBackTurn;
    double centerX, centerY;
    
    static void Main(string[] args)
    {
        new CircleImpact().Start();
    }

    CircleImpact() : base(BotInfo.FromFile("CircleImpact.json")) { }

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
        
        double radius = 150;
        centerX = ArenaWidth / 2; centerY = ArenaHeight / 2;
        firstHundredTurns = 200; bouncingBackTurn = 0;
        followingEnemy = false; startingCircle = true; goingClockwise = true;

        while (IsRunning)
        {            
            double distance = Math.Abs((Math.Sqrt(Math.Pow(X - centerX, 2) + Math.Pow(Y - centerY, 2))));
            Console.WriteLine("nilai firstHundredTurns: " + firstHundredTurns);
            Console.WriteLine("nilai followingEnemy: " + followingEnemy);

            // jika bot sedang bertabrakan dengan bot lain atau tembok, 
            // utamakan untuk kembali ke dalam momentum terlebih dahulu
            if (bouncingBackTurn <= 0)
            {

                // jika bot tidak sedang mengikuti musuh, lakukan patroli
                // dalam lintasan melingkar
                if (!followingEnemy)
                {
                    double angleToCenter = BearingTo(centerX, centerY);
                    Console.WriteLine("nilai bearingTo: " + BearingTo(centerX, centerY));

                    // pastikan bot berada dalam radius tertentu dari center arena
                    if (distance > radius) 
                    {
                        if (angleToCenter != 0)
                        {
                            SetTurnLeft(angleToCenter);
                        }
                    
                        SetForward(20);
                        startingCircle = true;
                    }

                    // setelah bot berada dalam radius tertentu, lakukan patroli melingkar
                    else
                    {
                        if (goingClockwise)
                        {
                            if (startingCircle) 
                            {
                                SetTurnLeft(90);
                                startingCircle = false;
                            }
                            else if (TurnRemaining == 0)
                            {
                                SetTurnRight(45);
                                SetForward(distance);
                            }
                        }
                        else
                        {
                            if (startingCircle) 
                            {
                                SetTurnRight(90);
                                startingCircle = false;
                            }
                            else if (TurnRemaining == 0)
                            {
                                SetTurnLeft(45);
                                SetForward(distance);
                            }
                        }
                    }
                }

                // jika bot mendeteksi ada musuh di sekitarnya
                else
                {

                    // lakukan penyerangan jika 200 turns pertama telah dilewati
                    if (firstHundredTurns <= 0)
                    {
                        Console.WriteLine("nilai turnToShoot: " + turnToShoot);
                        SetTurnLeft(60);
                        SetForward(10);

                        // lakukan penyerangan selama maksimal 20 turns untuk satu musuh,
                        // sebelum mencoba untuk mencari musuh yang lain
                        if (turnToShoot < 0)
                        {
                            ClearEvents();
                            followingEnemy = false;
                            turnToShoot = 20;
                        } 
                    }
                }
                
                // lakukan scanning musuh setelah 200 turns pertama telah dilewati
                if (firstHundredTurns <= 0)
                { 
                    SetTurnRadarLeft(60);
                }
            }
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        
        double distance = DistanceTo(e.X, e.Y);

        // jika awalnya tidak mengikuti musuh, ikuti musuh 
        // yang terkena scan radar dalam kondisi tertentu:
        // distance terhadap musuh tidak jauh dan sudah melalui 200 turns pertama
        if (!followingEnemy)
        {        
            Console.WriteLine("I see a bot at " + e.X + ", " + e.Y);

            // musuh tidak diikuti jika jarak dari bot kurang dari 300
            if (distance > 300)
            {
                Console.WriteLine("but it's too far away");
            }

            else
            {

                // musuh tidak diikuti jika 200 turns pertama belum dilewati
                if (firstHundredTurns <= 0)
                {
                    ClearEvents();
                    followingEnemy = true;
                }
            }
        }

        // jika sedang mengikuti musuh, lakukan penembakan secara brutal
        if (followingEnemy)
        {
            var bearingFromGun = GunBearingTo(e.X, e.Y);
            SetTurnGunLeft(bearingFromGun);
            if (Energy >= 30)
            { 
                SetFire(2.5);
            }
            else
            {
                SetFire(1.5);
            }
        }
    }

    // fungsi untuk menghitung jumlah turn yang telah dilewati berdasarkan suatu kondisi
    public override void OnTick(TickEvent tickEvent)
    {
        if (bouncingBackTurn > 0)
        {
            Console.WriteLine("nilai bouncingBackTurn: " + bouncingBackTurn);
            bouncingBackTurn--;
        }

        else 
        {
            if (firstHundredTurns > 0)
            {
                firstHundredTurns--;
            }

            if (followingEnemy)
            {
                turnToShoot--;
            }
        }
    }

    // jika bot mengenai bot lain, lakukan retreat dan abaikan perintah lainnya
    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Ouch! I hit a bot at " + e.X + ", " + e.Y);
        bouncingBackTurn = 10;

        if (goingClockwise) 
        {
            goingClockwise = false;
        }
        else
        {
            goingClockwise = true;
        }
        TurnLeft(180);
        Forward(100);
    }

    // jika bot mengenai dinding, lakukan retreat dan abaikan perintah lainnya
    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
        bouncingBackTurn = 10;

        double bearing = BearingTo(centerX, centerY);
        TurnLeft(bearing);
        Forward(100);

    }

    /* Read the documentation for more events and methods */
}
