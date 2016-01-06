namespace Hfr.Utilities
{
    public class HFRCats
    {
        // TODO : FIX THIS, IT SHOULD BE DYNAMIC !!!
        public static string PlainNameFromId(int id)
        {
            if (id == 1) { return ("Hardware"); }
            if (id == 16) { return ("Hardware & Périphériques"); }
            if (id == 15) { return ("Ordinateurs portables"); }
            if (id == 23) { return ("Technologies Mobiles"); }
            if (id == 2) { return ("Overclocking, Cooling & Tuning"); }
            if (id == 25) { return ("Apple      "); }
            if (id == 3) { return ("Vidéo & Son"); }
            if (id == 14) { return ("Photo Numérique"); }
            if (id == 5) { return ("Jeux-vidéo"); }
            if (id == 4) { return ("Windows & Software"); }
            if (id == 22) { return ("Réseaux grand public/SoHo"); }
            if (id == 21) { return ("Systèmes & Réseaux Pro"); }
            if (id == 11) { return ("OS Alternatifs"); }
            if (id == 10) { return ("Programmation"); }
            if (id == 12) { return ("Graphisme"); }
            if (id == 6) { return ("Achats & Ventes"); }
            if (id == 8) { return ("Emploi & Etudes"); }
            if (id == 9) { return ("Seti et projets distribués"); }
            if (id == 13) { return ("Discussions"); }
            if (id == 30) { return ("Electronique, domotique, DIY"); }
            return id == 0 ? ("Section réservée") : ("Erreur");
        }

        public static string IdFromPlainName(string name)
        {
            if (name == "Hardware") return ("1");
            if (name == "Hardware & Périphériques") return ("16");
            if (name == "Ordinateurs portables") return ("15");
            if (name == "Technologies Mobiles") return ("23");
            if (name == "Overclocking, Cooling & Tuning") return ("2");
            if (name == "Apple") return ("25");
            if (name == "Vidéo & Son") return ("3");
            if (name == "Photo Numérique") return ("14");
            if (name == "Jeux-vidéo") return ("5");
            if (name == "Windows & Software") return ("4");
            if (name == "Réseaux grand public/SoHo") return ("22");
            if (name == "Systèmes & Réseaux Pro") return ("21");
            if (name == "OS Alternatifs") return ("11");
            if (name == "Programmation") return ("10");
            if (name == "Graphisme") return ("12");
            if (name == "Achats & Ventes") return ("6");
            if (name == "Emploi & Etudes") return ("8");
            if (name == "Seti et projets distribués") return ("9");
            if (name == "Discussions") return ("13");
            return name == "Section réservée" ? ("0") : ("13");
        }

        public static string ShortNameFromId(int id)
        {
            if (id == 1) { return ("Hardware"); }
            if (id == 16) { return ("HardwarePeripheriques"); }
            if (id == 15) { return ("OrdinateursPortables"); }
            if (id == 23) { return ("gsmgpspda"); }
            if (id == 2) { return ("OverclockingCoolingTuning"); }
            if (id == 25) { return ("apple"); }
            if (id == 3) { return ("VideoSon"); }
            if (id == 14) { return ("Photonumerique"); }
            if (id == 5) { return ("JeuxVideo"); }
            if (id == 4) { return ("WindowsSoftware"); }
            if (id == 22) { return ("reseauxpersosoho"); }
            if (id == 21) { return ("systemereseauxpro"); }
            if (id == 11) { return ("OSAlternatifs"); }
            if (id == 10) { return ("Programmation"); }
            if (id == 12) { return ("Graphisme"); }
            if (id == 6) { return ("AchatsVentes"); }
            if (id == 8) { return ("EmploiEtudes"); }
            if (id == 9) { return ("Setietprojetsdistribues"); }
            return ("Discussions");
        }

        public static int GetHFRIndexFromId(int id)
        {
            switch (id)
            {
                case 1:
                    return 0;
                case 16:
                    return 1;
                case 15:
                    return 2;
                case 23:
                    return 3;
                case 2:
                    return 4;
                case 25:
                    return 5;
                case 3:
                    return 6;
                case 14:
                    return 7;
                case 5:
                    return 8;
                case 4:
                    return 9;
                case 22:
                    return 10;
                case 21:
                    return 11;
                case 11:
                    return 12;
                case 10:
                    return 13;
                case 12:
                    return 14;
                case 6:
                    return 15;
                case 8:
                    return 16;
                case 9:
                    return 17;
                case 13:
                    return 18;
                default:
                    return 0;
            }
        }
        public static string PlainNameFromIndex(int id)
        {
            if (id == 0) { return ("Hardware"); }
            if (id == 1) { return ("Hardware & Périphériques"); }
            if (id == 2) { return ("Ordinateurs portables"); }
            if (id == 3) { return ("Technologies Mobiles"); }
            if (id == 4) { return ("Overclocking, Cooling & Tuning"); }
            if (id == 5) { return ("Apple      "); }
            if (id == 6) { return ("Vidéo & Son"); }
            if (id == 7) { return ("Photo Numérique"); }
            if (id == 8) { return ("Jeux-vidéo"); }
            if (id == 9) { return ("Windows & Software"); }
            if (id == 10) { return ("Réseaux grand public/SoHo"); }
            if (id == 11) { return ("Systèmes & Réseaux Pro"); }
            if (id == 12) { return ("OS Alternatifs"); }
            if (id == 13) { return ("Programmation"); }
            if (id == 14) { return ("Graphisme"); }
            if (id == 15) { return ("Achats & Ventes"); }
            if (id == 16) { return ("Emploi & Etudes"); }
            if (id == 17) { return ("Seti et projets distribués"); }
            if (id == 18) { return ("Discussions"); }
            return id == 0 ? ("Section réservée") : ("Erreur");
        }
    }
}
