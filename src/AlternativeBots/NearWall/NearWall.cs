using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class NearWall : Bot
{
    private bool clockwise = true; // Flag untuk menentukan arah rotasi (searah jarum jam atau tidak)
    private int nearWallDistance = 50; // Jarak minimum dari dinding
    // private bool loc[4];

    static void Main(string[] args)
    {
        new NearWall().Start();
    }

    NearWall() : base(BotInfo.FromFile("NearWall.json")) { }


    public override void Run()
    {
        // Mengatur warna bot
        BodyColor = Color.FromArgb(0x07, 0x8D, 0x70);   // dark green
        TurretColor = Color.FromArgb(0x26, 0xCE, 0xAA); // green
        RadarColor = Color.FromArgb(0x98, 0xE8, 0xC1);  // light green
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // white
        ScanColor = Color.FromArgb(0x7B, 0xAD, 0xE2);   // light blue
        TracksColor = Color.FromArgb(0x50, 0x49, 0xCC); // indigo
        GunColor = Color.FromArgb(0x3D, 0x1A, 0x78);    // blue
        AdjustGunForBodyTurn = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn = true;
        double bear; // Variabel untuk menyimpan sudut bearing
        clockwise = true; // Inisialisasi arah rotasi
        while (IsRunning)
        {
            if(clockwise)
            {
                Console.WriteLine("Rotating clockwise");
            }
            else
            {
                Console.WriteLine("Rotating counter-clockwise");
            }

            // Logika untuk menentukan arah dan pergerakan bot berdasarkan posisi di arena
            if (Y < ArenaHeight / 2)//bawah
            {
                if (X < ArenaWidth / 2)
                {
                    //di kiri bawah
                    
                    if (!clockwise)
                    {
                        //ke kanan bawah
                        if(Math.Abs(BearingTo(ArenaWidth - nearWallDistance, nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(ArenaWidth - nearWallDistance, nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X, Y+400));
                            SetTurnRadarLeft(RadarBearingTo(X, Y+400));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(ArenaWidth - nearWallDistance, nearWallDistance));
                        }
                    }
                    else
                    {
                        //ke kiri atas
                        if(Math.Abs(BearingTo(nearWallDistance, ArenaHeight - nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X+400, Y));
                            SetTurnRadarLeft(RadarBearingTo(X+400, Y));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                        }
                    }
                }
                else
                {
                    //di kanan bawah
                    if (!clockwise)
                    {
                        //ke kanan atas
                        if(Math.Abs(BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X-400, Y));
                            SetTurnRadarLeft(RadarBearingTo(X-400, Y));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            
                            Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                        }
                    }
                    else
                    {
                        //ke kiri bawah
                        if(Math.Abs(BearingTo(nearWallDistance, nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(nearWallDistance, nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X, Y+400));
                            SetTurnRadarLeft(RadarBearingTo(X, Y+400));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(nearWallDistance, nearWallDistance));
                        }
                    }
                }
            }
            else//atas
            {
                // di kiri atas
                if (X < ArenaWidth / 2)
                {
                    if (!clockwise)
                    {
                        //ke kiri bawah
                        if(Math.Abs(BearingTo(nearWallDistance, nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(nearWallDistance, nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X+400, Y));
                            SetTurnRadarLeft(RadarBearingTo(X+400, Y));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(nearWallDistance, nearWallDistance));
                        }
                    }
                    else
                    {
                        //ke kanan atas
                        if(Math.Abs(BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance)) > 1)
                        {

                            ClearEvents();
                            bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X, Y-400));
                            SetTurnRadarLeft(RadarBearingTo(X, Y-400));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            

                            Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                        }
                    }
                }
                else
                {
                    //di kanan atas
                    if (!clockwise)
                    {
                        //ke kiri atas
                        if(Math.Abs(BearingTo(nearWallDistance, ArenaHeight - nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X, Y-400));
                            SetTurnRadarLeft(RadarBearingTo(X, Y-400));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                            
                        }
                    }
                    else
                    {
                        //ke kanan bawah
                        if(Math.Abs(BearingTo(ArenaWidth - nearWallDistance, nearWallDistance)) > 1)
                        {
                            ClearEvents();
                            bear = BearingTo(ArenaWidth - nearWallDistance, nearWallDistance);
                            SetTurnLeft(bear);
                            SetTurnGunLeft(GunBearingTo(X-400, Y));
                            SetTurnRadarLeft(RadarBearingTo(X-400, Y));
                            
                            Go();
                            ClearEvents();
                        }
                        else
                        {
                            
                            Forward(DistanceTo(ArenaWidth - nearWallDistance, nearWallDistance));
                        }
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
        // Mengubah arah rotasi
        Console.WriteLine("Changing direction!");
        if(clockwise){
            clockwise = false;
        }
        else{
            clockwise = true;
        }
    }
}
