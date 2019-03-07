using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensage.Common.Menu;
using Ensage.SDK.Menu;
using SharpDX;

namespace CritControl.Utilities
{
    internal class MenuManager
    {
        private MenuFactory Factory { get; }

        public MenuItem<KeyBind> ComboKeyItem { get; }

        public MenuItem comboMenu {get;}

        public MenuManager(Config config)
        {
            Factory = MenuFactory.CreateWithTexture("Juggernaut", "npc_dota_hero_juggernaut");
            Factory.Target.SetFontColor(Color.Aqua);
            var comboMenu = Factory.Menu("Combo");
            ComboKeyItem = comboMenu.Item("Combo Key", new KeyBind('D'));          
        }


        public void Dispose()
        {
            Factory.Dispose();

        }
    }
}
