using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CardDesign
{
    /// <summary>
    /// A controler to highight sorting groups
    /// </summary>
    class Sorting_Group_Controler
    {
        Controlers control;
        /// <summary>
        /// Store and manage the sorting groups
        /// </summary>
        /// <param name="gestureControler">The one created in the MainWindow</param>
        public Sorting_Group_Controler(Controlers control) {
            this.control = control;           
        }
        public bool ContainCard(String key, Card c)
        {
            return Group_List.CardGroups.ContainsKey(key)&&
                Group_List.CardGroups[key].Contains(c);
        }
        public void GroupCard(String key, Card c) {
            Group_List.Add(key, c);
            c.SortToGroup(key);
        }
        public void RemoveCard(String key, Card c) {
            Group_List.RemoveCard(key, c);
            c.RemoveFromGroup(key);
        }
        public void HighlightCards(String key) {
            List<Card> cards = Group_List.CardGroups[key];
            foreach (Card c in cards) {
                c.Hightlight();
            }
        }
        public void DehighlightCards(String key)
        {
            List<Card> cards = Group_List.CardGroups[key];
            foreach (Card c in cards)
            {
                c.Dehightlight();
            }
        }

        public void CreateGroup(String owner, String id, String text, String textbrief, Point position) {
            Group_List.GroupBox[id] = new Menu_Sort_Box(control.MainWindow.MenuLayer, owner, id, text, textbrief);
            Group_List.GroupBox[id].IsManipulationEnabled = true;
            Group_List.GroupBox[id].IsHitTestVisible = true;
            Matrix matrix = new Matrix(1, 0, 0, 1, position.X, position.Y);
            Group_List.GroupBox[id].RenderTransform = new MatrixTransform(matrix);
            Group_List.GroupBox[id].SetStartPosition(matrix.OffsetX + Group_List.GroupBox[id].Width / 2,
                matrix.OffsetY + Group_List.GroupBox[id].Height / 2);
            control.MainWindow.MenuLayer.AddGroupButton(Group_List.GroupBox[id]);
        }
    }
}
