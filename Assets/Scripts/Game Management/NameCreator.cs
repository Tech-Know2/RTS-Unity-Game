using System.Collections.Generic;
using UnityEngine;

public class NameCreator : MonoBehaviour
{
    // City Names
    private List<string> cityPrefixes = new List<string> { "Shadow", "Raven", "Gloom", "Crypt", "Mist", "Dusk", "Elder", "Sable", "Whisper", "Abyss", "Cursed", "Forgotten", "Grim", "Haunted", "Nocturnal", "Obsidian", "Silent", "Twilight", "Veiled" };
    private List<string> citySuffixes = new List<string> { "burg", "haven", "shire", "stead", "gate", "borough", "hold", "keep", "vale", "citadel", "enclave", "hollow", "nexus", "sanctum", "temple", "vault", "wraith", "zenith" };

    // Religion Names
    private List<string> religionPrefixes = new List<string> { "Celestial", "Harmony", "Eternal", "Divine", "Sacred", "Transcendent", "Mystic", "Enlightened", "Serenity", "Astral", "Ethereal", "Cosmic", "Radiant", "Zen", "Seraphic", "Oracle", "Reverent", "Immutable" };
    private List<string> religionSuffixes = new List<string> { "order", "path", "faith", "doctrine", "way", "tradition", "guidance", "teaching", "covenant", "essence", "illumination", "luminescence", "oracle", "unity", "fellowship", "sanctity", "harmony" };

    // Unit Names
    private List<string> maleFirstNames = new List<string> { "Arnulf", "Berengar", "Cuthbert", "Dunstan", "Ealdred", "Freeman", "Godfrey", "Harold", "Ingemar", "Jocelyn", "Kendrick", "Ludolf", "Maelwine", "Njal", "Osmund", "Paul", "Ender", "Andrew", "Leto", "Harry" };
    private List<string> femaleFirstNames = new List<string> { "Aelfgifu", "Brynhild", "Cwenburh", "Dyveke", "Eadgyth", "Freydis", "Gisela", "Hildegarde", "Isolde", "Jorunn", "Kveldulfr", "Lilith", "Matilda", "Nanna", "Ottora", "Alia", "Valentine" };
    private List<string> lastNames = new List<string> { "Aelfricsson", "Beornwulfsson", "Cynbelsson", "Dagsson", "Eorlsson", "Fridhleifsson", "Gunnarsson", "Helmstan", "Ivarsson", "Jarlsson", "Ketilsson", "Leifsson", "Magnusson", "Njallson", "Ormrsson", "Atredies", "Harkonnen", "Wiggin", "Corrino", "Fenring", "Potter", "Granger" };

    //Empire Names
    private List<string> empirePrefixes = new List<string> { "Holy", "King's", "Eastern", "Western", "Northern", "Southern", "Ancient", "Duchy of", "County of", "Imperial", "Grand", "Free", "Golden", "Silver", "Iron", "Oceanic", "Celestial" };
    private List<string> dynastyNames = new List<string> { "von Hohenstein", "de Medici", "al-Farsi", "plantagenet", "habsburg", "palaiologos", "lancaster", "york", "sforza", "ming", "tokugawa", "timurid", "borgia", "song", "rashid", "seljuk", "bourbon", "romanov", "umayyad", "abbasid" };
    private List<string> empireSuffixes = new List<string> { "Empire", "Kingdom", "Principality", "Dominion", "Caliphate", "Sultanate", "Grand Duchy", "County", "Union", "Commonwealth", "Federation", "Republic", "Collective", "Territory", "State", "Expanse" };

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
