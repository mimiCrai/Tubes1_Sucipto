using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class NearWall : Bot
{
    private bool turnGun = false; // Flag untuk menentukan apakah turret harus berputar
    private bool clockwise = true; // Flag untuk menentukan arah rotasi (searah jarum jam atau tidak)
    private int nearWallDistance = 50; // Jarak minimum dari dinding

    static void Main(string[] args)
    {
        new NearWall().Start();
    }

    NearWall() : base(BotInfo.FromFile("NearWall.json")) { }


    public override void Run()
    {
        // Mengatur warna bot
        BodyColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        TurretColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        RadarColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        BulletColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        ScanColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        TracksColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        GunColor = Color.FromArgb(0xF0, 0xF0, 0xF0);

        TurnGunLeft(90); // Memutar turret ke kiri 90 derajat

        double bear; // Variabel untuk menyimpan sudut bearing
        clockwise = true; // Inisialisasi arah rotasi

        while (IsRunning)
        {
            // Jika flag turnGun aktif, putar turret 180 derajat
            if (turnGun)
            {
                turnGun = false;
                TurnGunLeft(180);
            }

            // Logika untuk menentukan arah dan pergerakan bot berdasarkan posisi di arena
            if (Y < ArenaHeight / 2)//bawah
            {
                if (X < ArenaWidth / 2)
                {
                    //di kiri bawah
                    if (clockwise)
                    {
                        //ke kiri atas
                        bear = BearingTo(ArenaWidth - nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, nearWallDistance));
                    }
                    else
                    {
                        //ke kanan bawah
                        bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                }
                else
                {
                    //di kanan bawah
                    if (clockwise)
                    {
                        //ke kanan atas
                        bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                    else
                    {
                        //ke kiri bawah
                        bear = BearingTo(nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, nearWallDistance));
                    }
                }
            }
            else//atas
            {
                // di kiri atas
                if (X < ArenaWidth / 2)
                {
                    if (clockwise)
                    {
                        //ke kiri bawah
                        bear = BearingTo(nearWallDistance, nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, nearWallDistance));
                    }
                    else
                    {
                        //ke kanan atas
                        bear = BearingTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(ArenaWidth - nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                }
                else
                {
                    //di kanan atas
                    if (clockwise)
                    {
                        //ke kiri atas
                        bear = BearingTo(nearWallDistance, ArenaHeight - nearWallDistance);
                        TurnLeft(bear);
                        Forward(DistanceTo(nearWallDistance, ArenaHeight - nearWallDistance));
                    }
                    else
                    {
                        //ke kanan bawah
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
        Fire(5); // Menembak dengan kekuatan 5
    }

    public override void OnHitBot(HitBotEvent e)
    {
        // Mengubah arah rotasi
        clockwise = !clockwise;

        // Mengubah flag turnGun
        turnGun = !turnGun;
    }
}
