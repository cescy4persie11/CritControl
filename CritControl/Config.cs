using System;
using System.Windows.Input;

using Ensage;
using Ensage.Common.Menu;
using Ensage.Common.Objects.UtilityObjects;
using Ensage.SDK.Orbwalker.Modes;
using CritControl.Features;
using CritControl.Utilities;
using SharpDX;
using Ensage.SDK.Menu;
using MenuManager = CritControl.Utilities.MenuManager;

namespace CritControl

{
    internal class Config : IDisposable
    {
        public CritControl Main { get; }

        public Vector2 Screen { get; }

        public MultiSleeper MultiSleeper { get; }

        public MenuManager Menu { get; }

        private bool Disposed { get; set; }

        private AlwaysCrit AlwaysCrit { get; }

        public MenuItem<bool> TextItem { get; }

        public MenuItem<Slider> TextXItem { get; }

        public MenuItem<Slider> TextYItem { get; }

        public Config(CritControl main)
        {
            Main = main;

            Screen = new Vector2(Drawing.Width - 160, Drawing.Height);
            MultiSleeper = new MultiSleeper();

            Menu = new MenuManager(this);

            //
            AlwaysCrit = new AlwaysCrit(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {

                Menu.Dispose();
                AlwaysCrit.Dispose();
            }

            Disposed = true;
        }
    }
}