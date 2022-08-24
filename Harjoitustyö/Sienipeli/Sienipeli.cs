using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;

///@author Pekko
///@Version 1.0

namespace Sienipeli
{
    /// <summary>
    /// Luo pelikentän. Käsittelee hahmon controllit, sekä interaktiot pelikentän objektien kanssa.
    /// </summary>
    public class Sienipeli : PhysicsGame
    {
        private const double OMENANOPEUS = 250;
        private const double NOPEUS = 200;
        private const double HYPPYNOPEUS = 730;
        private const int RUUDUN_KOKO = 40;
        private IntMeter pisteet;

        private PlatformCharacter hahmo;
        private PlatformCharacter omena;

        private Image hahmonKuva; 
        private Image tahtiKuva; 
        private Image alaTasoKuva; 
        private Image keskiTasoKuva; 
        private Image ylaTasoKuva; 
        private Image omenaKuva; 
        private Image lippuKuva; 
        private Image shokki1; 
        private Image shokki2; 
        private Image omena2; 
        private Image sahkoOmena; 
        private Image taustakuva; 
        private static Image[] animaatio;

        /// <summary>
        /// Pelin kuvatiedostojen lataus.
        /// </summary>
        public Sienipeli()
        {
            hahmonKuva = LoadImage("sieni.png");
            tahtiKuva = LoadImage("tahti.png");
            alaTasoKuva = LoadImage("alataso.png");
            keskiTasoKuva = LoadImage("keskitaso.png");
            ylaTasoKuva = LoadImage("ylataso.png");
            omenaKuva = LoadImage("omena.png");
            lippuKuva = LoadImage("lippu.png");
            shokki1 = LoadImage("sieni4.png");
            shokki2 = LoadImage("sieni5.png");
            omena2 = LoadImage("omena2.png");
            sahkoOmena = LoadImage("SahkoOmena.png");
            taustakuva = LoadImage("taustankuva.png");
            animaatio = new Image[] { shokki1, shokki2, hahmonKuva};
        }

        /// <summary>
        /// Pelikentän luonti.
        /// </summary>
        private void LuoKentta()
        {
            TileMap kentta = TileMap.FromLevelAsset("kentta1.txt");
            kentta.SetTileMethod('#', LisaaTaso);
            kentta.SetTileMethod('*', LisaaTahti);
            kentta.SetTileMethod('N', LisaaHahmo);
            kentta.SetTileMethod('O', LisaaOmena);
            kentta.SetTileMethod('T', LisaaAlataso);
            kentta.SetTileMethod('Y', LisaaKeskitaso);
            kentta.SetTileMethod('U', LisaaYlataso);
            kentta.SetTileMethod('L', LisaaMaali);
            kentta.SetTileMethod('A', LisaaAita);
            kentta.Execute(RUUDUN_KOKO, RUUDUN_KOKO);
            Level.CreateBorders();           
            Level.Background.Image = taustakuva;
            Level.Background.ScaleToLevelByWidth();
        }

        /// <summary>
        /// Pistelaskurin lisäys kenttään.
        /// </summary>
        private void LisaaLaskuri()
        {
            pisteet = LuoPisteLaskuri(Screen.Right - 70, Screen.Top - 70);
        }

        /// <summary>
        /// Pistelaskurin määritys.
        /// </summary>
        /// <param name="x">pistenäytön X -koordinantti.</param>
        /// <param name="y">pistenäytön Y -koordinantti.</param>
        /// <returns></returns>
        IntMeter LuoPisteLaskuri(double x, double y)
        {
            IntMeter laskuri = new IntMeter(0);
            laskuri.MinValue = 0;

            Label naytto = new Label();
            naytto.BindTo(laskuri);
            naytto.X = x;
            naytto.Y = y;
            naytto.TextColor = Color.White;
            naytto.Title = "Pisteet: ";
            Add(naytto);
            return laskuri;
        }

        /// <summary>
        /// Maalin määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaMaali(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject maali = PhysicsObject.CreateStaticObject(leveys, korkeus);
            maali.Position = paikka;
            maali.Image = lippuKuva;
            maali.IgnoresCollisionResponse = true;
            maali.Tag = "maali";
            Add(maali);
        }

