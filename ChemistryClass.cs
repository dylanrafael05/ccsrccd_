using System;
using System.Collections.Generic;
using ChemistryClass.ModUtils;
using ChemistryClass.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChemistryClass {
    public class ChemistryClass : Mod {

        //VARIABLES
        public static ModHotKey InteractRefinementMenu;

        static internal RefinementMenuState refinementMenu;
        //static internal RefinementMenuStateWithAR refinementMenuWithAR;
        static internal UserInterface _refinementMenu;
        static internal bool allowRefinementMenu;

        //MEMBERS
        private static ulong? _unpausedUpdateCount;
        public static ulong UnpausedUpdateCount
            => _unpausedUpdateCount.Value;

        public static bool TimeIsMultOf(int m) => UnpausedUpdateCount % (ulong)m == 0;
        public static void SparseDebug(object o) {
            if (TimeIsMultOf(60)) Main.NewText(o);
        }

        //Beaker id
        public static int BeakerTileID
            => ModContent.TileType<Tiles.Multitiles.BeakerTile>();

        //Load information
        public override void Load() {

            InteractRefinementMenu = RegisterHotKey("Open/Close Chemical Refinement Menu", "L");
            allowRefinementMenu = true;

            if (!Main.dedServ) {

                refinementMenu = new RefinementMenuState {
                    Left = 0f.ToStyleDimension(),
                    Top = 0f.ToStyleDimension(),
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.Fill
                };
                refinementMenu.Activate();

                _refinementMenu = new UserInterface();

            }

            _unpausedUpdateCount = 0;

        }

        //Unload information
        public override void Unload() {

            if (_refinementMenu != null)
                _refinementMenu.SetState(null);

            refinementMenu = null;
            _refinementMenu = null;

            _unpausedUpdateCount = null;

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

            if (InteractRefinementMenu.JustPressed && Main.playerInventory) {

                allowRefinementMenu.Invert();

                if(allowRefinementMenu) {
                    Main.PlaySound(SoundID.MenuOpen);
                } else {
                    Main.PlaySound(SoundID.MenuClose);
                }

            }

        }

        //Update UI layers
        public override void UpdateUI(GameTime gameTime) {

            if (Main.dedServ) return;

            bool menuActive = Main.playerInventory && allowRefinementMenu;

            if (menuActive && _refinementMenu.CurrentState == null) {

                refinementMenu.menu.autoRefineSlot.Item = Main.LocalPlayer.Chemistry().autoRefineItem;
                _refinementMenu.SetState(refinementMenu);

            } else if (!menuActive && _refinementMenu.CurrentState != null) {

                Main.LocalPlayer.Chemistry().autoRefineItem = refinementMenu.menu.autoRefineSlot.Item;
                _refinementMenu.SetState(null);

            }

            _refinementMenu?.Update(gameTime);

        }

        //Update unpaused count
        public override void PostUpdateEverything() {

            if (!Main.gamePaused) _unpausedUpdateCount++;

            if (_unpausedUpdateCount == ulong.MaxValue)
                _unpausedUpdateCount = 0;

        }

        //BIOME MUSIC
        public override void UpdateMusic(ref int music, ref MusicPriority priority) {

            if (Main.gameMenu || Main.menuMultiplayer ||
                Main.menuServer || Main.dedServ) return;

            if (Main.LocalPlayer.GetModPlayer<ChemistryClassPlayer>().zoneSulfur) {

                music = MusicID.Eerie;
                priority = MusicPriority.BiomeHigh;

            }

        }

    }
}