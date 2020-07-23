using System;
using System.Collections.Generic;
using ChemistryClass.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChemistryClass
{
	public class ChemistryClass : Mod
	{

        //VARIABLES
        public static ModHotKey InteractRefinementMenu;

        internal RefinementMenuState refinementMenu;
        private UserInterface _refinementMenu;
        private bool allowRefinementMenu;

        //MEMBERS
        private static ulong? _unpausedUpdateCount;
        public static ulong UnpausedUpdateCount
            => _unpausedUpdateCount.Value;

        public static bool TimeIsMultOf(int m) => UnpausedUpdateCount % (ulong)m == 0;

        //Beaker id
        public static int BeakerTileID
            => ModContent.TileType<Tiles.BeakerTile>();

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
                _refinementMenu.SetState(refinementMenu);

            }

            _unpausedUpdateCount = 0;

        }

        //Unload information
        public override void Unload() {

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

        //Update General
        public override void PostUpdateInput() {

            if (InteractRefinementMenu.JustPressed) allowRefinementMenu.Invert();

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

            if (!Main.gamePaused) _unpausedUpdateCount++;

            if (_unpausedUpdateCount == ulong.MaxValue)
                _unpausedUpdateCount = 0;

        }

    }
}