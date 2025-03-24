using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class TRIPIN : Bot
{   
    /* A bot that drives forward and backward, and fires a bullet */
    private int county = 0;
    private bool RESETTING = true;
    static void Main(string[] args)
    {
        new TRIPIN().Start();
    }

    TRIPIN() : base(BotInfo.FromFile("TRIPIN.json")) { }

    public override void Run()
    {
        BodyColor = Color.FromArgb(0x07, 0x8D, 0x70);   // dark green
        TurretColor = Color.FromArgb(0x26, 0xCE, 0xAA); // green
        RadarColor = Color.FromArgb(0x98, 0xE8, 0xC1);  // light green
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // white
        ScanColor = Color.FromArgb(0x7B, 0xAD, 0xE2);   // light blue
        TracksColor = Color.FromArgb(0x50, 0x49, 0xCC); // indigo
        GunColor = Color.FromArgb(0x3D, 0x1A, 0x78);    // blue
        while (IsRunning)
        {

            if(RESETTING){// menabrak dinding, maka kembali ke tengah
                TurnLeft(BearingTo(ArenaWidth / 2, ArenaHeight / 2));
                Forward(DistanceTo(ArenaWidth / 2, ArenaHeight / 2));
                RESETTING = false;
            }else{
                //melakukan pembuatan pola pada gerakan
                county++;
                if((county/6)%16<=4){
                    SetTurnLeft(0);
                    SetForward(20);
                }
                else
                {
                    SetForward(5);
                    SetTurnLeft(30);
                }
            }
            Go();   
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine("I see a bot at " + e.X + ", " + e.Y);

        //langsung tembak saat melihat lawan
        SetFire(5);

        //Apabila jarak sangat dekat, beri power sangat tinggi
        double dis = DistanceTo(e.X, e.Y);
        if(dis < 25.0 && Energy>60){
            SetFire(50);
        }else if(dis < 25.0 && Energy > 40){
            SetFire(30);
        }
        else 
        if(dis < 25.0 && Energy>20){
            SetFire(10);
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        //mereposisi ke tengah
        ClearEvents();
        RESETTING = true;
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
    }
}
