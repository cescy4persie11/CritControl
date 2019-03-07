using System;
using System.ComponentModel.Composition;
using Ensage;

using Ensage.SDK.Service;
using Ensage.SDK.Service.Metadata;

namespace CritControl
{

    [ExportPlugin(
       name: "namespace SpiritBreaker")]
       // units: HeroId.npc_dota_hero_troll_warlord)]
    internal class CritControl : Plugin
    {
        public IServiceContext context;

        private readonly Unit owner;

        private Config Config { get; set; }


        [ImportingConstructor]
        public CritControl(IServiceContext context)
        {
            this.context = context;
            this.owner = context.Owner;
        }

        protected override void OnActivate()
        {
            Config = new Config(this);
        }

        protected override void OnDeactivate()
        {
            Config?.Dispose();
        }
    }
}
