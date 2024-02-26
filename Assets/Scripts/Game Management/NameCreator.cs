using System.Collections.Generic;
using UnityEngine;

public class NameCreator : MonoBehaviour
{
    // City Names
    public List<string> cityPrefixes = new List<string> { "Shadow", "Raven", "Gloom", "Crypt", "Mist", "Dusk", "Elder", "Sable", "Whisper", "Abyss", "Cursed", "Forgotten", "Grim", "Haunted", "Nocturnal", "Obsidian", "Silent", "Twilight", "Veiled" };
    public List<string> citySuffixes = new List<string> { "burg", "haven", "shire", "stead", "gate", "borough", "hold", "keep", "vale", "Citadel", "Enclave", "Hollow", "Nexus", "Sanctum", "Temple", "Vault", "Wraith", "Zenith" };

    // Religion Names
    public List<string> religionPrefixes = new List<string> { "Celestial", "Harmony", "Eternal", "Divine", "Sacred", "Transcendent", "Mystic", "Enlightened", "Serenity", "Astral", "Ethereal", "Cosmic", "Radiant", "Zen", "Seraphic", "Oracle", "Reverent", "Immutable" };
    public List<string> religionSuffixes = new List<string> { "Order", "Path", "Faith", "Doctrine", "Way", "Tradition", "Guidance", "Doctrine", "Teaching", "Covenant", "Essence", "Illumination", "Luminescence", "Oracle", "Unity", "Fellowship", "Sanctity", "Harmony" };

    // Unit Names
    public List<string> maleFirstNames = new List<string> { "Arnulf", "Berengar", "Cuthbert", "Dunstan", "Ealdred", "Freeman", "Godfrey", "Harold", "Ingemar", "Jocelyn", "Kendrick", "Ludolf", "Maelwine", "Njal", "Osmund" };
    public List<string> femaleFirstNames = new List<string> { "Aelfgifu", "Brynhild", "Cwenburh", "Dyveke", "Eadgyth", "Freydis", "Gisela", "Hildegarde", "Isolde", "Jorunn", "Kveldulfr", "Lilith", "Matilda", "Nanna", "Ottora" };
    public List<string> lastNames = new List<string> { "Aelfricsson", "Beornwulfsson", "Cynbelsson", "Dagsson", "Eorlsson", "Fridhleifsson", "Gunnarsson", "Helmstan", "Ivarsson", "Jarlsson", "Ketilsson", "Leifsson", "Magnusson", "Njallson", "Ormrsson" };

    //Empire Names
    public List<string> empirePrefixes = new List<string> { "Holy", "King's", "Eastern", "Western", "Northern", "Southern", "Ancient", "Duchy of", "County of", "Imperial", "Grand", "Free", "Golden", "Silver", "Iron", "Oceanic", "Celestial" };
    public List<string> dynastyNames = new List<string> { "von Hohenstein", "de Medici", "al-Farsi", "Plantagenet", "Habsburg", "Palaiologos", "Lancaster", "York", "Sforza", "Ming", "Tokugawa", "Timurid", "Borgia", "Song", "Rashid", "Seljuk", "Bourbon", "Romanov", "Umayyad", "Abbasid" };
    public List<string> empireSuffixes = new List<string> { "Empire", "Kingdom", "Principality", "Dominion", "Caliphate", "Sultanate", "Grand Duchy", "County", "Union", "Commonwealth", "Federation", "Republic", "Collective", "Territory", "State", "Expanse" };

    public string GenerateCityName()
    {
        string prefix = GetRandomElement(cityPrefixes);
        string suffix = GetRandomElement(citySuffixes);

        return prefix + suffix;
    }

    public string GenerateReligionName()
    {
        string prefix = GetRandomElement(religionPrefixes);
        string suffix = GetRandomElement(religionSuffixes);

        return prefix + " " + suffix;
    }

    public string GeneratePersonName(bool isMale)
    {
        string firstName = isMale ? GetRandomElement(maleFirstNames) : GetRandomElement(femaleFirstNames);
        string lastName = GetRandomElement(lastNames);

        return firstName + " " + lastName;
    }

    public string GenerateEmpireName()
    {
        string prefix = GetRandomElement(empirePrefixes);
        string dynastyName = GetRandomElement(dynastyNames);
        string suffix = GetRandomElement(empireSuffixes);

        return prefix + " " + dynastyName + " " + suffix;
    }

    private string GetRandomElement(List<string> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }
}
