using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Suniper2 : Bot
{   
    private const int MODI = (1<<23); //untuk warna (UNUSED)
    private bool runWall = false; // untuk status kena dinding
    private int maju = 0; // untuk maju mundur
    private bool shoot = false;// untuk menembak
    private bool startCheck = false;// untuk mulai mengecek ancaman terdekat
    private int ColorChoice = 0; //UNUSED
    private ScannedBotEvent en;// untuk mengecek ancaman terdekat
    private ScannedBotEvent enSHOOT;// untuk menembak
    private double minX, minY, minDist;

    static void Main(string[] args)
    {
        new Suniper2().Start();
    }

    Suniper2() : base(BotInfo.FromFile("Suniper2.json")) { }

    
    public override void Run()
    {
        // warna
        BodyColor = Color.FromArgb(0x07, 0x8D, 0x70);   // dark green
        TurretColor = Color.FromArgb(0x26, 0xCE, 0xAA); // green
        RadarColor = Color.FromArgb(0x98, 0xE8, 0xC1);  // light green
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // white
        ScanColor = Color.FromArgb(0x7B, 0xAD, 0xE2);   // light blue
        TracksColor = Color.FromArgb(0x50, 0x49, 0xCC); // indigo
        GunColor = Color.FromArgb(0x3D, 0x1A, 0x78);    // blue

        // rotasi independen
        AdjustGunForBodyTurn = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn = true;

        // ancaman terdekat dibuat di tengah agar pergi ke pojok
        minX = ArenaWidth / 2;
        minY = ArenaHeight / 2;
        minDist = DistanceTo(minX,minY);

        // mulai
        while (IsRunning)
        {
            // IntToRGB();
            SetTurnRadarRight(double.PositiveInfinity); //putar radar 360 derajat untuk mengecek ancaman
            if(startCheck){ // cek ancaman terdekat
                double dist = DistanceTo(en.X, en.Y);
                if(dist < minDist || EnemyCount == 1){
                    minDist = dist;
                    minX = en.X;
                    minY = en.Y;
                }
            }
            
            if(shoot){ // cek tembak
                TEMBAK();
            }

            // cek ancaman terdekat
            minDist = DistanceTo(minX,minY);
            
            if(minDist < 200) // dikejarr, bahaya
            {
                Console.WriteLine("Threat at: " + minX + ", " + minY);
                SetTurnLeft(BearingTo(minX,minY));
                SetBack(90);
                
                //dekat wall, putar arah
                if(nearWall()){
                    double choice = -90;
                    if(Y < 150){
                        if(BearingTo(X,0) > 0){
                            choice = 90;
                        }
                    }else if(Y > ArenaHeight - 150){
                        if(BearingTo(X,ArenaHeight) > 0){
                            choice = 90;
                        }
                    }
                    if(X < 150){
                        if(BearingTo(0,Y) > 0){
                            choice = 90;
                        }
                    }else if(X > ArenaWidth - 150){
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
                Console.WriteLine("Running away, rotation: " + TurnRemaining);

            }else{// tidak dikejar, lakukan gerakan maju mundur
                maju++;// add counter
                if(Math.Abs(maju)>=75){//diam
                    SetForward(0);
                }else if(maju>=50){//maju
                    SetForward(70);
                }else if(maju>=25){//diam
                    SetBack(0);
                }else{//mundur
                    SetBack(70);
                }
                if(maju==100){maju*=0;}// reset counter
                
                //putar ke arah ancaman, dibuat 90 derajat agar gerak maju mundur membuat lebih susah ditembak
                SetTurnLeft(BearingTo(minX,minY) - 90); 
            }

            // cek apakah kena dinding
            if(runWall){
                stabilize();
            }
            Go();
        }
    }    
    
    private void stabilize(){
        Console.WriteLine("Stabilizing...");

        // cek X baru agar stabil
        double newX2 = X;
        if(X < 200){
            newX2 = 200;
        }else if(X > ArenaWidth - 200){
            newX2 = ArenaWidth - 200;
        }

        // cek Y baru agar stabil
        double newY2 = Y;
        if(Y < 200){
            newY2 = 200;
        }else if(Y > ArenaHeight - 200){
            newY2 = ArenaHeight - 200;
        }

        // cek sudut agar stabil
        double angleRun = BearingTo(newX2,newY2);
        while(angleRun > 180){
            angleRun -= 360;
        }
        while(angleRun < -180){
            angleRun += 360;
        }

        // stabilkan
        SetTurnLeft(angleRun);
        SetForward(200);
        MaxSpeed = 10;

        // selalu cek apakah sudah stabil
        if(X>=200 && X<=ArenaWidth-200 && Y>=200 && Y<=ArenaHeight-200){
            Console.WriteLine("Safe!");
            runWall = false;
            MaxSpeed = 100;
        }
    }
    private bool nearWall(){// cek dekat dinding
        return X < 150 || X > ArenaWidth - 150 || Y < 150 || Y > ArenaHeight - 150;
    }

    private void TEMBAK(){
        if(GunHeat == 0) // Mengecek cooldown
        {
            double dist = DistanceTo(enSHOOT.X, enSHOOT.Y);
            double firePow = calcPower(dist);
            double gunAngle = calcAngle(firePow);
            SetTurnGunLeft(gunAngle);

            // Apabila sudut tembak sudah mendekatik lokasi prediksi, tembak.
            if(Math.Abs(GunTurnRemaining) < 2.5)
            {    
                SetFire(firePow);
                shoot = false;
            }
        }
        else// belum bisa menembak
        {
            shoot = false;
        }
    }
    private double calcAngle(double firepower) //menghitung sudut tembak
    {
        double time = DistanceTo(enSHOOT.X, enSHOOT.Y) / CalcBulletSpeed(firepower);
        double newX = enSHOOT.X + enSHOOT.Speed * time * Math.Cos(ToRadians(enSHOOT.Direction));
        double newY = enSHOOT.Y + enSHOOT.Speed * time * Math.Sin(ToRadians(enSHOOT.Direction));
        double gunAngle = GunBearingTo(newX,newY);
        while (gunAngle > 180){ gunAngle -= 360; }
        while (gunAngle < -180){ gunAngle += 360; }
        return gunAngle;
    }
    private double calcPower(double dist) //menghitung power
    {
        double firePow = 0;
        //sangat dekat, fullpower
        if(dist < 50){
            // berdasarkan energi
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
        else if(dist < 150) // jarak sedang, energi secukupnya
        {
            firePow = Math.Min(3, Energy/7);
            if(Energy < 20){
                firePow = Math.Min(firePow , Energy/10);
            }
            else if(Energy < 10){// simpan energi
                firePow = 0;
            }
        }
        else
        {   
            if(dist < 250){// cukup jauh, energi secukupnya
                firePow = Math.Min(3, Energy/17);
            }else if(dist<400){// jauh, kekuatan dikurangi
                firePow = Math.Min(2, Energy/40);
            }else{// sangat jauh, kekuatan dikurangi lagi
                firePow = Math.Min(1, Energy/45);
            }
            if(Energy < 10){// simpan energi
                firePow = 0;
            }
        }
        return firePow;
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine("I see a bot at " + e.X + ", " + e.Y);
        //digunakan untuk mengecek ancaman terdekat
        en = e;

        if(!shoot){//tembak bila bisa
            enSHOOT = e;
            shoot = true;
        }

        //mulai cek ancaman terdekat
        startCheck = true;
    }



    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("I hit a bot at " + e.X + ", " + e.Y);
        // membuat bot yang menabrak sebagai ancaman agar bisa menjauh
        minDist = DistanceTo(e.X, e.Y);
        minX = e.X;
        minY = e.Y;
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("HitWall!");
        // mengeset runwall untuk kabur dari dinding
        runWall = true;
    }
    private double ToRadians(double degrees) => degrees * Math.PI / 180;


//set warna berganti-ganti (UNUSED)
    private void IntToRGB()
    {
        if(ColorChoice%2==0){
            ColorChoice+=127;
        }else{
            ColorChoice+=8193;
        }
        ColorChoice%=MODI;
        int r = (ColorChoice >> 16) & 0xFF;
        int g = (ColorChoice >> 8) & 0xFF;
        int b = ColorChoice & 0xFF;
        BodyColor = Color.FromArgb(r, g, b);
        TurretColor = Color.FromArgb(r, g, b);
        RadarColor = Color.FromArgb(r, g, b);
        BulletColor = Color.FromArgb(r, g, b);
        ScanColor = Color.FromArgb(r, g, b);
        TracksColor = Color.FromArgb(r, g, b);
        GunColor = Color.FromArgb(r, g, b);
    }

}
