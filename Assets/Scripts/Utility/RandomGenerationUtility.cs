using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class RandomGenerationUtility
{
    public static string GenerateRandomItemName()
    {
        string[] adjectives = { "Shiny", "Glowing", "Mysterious", "Enchanted", "Ancient", "Magical", "Radiant", "Ethereal", "Legendary" };
        string[] objects = { "Sword", "Ring", "Amulet", "Staff", "Potion", "Armor", "Wand", "Scroll", "Talisman" };
        string[] actions = { "Power", "Destruction", "Healing", "Wisdom", "Fortune", "Protection", "Knowledge", "Dominance", "Eternity" };

        Random random = new Random();
        string adjective = adjectives[random.Next(adjectives.Length)];
        string obj = objects[random.Next(objects.Length)];
        string action = actions[random.Next(actions.Length)];

        return $"{adjective} {obj} of {action}";
    }

    public static string GenerateItemDescription(Item item)
    {
        string description = $"This {item.ItemName} has the following abilities: ";
        foreach (Ability ability in item.Abilities)
        {
            string abilityDescription = ability.GetDescription();
            description += abilityDescription + ", ";
        }

        description = description.TrimEnd(',', ' ');

        return description + ".";
    }

    public static Ability GenerateRandomAbility()
    {
        string[] abilityTypes = { "DamageAbility", "HealAbility", "BuffAbility" };
        Elements[] elements = (Elements[])Enum.GetValues(typeof(Elements));

        Random random = new Random();
        string abilityType = abilityTypes[random.Next(abilityTypes.Length)];
        Elements element = elements[random.Next(elements.Length)];
        int value = random.Next(5, 26);

        switch (abilityType)
        {
            case "DamageAbility":
                return new DamageAbility
                {
                    BaseDamageAmount = value,
                    Element = element
                };
            case "HealAbility":
                if (BattleSystem.Instance.level == 1)
                {
                    return new DamageAbility
                    {
                        BaseDamageAmount = value,
                        Element = element
                    };
                }
                return new HealAbility
                {
                    BaseHealAmount = value
                };
            case "BuffAbility":
                return new BuffAbility
                {
                    BaseBuffAmount = value,
                    Element = element
                };
            default:
                throw new ArgumentException("Invalid ability type");
        }
    }

    public static void DebugPrintAbilityText(Ability ability)
    {
        Debug.Log($"Ability Type: {ability.GetType().Name}");


        if (ability is DamageAbility damageAbility)
        {
            Debug.Log($"Damage Amount: {damageAbility.BaseDamageAmount}");
            Debug.Log($"Element: {damageAbility.Element}");
        }
        else if (ability is HealAbility healAbility)
        {
            Debug.Log($"Heal Amount: {healAbility.BaseHealAmount}");
            //  Debug.Log($"Element: {healAbility.element}");
        }
        else if (ability is BuffAbility buffAbility)
        {
            Debug.Log($"Buff Amount: {buffAbility.BaseBuffAmount}");
            Debug.Log($"Element: {buffAbility.Element}");
        }
        else
        {
            Debug.Log("Unknown ability type");
        }
    }

    public static Item CreateRandomItem()
    {
        Item itemToReturn = new Item
        {
            ItemName = GenerateRandomItemName(),
            ItemDescription = "",
            ItemImage = null,
            Abilities = new List<Ability> { GenerateRandomAbility() }
        };
        itemToReturn.ItemDescription = GenerateItemDescription(itemToReturn);
        return itemToReturn;
    }

    public static EnemyCombatData CreateRandomEnemy(int level, Sprite enemySprite = null)
    {
        if (level < 3) level = 1;
        else if (level > 3 && level < 6) level = 2;
        else level = 3;

        Random random = new Random();
        int health = (int)(Math.Round(random.NextDouble() * 2 + 5) * level);
        int damage = (int)(Math.Round(random.NextDouble() * 2 + 2) * level);

        Elements element = (Elements)random.Next(Enum.GetValues(typeof(Elements)).Length);

        Debug.Log($"Health: {health}, Damage: {damage}, Element: {element}");
        return new EnemyCombatData(health)
        {
            // MaxHealth = health,
            Damage = damage,
            EnemyElement = element,
            EnemyName = CreateRandomEnemyName(level),
            EnemySprite = enemySprite
        };
    }


    public static string CreateRandomEnemyName(int level)
    {
        string[] adjectives = { "Fierce", "Vicious", "Terrifying", "Deadly", "Savage", "Ruthless", "Sinister", "Menacing", "Dreadful" };
        string[] objects = { "Dragon", "Giant", "Demon", "Beast", "Serpent", "Ogre", "Troll", "Wyrm", "Golem" };

        Random random = new Random();
        string adjective = adjectives[random.Next(adjectives.Length)];
        string obj = objects[random.Next(objects.Length)];

        string enemyName = $"{adjective} {obj}";

        return enemyName;
    }

    public static Item UpgradeItem(Item itemToUpgrade, int upgradeLevel = 1)
    {
        Item upgradedItem = new Item
        {
            ItemName = itemToUpgrade.ItemName,
            ItemDescription = itemToUpgrade.ItemDescription,
            ItemImage = itemToUpgrade.ItemImage,
            Abilities = UpgradeAbilities(itemToUpgrade.Abilities, upgradeLevel)
        };
        upgradedItem.ItemDescription = GenerateItemDescription(upgradedItem);

        itemToUpgrade.AddItemUpgrade(upgradedItem);

        return upgradedItem;
    }


    public static List<Ability> UpgradeAbilities(List<Ability> abilitiesToUpgrade, int upgradeLevel = 1)
    {
        List<Ability> upgradedAbilities = new List<Ability>();

        Random random = new Random();
        int upgradeAmount = 0;
        for (int x = 0; x < upgradeLevel; x++)
        {
            upgradeAmount += random.Next(2, 9);
        }

        foreach (Ability ability in abilitiesToUpgrade)
        {
            if (ability is DamageAbility damageAbility)
            {
                DamageAbility upgradedAbility = new DamageAbility
                {
                    BaseDamageAmount = damageAbility.BaseDamageAmount + upgradeAmount,
                    Element = damageAbility.Element
                };
                upgradedAbilities.Add(upgradedAbility);
            }
            else if (ability is HealAbility healAbility)
            {
                HealAbility upgradedAbility = new HealAbility
                {
                    BaseHealAmount = healAbility.BaseHealAmount + upgradeAmount
                };
                upgradedAbilities.Add(upgradedAbility);
            }
            else if (ability is BuffAbility buffAbility)
            {
                BuffAbility upgradedAbility = new BuffAbility
                {
                    BaseBuffAmount = buffAbility.BaseBuffAmount + upgradeAmount,
                    Element = buffAbility.Element
                };
                upgradedAbilities.Add(upgradedAbility);
            }
        }

        return upgradedAbilities;
    }
    //{objectName,objectHeight,actionName (Name of ability),actionElement (Elemnt type),actionType (DMG / Healing), actionAmount (raw value )}
    public static Item GenerateItemFromAIOutput(string[] object_properties, Sprite object_sprite)
    {
        Item itemToReturn = new Item
        {
            ItemName = object_properties[0],
            ItemDescription = "",
            ItemImage = object_sprite,
            Abilities = new List<Ability>
            {
                GenerateAbilityFromAIOutput(object_properties)
            }
        };
        itemToReturn.ItemDescription = GenerateItemDescription(itemToReturn);
        return itemToReturn;
    }


    public static Ability GenerateAbilityFromAIOutput(string[] object_properties)
    {
        switch (object_properties[4])
        {
            case " damage":
                return new DamageAbility
                {
                    BaseDamageAmount = int.Parse(object_properties[5]),
                    Element = (Elements)Enum.Parse(typeof(Elements), object_properties[3])
                };
            // break;
            case " healing":
                {
                    return new HealAbility
                    {
                        BaseHealAmount = int.Parse(object_properties[5]),
                        Element = (Elements)Enum.Parse(typeof(Elements), object_properties[3])
                    };
                }
            // break;
            default:

                return new DamageAbility
                {
                    BaseDamageAmount = int.Parse(object_properties[5]),
                    Element = (Elements)Enum.Parse(typeof(Elements), object_properties[3])
                };

        }
    }



}
