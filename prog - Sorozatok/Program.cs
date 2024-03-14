namespace sorozatok
{
    class Program
    {


        class Sorozat
        {
            public string datum;
            public string cim;
            public string evad_resz;
            public int ido;
            public int megnezte;

            public Sorozat(string datum, string cim, string evad_resz, int ido, int megnezte)
            {
                this.datum = datum;
                this.cim = cim;
                this.evad_resz = evad_resz;
                this.ido = ido;
                this.megnezte = megnezte;
            }
        }
        public static string NOP(int ido)
        {
            string formatum = "";
            int napok = ido / (60 * 24);
            int orak = (ido - napok * (60 * 24)) / 60;
            int percek = ido - napok * (60 * 24) - orak * 60;
            formatum = $"{napok} napot {orak} órát és {percek} percet";
            return formatum;
        }
        public static bool Kiir(string date, string ep_date)
        {
            if (ep_date == "NI") return false;
            string[] date1 = date.Split('.');
            string[] date2 = ep_date.Split('.');
            int[] be_date = new int[3];
            be_date[0] = int.Parse(date1[0]);
            be_date[1] = int.Parse(date1[1]);
            be_date[2] = int.Parse(date1[2]);
            int[] v_date = new int[3];
            v_date[0] = int.Parse(date2[0]);
            v_date[1] = int.Parse(date2[1]);
            v_date[2] = int.Parse(date2[2]);
            if (be_date[0] >= v_date[0] && be_date[1] >= v_date[1] && be_date[2] >= v_date[2])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string Hetnapja(int ev, int ho, int nap)
        {
            string hetnapja = "";
            string[] napok = { "v", "h", "k", "sze", "cs", "p", "szo" };
            int[] honapok = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            if (ho < 3) ev--;
            hetnapja = napok[(ev + ev / 4 - ev / 100 + ev / 400 + honapok[ho - 1] + nap) % 7];
            return hetnapja;
        }
        public static void Main(string[] args)
        {
            string[] beolvasas = File.ReadAllLines("./lista-1.txt");
            int c = 0;
            string[] beolvasott = new string[5];
            List<Sorozat> sorozatok = new List<Sorozat>();

            foreach (var item in beolvasas)
            {
                beolvasott[c] = item;
                c++;
                if (c == 5)
                {
                    sorozatok.Add(new Sorozat(beolvasott[0], beolvasott[1], beolvasott[2], int.Parse(beolvasott[3]), int.Parse(beolvasott[4])));
                    c = 0;
                }
            }
            // megszámlálások
            int vetitett = 0;
            float latta = 0;
            int ossz_ido = 0;
            foreach (var item in sorozatok)
            {
                if (item.datum != "NI") vetitett++;
                if (item.megnezte == 1) { latta++; ossz_ido += item.ido; }

            }
            float arany = latta / sorozatok.Count * 100;
            Console.WriteLine($"2. feladat\nA listában {vetitett} db vetítési dátummal rendelkező epizód van.");
            Console.WriteLine($"3. feladat\nA listában lévő epizódok {Math.Round(arany, 2)}%-át látta.");
            Console.WriteLine($"4. feladat\nSorozatnézéssel {NOP(ossz_ido)} töltött.");
            Console.Write("5. feladat\nAdjon meg egy dátumot! Dátum= ");
            string datum = Console.ReadLine();

            foreach (var item in sorozatok)
            {

                if (Kiir(datum, item.datum) && item.megnezte == 0)
                {
                    Console.WriteLine($"{item.evad_resz}\t{item.cim}");
                }
            }
            Console.Write("7. feladat\nAdja meg a hét egy napját (például cs)! Nap= ");
            string a_nap = Console.ReadLine();
            int vetitettek = 0;
            int nap = 0;
            int ho = 0;
            int ev = 0;
            string[] knvert;
            bool nem_volt = true;
            HashSet<string> v_sor = new HashSet<string>();
            foreach (var item in sorozatok)
            {

                if (item.datum != "NI")
                {
                    knvert = item.datum.Split('.');
                    ev = int.Parse(knvert[0]);
                    ho = int.Parse(knvert[1]);
                    nap = int.Parse(knvert[2]);
                    if (Hetnapja(ev, ho, nap) == a_nap)
                    {
                        v_sor.Add(item.cim);
                        nem_volt = false;
                    }

                }
            }
            foreach (var item in v_sor)
            {
                Console.WriteLine(item);
            }
            if (nem_volt) Console.WriteLine("Az adott napon nem kerül adásba sorozat.");
            //8. Feladat
            Dictionary<string, int[]> szum = new Dictionary<string, int[]>();
            foreach (var item in sorozatok){
                if(szum.ContainsKey(item.cim)){
                    szum[item.cim][0]+=item.ido;
                    szum[item.cim][1]++;
                }else{
                    szum.Add(item.cim, new int[2]);
                    szum[item.cim][0]+=item.ido;
                    szum[item.cim][1]++;
                }
            }
            string txt_out = "";
            foreach (var item in szum)
            {
                txt_out += $"{item.Key} {item.Value[0]} {item.Value[1]}\n";
            }
            File.WriteAllText("./summa.txt", txt_out);
            Console.ReadKey();
        }

    }
}