        /// <summary>
        /// Alatason määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaAlataso(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject alaTaso = PhysicsObject.CreateStaticObject(leveys, korkeus);
            alaTaso.Position = paikka;
            alaTaso.Image = alaTasoKuva;
            Add(alaTaso, -1);
        }

        /// <summary>
        /// Keskitason määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaKeskitaso(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject keskiTaso = PhysicsObject.CreateStaticObject(leveys, korkeus);
            keskiTaso.Position = paikka;
            keskiTaso.Image = keskiTasoKuva;
            Add(keskiTaso, -1);
        }

        /// <summary>
        /// Ylätason määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaYlataso(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject ylaTaso = PhysicsObject.CreateStaticObject(leveys, korkeus);
            ylaTaso.Position = paikka;
            ylaTaso.Image = ylaTasoKuva;
            Add(ylaTaso, -1);
        }

        /// <summary>
        /// Pelitasojen määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaTaso(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
            taso.Position = paikka;
            taso.Color = Color.Brown;
            Add(taso, -1);
        }

        /// <summary>
        /// Tähtien määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaTahti(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject tahti = PhysicsObject.CreateStaticObject(leveys, korkeus);
            tahti.IgnoresCollisionResponse = true;
            tahti.Position = paikka;
            tahti.Image = tahtiKuva;
            tahti.IgnoresCollisionResponse = true;
            tahti.Tag = "tahti";
            Add(tahti, 1);
        }

        /// <summary>
        /// Hahmon määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaHahmo(Vector paikka, double leveys, double korkeus)
        {
            hahmo = new PlatformCharacter(leveys, korkeus);
            hahmo.Position = paikka;
            hahmo.Image = hahmonKuva;
            hahmo.CollisionIgnoreGroup = 1;
            AddCollisionHandler(hahmo, "tahti", TormaaTahteen);
            AddCollisionHandler(hahmo, "maali", TormaaMaaliin);
            AddCollisionHandler(hahmo, "omena", TormaaOmenaan);
            Add(hahmo);
        }

        /// <summary>
        /// Omenoiden määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaOmena(Vector paikka, double leveys, double korkeus)
        {
            omena = new PlatformCharacter(leveys, korkeus);
            omena.Position = paikka;
            omena.Mass = 2.0;
            omena.Image = omenaKuva;
            RandomMoverBrain liikutaan = new RandomMoverBrain(OMENANOPEUS);
            liikutaan.ChangeMovementSeconds = 1;
            omena.Brain = liikutaan;
            omena.Tag = "omena";
            Add(omena);
        }

        /// <summary>
        /// Omenoiden aitojen määritys.
        /// </summary>
        /// <param name="paikka"></param>
        /// <param name="leveys"></param>
        /// <param name="korkeus"></param>
        private void LisaaAita(Vector paikka, double leveys, double korkeus)
        {
            PhysicsObject aita = PhysicsObject.CreateStaticObject(leveys, korkeus);
            aita.Position = paikka;
            aita.CollisionIgnoreGroup = 1;
            aita.Color = Color.Transparent;
            Add(aita);

        }

        /// <summary>
        /// Pelinäppäinten määritys.
        /// </summary>
        private void LisaaNappaimet()
        {
            Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

            Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", hahmo, -NOPEUS);
            Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", hahmo, NOPEUS);
            Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Hahmo hyppää", hahmo, HYPPYNOPEUS);
        }

        /// <summary>
        /// Hahmon liikkeen määritys.
        /// </summary>
        /// <param name="hahmo"></param>
        /// <param name="nopeus"></param>
        private void Liikuta(PlatformCharacter hahmo, double nopeus)
        {
            hahmo.Walk(nopeus);
        }

        /// <summary>
        /// Hahmon hypyn määritys.
        /// </summary>
        /// <param name="hahmo"></param>
        /// <param name="nopeus"></param>
        private void Hyppaa(PlatformCharacter hahmo, double nopeus)
        {
            hahmo.Jump(nopeus);
        }

        /// <summary>
        /// Toteuttaa tähden keräämisen.
        /// </summary>
        /// <param name="hahmo"></param>
        /// <param name="tahti"></param>
        private void TormaaTahteen(PhysicsObject hahmo, PhysicsObject tahti)
        {
            tahti.Destroy();
            pisteet.Value = pisteet.Value + 1;
        }

        /// <summary>
        /// Toteuttaa maaliin pääsemisen.
        /// </summary>
        /// <param name="hahmo"></param>
        /// <param name="maali"></param>
        private void TormaaMaaliin(PhysicsObject hahmo, PhysicsObject maali)
        {
            MultiSelectWindow loppuvalikko = new MultiSelectWindow("Pääsit maaliin! Sait yhteensä  " + pisteet + " pistettä!", "Uusi peli", "Lopeta");
            loppuvalikko.AddItemHandler(0, UusiPeli);
            loppuvalikko.AddItemHandler(1, Exit);
            Add(loppuvalikko);
        }

        /// <summary>
        /// Toteuttaa uuden pelin aloituksen.
        /// </summary>
        private void UusiPeli()
        {
            ClearAll();

            Gravity = new Vector(0, -1000);
            LuoKentta();
            LisaaNappaimet();
            LisaaLaskuri();
            Camera.Follow(hahmo);
            Camera.ZoomFactor = 1.2;
            Camera.StayInLevel = true;

            MasterVolume = 0.0;
        }

        /// <summary>
        /// Toteuttaa interaktiot omenan kanssa.
        /// </summary>
        /// <param name="hahmo"></param>
        /// <param name="omena"></param>
        private void TormaaOmenaan(PhysicsObject hahmo, PhysicsObject omena)
        {  

            //Määrittää tapahtuman, kun hahmo osuu omenaan hyppäämällä.
            if(hahmo.Bottom +2 >= omena.Top)
            {               
                omena.IgnoresCollisionResponse = true;
                Timer.CreateAndStart(2, omena.Destroy);
                pisteet.Value += 2;
                omena.Tag = "";
                omena.Image = omena2;

            }

            // Määrittää tapahtuman, kun omena osuu hahmoon sivusta päin.
            else
            {
                int pituus = animaatio.Length;

                pisteet.Value = pisteet.Value - 1;
                omena.Image = sahkoOmena;
                Timer.SingleShot(0.2,
                    delegate { omena.Image = omenaKuva; }
                    );


                double delay = 0.1;
                foreach (var a in animaatio)
                {
                    Timer.SingleShot(delay,
                        delegate { hahmo.Image = a; }
                        );
                    delay += 0.1;
                }
            }
        }

        /// <summary>
        /// Pelin ja kentän käynnistys.
        /// </summary>
        public override void Begin()
        {
            Gravity = new Vector(0, -1000);

            LuoKentta();
            LisaaNappaimet();
            LisaaLaskuri();

            Camera.Follow(hahmo);
            Camera.ZoomFactor = 1.2;
            Camera.StayInLevel = true;

            MasterVolume = 0.5;
        }
    }
}