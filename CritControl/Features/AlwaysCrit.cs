using Ensage;
using Ensage.Common.Extensions;
using Ensage.Common.Threading;
using Ensage.SDK.Handlers;
using Ensage.SDK.Helpers;
using CritControl.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ensage.SDK.Service;
using Ensage.Common;
using SharpDX;
using Ensage.Common.Menu;

namespace CritControl.Features
{
    internal class AlwaysCrit
    {
        private Config Config { get; }

        private IServiceContext Context { get; }

        private Unit Target { get; set; }

        private Unit Owner { get; }

        private MenuManager Menu { get; }

        private TaskHandler Handler { get; }

        private bool enabled;

        public AlwaysCrit(Config config)
        {
            Config = config;
            Context = config.Main.context;
            Menu = config.Menu;
            Owner = config.Main.context.Owner;
            Handler = UpdateManager.Run(ExecuteAsync, true, false);

            Handler.RunAsync();
            Player.OnExecuteOrder += OnExecuteOrder;
            Drawing.OnDraw += OnDraw;
        }

        private async Task ExecuteAsync(CancellationToken token)
        {
            try
            {
                if (Game.IsPaused || !Owner.IsValid || !Owner.IsAlive || Owner.IsStunned())
                {
                    return;
                }
                if (Owner.ClassId != ClassId.CDOTA_Unit_Hero_Juggernaut &&
                    Owner.ClassId != ClassId.CDOTA_Unit_Hero_SkeletonKing &&
                    Owner.ClassId != ClassId.CDOTA_Unit_Hero_PhantomAssassin)
                {
                    return;
                }
                if (!Menu.ComboKeyItem)
                {
                    return;
                }

                if (!enabled)
                {
                    return;
                }
                if (Target == null || !Target.IsValid || !Target.IsAlive)
                {
                    Target = TargetSelector.BestAutoAttackTarget(Owner);
                }

                if (Owner.NetworkActivity == NetworkActivity.Attack || Owner.NetworkActivity == NetworkActivity.Attack2)
                {
                    var attack_point = Owner.AttackPoint();
                    
                    Owner.Stop();
                    await Await.Delay((int)(attack_point * 1000 + 100 + Game.Ping), token);
                    // if (Owner.CanAttack())
                    // {
                        if (Target == null)
                        {
                            Owner.Attack(Owner);
                        }
                        else
                        {
                            Owner.Attack(Target);
                        }
                    // }               
                }              
            }
            catch (TaskCanceledException)
            {
                // canceled
            }
            catch (Exception e)
            {
                //
            }
        }

        private void OnExecuteOrder(Player sender, ExecuteOrderEventArgs args)
        {

            if (!args.Entities.Contains(Owner) || args.IsQueued || Owner.IsChanneling() || !args.Process)
            {
                return;
            }

            if (enabled) {
                if (args.OrderId == OrderId.MoveLocation || args.OrderId == OrderId.MoveTarget)
                {
                    enabled = false;
                    // Flush Target
                    Target = null;
                    this.Owner.Stop();
                    this.Owner.Move(Game.MousePosition);
                }
            }
            if (!enabled)
            {
                if ((args.OrderId == OrderId.AttackLocation || args.OrderId == OrderId.AttackTarget || args.OrderId == OrderId.Ability))
                {
                    enabled = true;
                    
                }
            }
        }

        private void Text(string text, float heightpos, Color color, Vector2 setpos)
        {
            var pos = new Vector2(Config.Screen.X, Config.Screen.Y * heightpos) - setpos;

            Drawing.DrawText(text, "Arial", pos, new Vector2(22), color, FontFlags.None);
        }

      

        private void OnDraw(EventArgs args)
        {
            var setPos = new Vector2(
                Config.Screen.X - 60,
                Config.Screen.Y - 300);

            string text  = "Crit: " + (Config.Menu.ComboKeyItem ? "ON" : "OFF");
            Text(text, 0.84f, (Menu.ComboKeyItem ? Color.GreenYellow : Color.Red), setPos);
        }


        public void Dispose()
        {
            Player.OnExecuteOrder -= OnExecuteOrder;
            Drawing.OnDraw -= OnDraw;
        }
    }
}
