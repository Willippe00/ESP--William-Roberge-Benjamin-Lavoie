using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BanqueObjet : MonoBehaviour
{
    #region BanqueObjet

    // familles: "Inventaire", "Consommables", "Outils & Armes", "Ressources", "Stations"
    // usages: "Aucun", "Consommable", "Outil","Arme", "Arc", "Placer"
    // effets: "Faim", "Soif", "Vie", "Autres"




    public List<Objet> banqueObjet = new List<Objet>()
    {
        //Outils et armes
        new Objet("Pioche_1","Outils & Armes","Outil",1,10,0f), //0
        new Objet("Pioche_2","Outils & Armes","Outil",1,15,0.2f),
        new Objet("Pioche_3","Outils & Armes","Outil",1,25,0.4f),
        new Objet("Hache_1","Outils & Armes","Outil",1,10,0f), 
        new Objet("Hache_2","Outils & Armes","Outil",1,15,0.2f), 
        new Objet("Hache_3","Outils & Armes","Outil",1,25,0.4f), //5
        new Objet("Épée_1","Outils & Armes","Arme",1,10,0f,2f, 90),
        new Objet("Épée_2","Outils & Armes","Arme",1,15,0.2f,2.2f, 90),
        new Objet("Épée_3","Outils & Armes","Arme",1,25,0.4f,2.4f, 90),
        new Objet("Arc_1","Outils & Armes","Arc",1,10), 
        new Objet("Arc_2","Outils & Armes","Arc",1,15),//10
        new Objet("Arc_3","Outils & Armes","Arc",1,25),
        new Objet("Flèche","Outils & Armes","Aucun",1),
        new Objet("Arbalète","Outils & Armes","Arc",1,30),
        new Objet("Bouteille_Vide","Outils & Armes","Outil",1,0,0),

        //Consommables
        new Objet("Baie", "Consommables", "Consommable",1,"Faim",2), //15
        new Objet("Carotte", "Consommables", "Consommable",1,"Faim",8),
        new Objet("Laitue", "Consommables", "Consommable",1,"Faim",8),
        new Objet("Ognion", "Consommables", "Consommable",1,"Faim",8),
        new Objet("Cerise", "Consommables", "Consommable",1,"Faim",8),
        new Objet("Pomme", "Consommables", "Consommable",1,"Faim",8),//20
        new Objet("Noix", "Consommables", "Consommable",1,"Faim",4),
        new Objet("Raisin", "Consommables", "Consommable",1,"Faim",8),
        new Objet("Champignon_1", "Consommables", "Consommable",1,"Faim",5),
        new Objet("Champignon_2", "Consommables", "Consommable",1,"Vie",-20), 
        new Objet("Champignon_3", "Consommables", "Consommable",1,"Faim",15),//25
        new Objet("Pain", "Consommables", "Consommable",1,"Faim",15),
        new Objet("Viande_crue", "Consommables", "Consommable",1,"Faim",-10),
        new Objet("Viande_cuite", "Consommables", "Consommable",1,"Faim", 20),
        new Objet("Saucisse", "Consommables", "Consommable",1,"Faim", 25), 
        new Objet("Poisson_1", "Consommables", "Consommable",1,"Faim", 10), //30
        new Objet("Poisson_2", "Consommables", "Consommable",1,"Faim", 15),
        new Objet("Poisson_3", "Consommables", "Consommable",1,"Faim", 20),
        new Objet("Bouteille_Eau", "Consommables", "Consommable",1,"Soif",10),
        new Objet("PotionVie_1", "Consommables", "Consommable",1,"Vie",15), 
        new Objet("PotionVie_2", "Consommables", "Consommable",1,"Vie",30),//35
        new Objet("PotionVie_3", "Consommables", "Consommable",1,"Vie",60),

        //Ressources_Mine
        new Objet("Roche","Ressources","Aucun",1),
        new Objet("Cuivre","Ressources","Aucun",1),
        new Objet("Argent","Ressources","Aucun",1), 
        new Objet("Or","Ressources","Aucun",1),//40
        new Objet("Diamant","Ressources","Aucun",1),
        new Objet("Émeraude","Ressources","Aucun",1),
        new Objet("Rubis","Ressources","Aucun",1),
        new Objet("Platine","Ressources","Aucun",1), 
        new Objet("Lingot_Cuivre","Ressources","Aucun",1),//45
        new Objet("Lingot_Argent","Ressources","Aucun",1),
        new Objet("Lingot_Or","Ressources","Aucun",1),
        new Objet("Lingot_Platine","Ressources","Aucun",1),

        //Ressources_Nature
        new Objet("Bois","Ressources","Aucun",1), 
        new Objet("Branche","Ressources","Aucun",1),//50
        new Objet("Feuille","Ressources","Aucun",1),
        new Objet("Pousse","Ressources","Aucun",1),
        new Objet("Racine","Ressources","Aucun",1),
        new Objet("Herbes","Ressources","Aucun",1), 
        new Objet("Fleur_1","Ressources","Aucun",1),//55
        new Objet("Fleur_2","Ressources","Aucun",1),
        new Objet("Fleur_3","Ressources","Aucun",1),
        new Objet("Fleur_4","Ressources","Aucun",1),
        new Objet("Fleur_5","Ressources","Aucun",1), 
        new Objet("Fleur_6","Ressources","Aucun",1),//60
        new Objet("Fleur_7","Ressources","Aucun",1),
        new Objet("Fleur_8","Ressources","Aucun",1),
        new Objet("Fleur_9","Ressources","Aucun",1),
        new Objet("Sable","Ressources","Aucun",1), 
        new Objet("Coquillage","Ressources","Aucun",1),//65

        //Ressources_Artisanales
        new Objet("Sucre","Ressources","Aucun",1),
        new Objet("Corde","Ressources","Aucun",1),
        new Objet("Papier","Ressources","Aucun",1),
        new Objet("Soie","Ressources","Aucun",1), 
        new Objet("Planche","Ressources","Aucun",1),//70
        new Objet("Pack_bois","Ressources","Aucun",1),

        //Ressources_Animales
        new Objet("Laine","Ressources","Aucun",1),
        new Objet("Plume","Ressources","Aucun",1),
        new Objet("Oeuf","Ressources","Aucun",1), 
        new Objet("Fourrure","Ressources","Aucun",1),//75
        new Objet("Peau","Ressources","Aucun",1),
        new Objet("Corne","Ressources","Aucun",1),

        new Objet("Verre","Ressources","Aucun",1),

        //Stations
        new Objet("CraftTable_1","Stations","Placer",1), 
        new Objet("CraftTable_2","Stations","Placer",1),//80
        new Objet("Puit","Stations","Placer",1),
        new Objet("Barriere","Stations","Placer",1),
        new Objet("FeuCamp","Stations","Placer",1),
        new Objet("Bateau","Stations","Placer",1),
        new Objet("Coffre_1","Stations","Placer",1),//85
        new Objet("Coffre_2","Stations","Placer",1),
        new Objet("Lanterne","Stations","Placer",1),
        new Objet("Torche","Stations","Placer",1),

        new Objet("Lance_1","Outils & Armes","Arme",1,10,0f,2f, 15),
        new Objet("Lance_2","Outils & Armes","Arme",1,15,0.2f,2.5f, 15), //90
        new Objet("Lance_3","Outils & Armes","Arme",1,25,0.4f,3f, 15),

        new Objet("Boussole","Ressources","Aucun",1)



    };

    Sprite[] banqueSprites1;
    Sprite[] banqueSprites3;


    void ChargerBanqueObjet()
    {
        for (int i = 0; i < banqueObjet.Count; i++)
        {
            banqueObjet[i].id = i;
        }

        ChargerBanqueSprite("ItemsSprites", ref banqueSprites1);
        ChargerBanqueSprite("ItemsSprites3", ref banqueSprites3);


        foreach (Objet objet in banqueObjet)
        {
            objet.image = Array.Find(banqueSprites1, x => x.name == objet.nom); //Trouve le sprite qui a le meme nom que l'objet
            if (objet.image == null) objet.image = Resources.Load<Sprite>("Objets/" + objet.nom);
            if (objet.image == null) objet.image = Array.Find(banqueSprites3, x => x.name == objet.nom); //Trouve le sprite qui a le meme nom que l'objet
        }
    }


    void ChargerBanqueSprite(string nomBanque, ref Sprite[] banque)
    {
        banque = Resources.LoadAll<Sprite>("Objets/" + nomBanque);
    }

    #endregion

    #region Banque_Craft

    public Dictionary<string, Objet> banqueCraft = new Dictionary<string, Objet>();

    void ChargerBanqueCraft()
    {
        banqueCraft.Add("73_50_37_", banqueObjet[12]); //Flèche
        banqueCraft.Add("54_54_54_", banqueObjet[67]); //Corde
        banqueCraft.Add("78_78_", banqueObjet[14]); //Bouteille vide
        banqueCraft.Add("55_23_14_", banqueObjet[34]); //BouteilleVie_1
        banqueCraft.Add("56_23_14_", banqueObjet[34]); //BouteilleVie_1
        banqueCraft.Add("57_23_14_", banqueObjet[34]); //BouteilleVie_1
        banqueCraft.Add("58_23_14_", banqueObjet[34]); //BouteilleVie_1
        banqueCraft.Add("27_49_49_", banqueObjet[28]); //Viande_cuite
        banqueCraft.Add("51_51_33_", banqueObjet[68]); //Papier
        banqueCraft.Add("72_72_72_", banqueObjet[79]); //Soie
        banqueCraft.Add("49_49_49_", banqueObjet[71]); //Pack_Bois
        banqueCraft.Add("38_38_38_", banqueObjet[45]); //Lingot_Cuivre
        banqueCraft.Add("39_39_39_", banqueObjet[46]); //Lingot_Argent
        banqueCraft.Add("40_40_40_", banqueObjet[47]); //Lingot_Or
        banqueCraft.Add("38_68_49_", banqueObjet[88]); //Torche
        banqueCraft.Add("64_64_49_", banqueObjet[78]); //Verre
        banqueCraft.Add("43_78_46_", banqueObjet[92]); //Boussole
        banqueCraft.Add("68_76_71_", banqueObjet[79]); //CraftTable_1


        banqueCraft.Add("37_37_49_67_48_", banqueObjet[0]); //Pioche_1
        banqueCraft.Add("39_39_49_67_48_", banqueObjet[1]); //Pioche_2
        banqueCraft.Add("40_40_49_67_48_76_", banqueObjet[2]); //Pioche_3
        banqueCraft.Add("37_37_49_37_48_", banqueObjet[3]); //Hache_1
        banqueCraft.Add("39_39_49_39_48_", banqueObjet[4]); //Hache_2
        banqueCraft.Add("40_40_49_40_48_76_", banqueObjet[5]); //Hache_3
        banqueCraft.Add("38_38_67_49_67_", banqueObjet[6]); //Épée_1
        banqueCraft.Add("39_39_67_49_67_", banqueObjet[7]); //Épée_2
        banqueCraft.Add("40_40_67_49_67_", banqueObjet[8]); //Épée_3
        banqueCraft.Add("77_45_49_67_49_65", banqueObjet[89]); //Lance_1
        banqueCraft.Add("77_46_49_67_49_65", banqueObjet[90]); //Lance_2
        banqueCraft.Add("77_47_49_67_49_65", banqueObjet[91]); //Lance_3
        banqueCraft.Add("25_23_55_56_15_14_", banqueObjet[34]); //BouteilleVie_2
        banqueCraft.Add("67_67_45_76_49_49_", banqueObjet[85]); //Coffre_1
        banqueCraft.Add("39_39_71_71_71_71_", banqueObjet[82]); //Barriere
        banqueCraft.Add("47_67_88_78_46_", banqueObjet[87]); //Lanterne
        banqueCraft.Add("68_35_79_85_71_71_", banqueObjet[80]); //CraftTable_2


        banqueCraft.Add("68_88_88_68_51_50_50_51_52_71_71_52_", banqueObjet[83]); //FeuCamp
        banqueCraft.Add("71_67_67_71_40_43_75_40_71_49_49_71_", banqueObjet[86]); //Coffre_2
        banqueCraft.Add("55_56_57_58_59_60_61_62_33_53_23_24_", banqueObjet[36]); //PotionVie_3
        banqueCraft.Add("49_39_39_49_37_45_67_37_37_37_37_37_", banqueObjet[81]); //Puit
        banqueCraft.Add("41_69_69_36_87_69_48_92_71_71_71_86_", banqueObjet[84]); //Bateau



    }

    #endregion

    private void Awake()
    {
        ChargerBanqueObjet();
        ChargerBanqueCraft();
    }
}