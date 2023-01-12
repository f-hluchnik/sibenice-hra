using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Šibenice_2._3
{
    abstract class Support
    {
        protected static int pocet_radku, nevyskyt = 0, obtiznost, g; //Obtížnost je určena počtem vykreslených obrázků. {3, 5, 8}
        protected static string slovo, Galerie;
        protected static char pismeno;
        protected static bool kontrola_vstupu;
    }


    class Program : Support
    {
        private static string hrat_znovu = "ano";

        private static void Nulovani()
        {
            Console.Clear();
            obtiznost = 0;
            nevyskyt = 0;
            pocet_radku = 0;
            g = 0;
            pismeno = '\0';
            Uhodnute uhodnute_pismeno = new Uhodnute(0);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void Main(string[] args)
        {
            Console.Title = "Šibenice";

            while (hrat_znovu == "ano")
            {
                Nulovani(); // Nastavení počátečních hodnot.
                Obtiznost(); //Volba obtížnosti
                GetWord nahodne_slovo = new GetWord(); //Výběr náhodného slova ze souboru.
                nahodne_slovo.GettingWord();
                Game hra = new Game(); //Cyklus hry
                hra.CyklusHry();
                Ukonceni(); //Trolling na konec hry.
            }
        }
        private static void Obtiznost() //Volba obtížnosti hry.
        {
            string vstup;
            for (int i = 0; i != 1 && i != 2 && i != 3;)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Vítej u hry 'Šibenice'! Nejprve si zvol úroveň hry. Stiskni 1 pro lehkou, 2 pro střední a 3 pro těžkou obtížnost.");
                Console.WriteLine("Jo, a bylo by fajn, kdyby sis zapnul zvuk. ;-)");
                Console.WriteLine("A taky bys možná měl vědět, že hrajeme bez hacku a carek, protože se čtením diakritiky mám ještě pořád problémy, promiň. :-(");
                Console.Write("Obtížnost: ");
                // vstup = Console.ReadLine();
                vstup = "1";
                bool result = Int32.TryParse(vstup, out obtiznost); //Kontrola vstupu obtížnosti.
                i = obtiznost;
                if (result)
                {
                    if (obtiznost != 1 && obtiznost != 2 && obtiznost != 3)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Takovou obtížnost neznám. :-( Zkus to znovu! Pokračuj stiskem libovolné klávesy.");
                        // Console.Beep(1000, 1000);
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Neumím přečíst obtížnost: '{0}'. Pokračuj stiskem libovolné klávesy.", vstup);
                    // Console.Beep(1000, 1000);
                    Console.ReadKey();
                }
            }

            switch (obtiznost)
            {
                case 1:
                    obtiznost = 8;
                    break;
                case 2:
                    obtiznost = 5;
                    break;
                case 3:
                    obtiznost = 3;
                    break;
            }
        }
        private static string Ukonceni()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Konec hry, chcete hrát znovu? (ano/ne) :-) ");
            Skoncit();
            while (hrat_znovu == "ne" && hrat_znovu != "ano")
            {
                Console.WriteLine("Opravdu chceš skončit?");
                Skoncit();
                switch (hrat_znovu)
                {
                    case "ano":
                        Console.WriteLine("Co třeba zkusit ještě jedno slovíčko? :-)");
                        Skoncit();
                        switch (hrat_znovu)
                        {
                            case "ne":
                                Console.WriteLine("To mě mrzí. Brzy se vrať! Stiskni prosím ještě jednu klávesu na rozloučenou...");
                                Konec();
                                break;
                            case "ano":
                                Console.WriteLine("Správné rozhodnutí! Stiskni klávesu a jdeme na to!");
                                Console.ReadKey();
                                hrat_znovu = "ano";
                                break;
                        }
                        break;
                    case "ne":
                        Console.WriteLine("Správné rozhodnutí! Stiskni klávesu a jdeme na to!");
                        Console.ReadLine();
                        hrat_znovu = "ano";
                        break;
                }
                break;
            }

            void Skoncit() //Ověření vstupu - zacyklení, pokud neodpovídá.
            {
                hrat_znovu = "No počkej";
                while (hrat_znovu != "ano" && hrat_znovu != "ne")
                {
                    hrat_znovu = Console.ReadLine();
                    if (hrat_znovu != "ne" && hrat_znovu != "ano")
                    {
                        Console.WriteLine("Promiň, ale tomuhle nerozumím, zkus to prosím znovu.");
                    }
                }
            }

            void Konec() //Trolling na konec.
            {
                string Trolls = Path.GetFullPath("Texty/Trolls.txt");
                bool a = true;
                ConsoleKeyInfo cki;
                string key = "\0";
                while (a != false)
                {
                    Random cislo = new Random(); //Vytvoření proměnné pro náhodné číslo.
                    int nahodne_cislo = cislo.Next(1, 7); //Generování náhodného čísla.
                    StreamReader ReadLines = new StreamReader(Trolls);
                    string troll = File.ReadLines(Trolls).Skip(nahodne_cislo).Take(1).First(); //Načtení slova na náhodném řádku.
                    cki = Console.ReadKey();
                    if (cki.Key.ToString() == key)
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("       ");
                        Console.WriteLine(" No dobře, no...");
                        // Console.Beep(1000, 1500);
                        break;
                    }
                    else
                    {
                        key = cki.Key.ToString();
                        Console.WriteLine(" {0}", troll);
                        // Console.Beep(1000, 300); Console.Beep(1259, 300);
                    }
                }
            }
            return hrat_znovu;
        }
    }

    class GetWord : Support //Načtení slova ze souboru.
    {
        private string Slovnik;

        public string GettingWord()
        {
            Random cislo = new Random(); //Vytvoření proměnné pro náhodné číslo.
            Radky();
            int nahodne_cislo = cislo.Next(1, pocet_radku); //Generování náhodného čísla.
            StreamReader ReadLines = new StreamReader(Slovnik);
            slovo = File.ReadLines(Slovnik).Skip(nahodne_cislo).Take(1).First(); //Načtení slova na náhodném řádku.
            return slovo;
        }
        private int Radky() //Zjišťování počtu řádků zdrojového souboru.
        {
            Slovnik = Path.GetFullPath("Texty/Slovnik.txt");
            Existuje();
            using (var slovnik = new System.IO.StreamReader(Slovnik))
            {
                while ((Slovnik = slovnik.ReadLine()) != null)
                    pocet_radku++;
            }
            Slovnik = Path.GetFullPath("Texty/Slovnik.txt");
            return pocet_radku;
        }
        private void Existuje()
        {
            Slovnik = Path.GetFullPath("Texty/Slovnik.txt");
            if (File.Exists(Slovnik))
                Slovnik = Path.GetFullPath("Texty/Slovnik.txt");
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine();
                Console.WriteLine("Je mi líto, ale neumím načíst slovník.");
                // Console.Beep(1000, 1000);
                Console.ReadLine();
                Environment.Exit(-1);
            }
        }
    }

    class Game : Support
    {
        private char[] demaska = new char[slovo.Length];
        private bool prazdne_misto; //Ještě neuhodnuté písmeno.

        public void CyklusHry() //Hlavní cyklus hry.
        {
            Zaplneni(); //Zaplněění pole demaska.
            Picture obesenec = new Picture();
            while (PrazdneMisto() == true) //Cyklus se bud opakovat, dokud bude v poli demaska prázsné místo.
            {
                PrvniCast(); //Vypsání úvodu.
                if (nevyskyt == (obtiznost)) //Jestliže hráč udělal maximální počet chyb, ukončí se cyklus hry.
                {
                    PrvniCast();
                    kontrola_vstupu = true;
                    obesenec.Obrazek(); //Načtení obrázku.
                    // Console.Beep(3000, 500); Console.Beep(2831, 500); Console.Beep(2672, 500); Console.Beep(2522, 1000);
                    break;
                }
                kontrola_vstupu = true; //Tato proměnná zajistí, že se obrázek se bude vykreslovat pouze při uhodnutí chybného písmena.
                obesenec.Obrazek(); //Načtení obrázku.
                Uhodnute uhodnute_pismeno = new Uhodnute(1);
                GetLetter(); //Uživatelský vstup - písmeno.
                Overeni(); //Ověření, zda se písmeno nachází ve slově.
                obesenec.Obrazek(); //Načtení obrázku.
                Console.WriteLine();
                if (PrazdneMisto() == false) //Jestliže hráč uhodl celé slovo, ukončí se cyklus hry.
                    break;
            }

            if (PrazdneMisto() == false) //Jestliže hráč uhodl slovo, vypíše se výherní hláška.
            {
                PrvniCast();
                kontrola_vstupu = true;
                obesenec.Obrazek();
                Uhodnute uhodnute_pismeno = new Uhodnute(1);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Vyhráli jste!");
                // Console.Beep(1500, 500); Console.Beep(1890, 500); Console.Beep(2247, 500); Console.Beep(3000, 800);
            }
        }
        private bool PrazdneMisto() //Ověření, zda v poli je prázdné místo.
        {
            int a = 0;
            int b = 0;
            while (a < slovo.Length)
            {
                if (demaska[a] == char.Parse("_"))
                {
                    b++;
                }
                a++;
            }
            if (b > 0)
                prazdne_misto = true;
            else
                prazdne_misto = false;
            return prazdne_misto;
        }
        private void Zaplneni() //Zaplnění pole demaska.
        {
            for (int b = 0; b < slovo.Length; b++)
            {
                if (slovo[b] == char.Parse(" "))
                {
                    demaska[b] = char.Parse(" ");
                }
                else
                    demaska[b] = char.Parse("_");

            }
        }
        private char GetLetter() //Uživatelský vstup - písmeno.
        {
            bool overeni_pismena = false; //Tato proměnná zajistí běh cyklu vkládání znaku, dokud nebude vloženo písmeno.
            while (kontrola_vstupu == true)
            {
                Console.WriteLine();
                Console.WriteLine();
                try
                {
                    while (overeni_pismena == false)
                    {
                        Console.Write("Vložte písmeno a potvrďte klávesou Enter: ");
                        pismeno = char.Parse(Console.ReadLine());
                        overeni_pismena = Char.IsLetter(pismeno);
                        if (overeni_pismena == false)
                        {
                            kontrola_vstupu = true;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Nezadali jste písmeno, zkuste to znovu. Pokračujte stiskem libovolné klávesy.");
                            // Console.Beep(1000, 1000);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadKey();
                            PrvniCast();
                        }
                        else
                        {
                            kontrola_vstupu = false;
                            break;
                        }
                    }
                }
                catch (Exception) //Zachycení výjimky při zadání více znaků.
                {
                    kontrola_vstupu = true;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Vložte prosím jen jeden znak! Pokračujte stisknutím libovolné klávesy.");
                    // Console.Beep(1000, 1000);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    PrvniCast();
                }
            }
            return pismeno;
        }
        private void PrvniCast() //Metoda vypisující opakující se první část programu.
        {
            Console.Clear();
            Console.WriteLine("Hraješ šibenici, postupně hádej písmena.");
            Console.WriteLine("Připomínám, že hrajeme bez hacku a carek. Mrzí mě to, ale věřím, že to zvládneš!");
            Console.WriteLine();
            Vypsani();
            Console.WriteLine();
            Console.WriteLine();
        }
        private void Overeni() //Ověření, zda je uživatelem zadané písmeno v hádaném slově.
        {
            int c = 0;
            while (c < slovo.Length)
            {
                if (pismeno == slovo[c])
                {
                    demaska[c] = pismeno;
                }
                else
                {
                    if (demaska[c] == char.Parse("_"))
                    {
                        demaska[c] = char.Parse("_");
                    }
                }
                c++;
            }
        }
        private void Vypsani() //Vypsání pole demasky.
        {
            Console.Write("                    ");
            for (int d = 0; d < slovo.Length; d++) //Vypsání pole demasky.
            {
                Console.Write("{0}", demaska[d]);
                Console.Write(" ");
            }
        }
    }

    class Picture : Support
    {
        public void Obrazek()
        {
            ChybnePismeno();
            switch (obtiznost) //Volba obrázků podle obtížnosti, načtení cesty k určitému obrázku.
            {
                case 8:
                    switch (nevyskyt)
                    {
                        case 0:
                            Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 1:
                            Galerie = Path.GetFullPath("Texty/Obrazek1.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek1.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 2:
                            Galerie = Path.GetFullPath("Texty/Obrazek2.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek2.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 3:
                            Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 4:
                            Galerie = Path.GetFullPath("Texty/Obrazek4.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek4.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 5:
                            Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 6:
                            Galerie = Path.GetFullPath("Texty/Obrazek6.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek6.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 7:
                            Galerie = Path.GetFullPath("Texty/Obrazek7.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek7.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 8:
                            Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        default:
                            Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                    }
                    break;
                case 5:
                    switch (nevyskyt)
                    {
                        case 0:
                            Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 1:
                            Galerie = Path.GetFullPath("Texty/Obrazek1.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek1.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 2:
                            Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 3:
                            Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 4:
                            Galerie = Path.GetFullPath("Texty/Obrazek6.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek6.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 5:
                            Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        default:
                            Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (nevyskyt)
                    {
                        case 0:
                            Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek0.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 1:
                            Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek3.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 2:
                            Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek5.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        case 3:
                            Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Obrazek8.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                        default:
                            Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            if (File.Exists(Galerie))
                                Galerie = Path.GetFullPath("Texty/Chyba.txt");
                            else
                            {
                                ChybovaHlaska();
                            }
                            break;
                    }
                    break;
            }

            using (var obraz = new System.IO.StreamReader(Galerie)) //Začátek načítání obrázku.
            {
                switch (obtiznost) //Ukončení hry a prozrazení slova podle obtížnosti.
                {
                    case 8:
                        if (nevyskyt == 8)
                        {
                            Prohra();
                        }
                        break;
                    case 5:
                        if (nevyskyt == 5)
                        {
                            Prohra();
                        }
                        break;
                    case 3:
                        if (nevyskyt == 3)
                        {
                            Prohra();
                        }
                        break;
                }

                void Prohra() //Vypsání proherní hlášky a hádaného slova.
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    while ((Galerie = obraz.ReadLine()) != null)
                        Console.WriteLine(Galerie);
                    Console.ForegroundColor = ConsoleColor.White;
                    Uhodnute uhodnute_pismeno = new Uhodnute(1);
                    Console.WriteLine();
                    Console.Write("Prohrál jsi, člověče! Měl jsi uhodnout slovo ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(slovo);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("!!!");
                }

                switch (obtiznost) //Volba barvy obrázku
                {
                    case 8:
                        switch (nevyskyt)
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 6:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 7:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 8:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        break;
                    case 5:
                        switch (nevyskyt)
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        break;
                    case 3:
                        switch (nevyskyt)
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        break;
                }
                while ((Galerie = obraz.ReadLine()) != null) //Načtení obrázku.
                    Console.WriteLine(Galerie);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }
        private void ChybovaHlaska() //Načtení chybové hlášky v případě chybějícího obrázku.
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine();
            Console.WriteLine("Je mi líto, ale neumím načíst obrázkový soubor.");
            // Console.Beep(1000, 1000);
            Console.ReadLine();
            Environment.Exit(-1);
        }
        public int ChybnePismeno()
        {
            string pismeno_string = Char.ToString(pismeno);
            if (kontrola_vstupu == false && slovo.Contains(pismeno_string) == false) //Pokud slovo neobsahuje zadané písmeno, přehraje se zvuk a inkrementuje se proměnná označující počet chybných znaků.
            {
                // Console.Beep(1000, 500);
                nevyskyt++;
            }
            return nevyskyt;
        }
    }

    class Uhodnute : Support
    {
        protected static char[] hadana_pismena = new char[26];

        public Uhodnute(int chyba)
        {
            int h = 0;
            if (chyba == 0)
            {
                for (h = 0; h < hadana_pismena.Length; h++) //Vynulování pole chybna_pismena.
                {
                    hadana_pismena[h] = '\0';
                }
            }
            if (chyba == 1 && kontrola_vstupu == true) //Jestliže uhodnutý znak je písmeno, zařadí se toto písmeno do pole hádaných písmen.
            {
                hadana_pismena[g] = pismeno;
                g++;
                Console.WriteLine();
                Console.WriteLine("Již jste hádali: ");
                for (h = 0; h < hadana_pismena.Length; h++) //Vypsání pole chybna_pismena.
                {
                    Console.Write("{0}", hadana_pismena[h]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}