using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class TRIPIN : Bot
{   
    /* A bot that drives forward and backward, and fires a bullet */
    private int county = 0;
    private int bounty = 1;
    private bool RESETTING = true;
    static void Main(string[] args)
    {
        new TRIPIN().Start();
    }

    TRIPIN() : base(BotInfo.FromFile("TRIPIN.json")) { }

    public override void Run()
    {
        /* Customize bot colors, read the documentation for more information */
        BodyColor = Color.Gray;
        while (IsRunning)
        {

            if(RESETTING){
                TurnLeft(BearingTo(ArenaWidth / 2, ArenaHeight / 2));
                Forward(DistanceTo(ArenaWidth / 2, ArenaHeight / 2));
                RESETTING = false;
            }else{
                county++;
                if((county/6)%16<=4){//coba 22, 28, 34, 40.......... 192.168.1.1
                    SetTurnLeft(0);
                    SetForward(20);
                }
                
                else
                {
                    SetForward(5);
                    SetTurnLeft(30);
                }
                Go();   
            }

        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine("I see a bot at " + e.X + ", " + e.Y);
        SetFire(5);
        double dis = DistanceTo(e.X, e.Y);
        if(dis < 10.0 && Energy>60){
            SetFire(50);
        }else if(dis < 10.0 && Energy > 40){
            SetFire(30);
        }
        else 
        if(dis < 10.0 && Energy>20){
            SetFire(10);
        }
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Ouch! I hit a bot at " + e.X + ", " + e.Y);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        ClearEvents();
        RESETTING = true;
        Console.WriteLine("Ouch! I hit a wall, must turn back!");
    }

    /* Read the documentation for more events and methods */
}
