using System;
using System.Collections.Generic;
using TUtils;
using TUtils.Timers;
using ChemistryClass.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using log4net;

namespace ChemistryClass
{
    public class ChemistryClass : Mod {

        //VARIABLES
        public static ModHotKey InteractRefinementMenu;
        public static ILog ModLogger { get; protected set; }

        internal RefinementMenuState refinementMenu;
        private UserInterface _refinementMenu;
        private bool allowRefinementMenu = true;

        private bool hasLoaded = false;

        public static bool TimeIsMultOf(int m) => TimerUtils.activeUpdateTimer % m == 0;
        public static void SparseDebug(object o) {
            if (TimeIsMultOf(60)) Main.NewText(o);
        }

        //Beaker id
        public static int BeakerTileID
            => ModContent.TileType<Tiles.Multitiles.BeakerTile>();

        //Load information
        public override void Load() {

            Logger.InfoFormat($"{Name} LOGGING:");
            ModLogger = Logger;

            InteractRefinementMenu = RegisterHotKey("Open/Close Chemical Refinement Menu", "L");

            if (!Main.dedServ) {

                refinementMenu = new RefinementMenuState {
                    Left = new StyleDimension(0f, 0f),
                    Top = new StyleDimension(0f, 0f),
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.Fill
                };
                refinementMenu.Activate();

                _refinementMenu = new UserInterface();
                _refinementMenu.SetState(refinementMenu);

            }

            hasLoaded = true;

        }

        //Unload information
        public override void Unload() {

            if(_refinementMenu != null)
                _refinementMenu.SetState(null);

            refinementMenu = null;
            _refinementMenu = null;

        }

        //Add UI Layer
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {

            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1) {

                layers.Insert(

                    mouseTextIndex,

                    new LegacyGameInterfaceLayer(
                        "ChemistryClass: Refinement Menu",
                            delegate {
                                _refinementMenu.Draw(Main.spriteBatch, new GameTime());
                                return true;
                            },
                        InterfaceScaleType.UI
                    )

                );
            }

        }

        //Update Input
        public override void PostUpdateInput() {

            if (InteractRefinementMenu.JustPressed && hasLoaded) Logic.Invert(ref allowRefinementMenu);

        }

        //Update UI layers
        public override void UpdateUI(GameTime gameTime) {

            bool menuActive = Main.playerInventory && allowRefinementMenu;

            if ( menuActive && _refinementMenu.CurrentState == null ) {

                _refinementMenu.SetState(refinementMenu);

            } else if ( !menuActive && _refinementMenu.CurrentState != null ) {

                _refinementMenu.SetState(null);

            }

            _refinementMenu?.Update(gameTime);

        }

        //Update unpaused count
        public override void PostUpdateEverything() {

            TimerUtils.UpdateActiveTimer();

        }

        //BIOME MUSIC
        public override void UpdateMusic(ref int music, ref MusicPriority priority) {

            if (Main.gameMenu || Main.menuMultiplayer ||
                Main.menuServer) return;

            if(!Main.dedServ && Main.LocalPlayer.GetModPlayer<ChemistryClassPlayer>().ZoneSulfur) {

                music = MusicID.Eerie;
                priority = MusicPriority.BiomeHigh;

            }

        }

    }
}