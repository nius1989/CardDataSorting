using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///<summary>
///Group List stores the cards belong to the same sorting group
///It uses a string key as an identifier, which corresponds to the menubutton name
///</summary>
namespace CardDesign
{
    //The list controler for each of the card group associated with the icon
    public class Group_List
    {
        
        static Dictionary<Menu_Sort_Box, List<Card>> cardGroups = new Dictionary<Menu_Sort_Box, List<Card>>();
        static List<Menu_Sort_Box> groupBox = new List<Menu_Sort_Box>();

        public static List<Menu_Sort_Box> GroupBox
        {
            get { return Group_List.groupBox; }
            set { Group_List.groupBox = value; }
        }


        public static Dictionary<Menu_Sort_Box, List<Card>> CardGroups
        {
            get { return Group_List.cardGroups; }
            set { Group_List.cardGroups = value; }
        }

        //Add a card to the group "key"
        public static void Add(Menu_Sort_Box key, Card card)
        {
            if (!cardGroups.ContainsKey(key))
            {
                List<Card> list = new List<Card>();
                list.Add(card);
                cardGroups.Add(key, list);
            }
            else
            {
                cardGroups[key].Add(card);
            }
        }

        public static void CreateGroup(Menu_Sort_Box key) {
            groupBox.Add(key);
            if (!cardGroups.ContainsKey(key))
            {
                List<Card> list = new List<Card>();
                cardGroups.Add(key, list);
            }
        }
        //delete the group "key"
        public static void Remove(Menu_Sort_Box key)
        {
            if (cardGroups.ContainsKey(key))
            {
                foreach (Card c in cardGroups[key]) {
                    c.RemoveFromGroup(key);
                }
                cardGroups.Remove(key);

            }
        }
        //remove the card from the group "key"
        public static void RemoveCard(Menu_Sort_Box key, Card card)
        {
            if (cardGroups.ContainsKey(key))
            {
                if (cardGroups[key].Contains(card))
                {
                    card.RemoveFromGroup(key);
                    cardGroups[key].Remove(card);
                    if (cardGroups[key].Count == 0)
                    {
                        Remove(key);
                    }
                }
            }
        }
        public static bool ContainCard(Menu_Sort_Box key, Card card) {
            if (cardGroups.ContainsKey(key))
            {
                return cardGroups[key].Contains(card);
            }
            else {
                return false;
            }
        }
    }
}
