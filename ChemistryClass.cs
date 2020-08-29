using System;
using System.Collections.Generic;
using ChemistryClass.ModUtils;
using ChemistryClass.UI;
using log4net;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChemistryClass {
    public partial class ChemistryClass : Mod {

        //VARIABLES
        public static ModHotKey InteractRefinementMenu;

        internal static RefinementMenuState refinementMenu;
        internal static DecayMeterState decayMeter;
        internal static UserInterface _refinementMenu;
        internal static UserInterface _decayMeter;

        internal static ILog Logging = LogManager.GetLogger("ChemistryClass");

        private static bool? _allowRefinementMenu;
        internal static bool AllowRefinementMenu {
            get => _allowRefinementMenu.Value;
            set => _allowRefinementMenu = value;
        }

        //MEMBERS
        private static ulong? _unpausedUpdateCount;
        public static ulong UnpausedUpdateCount
            => _unpausedUpdateCount.Value;

        public static bool TimeIsMultOf(int m) => UnpausedUpdateCount % (ulong)m == 0;
        public static void SparseDebug(object o) {
            if (TimeIsMultOf(60)) Main.NewText(o);
        }

        public static ChemistryClassConfig Configuration { get; private set; }

        //Beaker id
        public static int BeakerTileID
            => ModContent.TileType<Tiles.Multitiles.BeakerTile>();

        //Load information
        public override void Load() {

            InteractRefinementMenu = RegisterHotKey("Open/Close Chemical Refinement Menu", "L");
            AllowRefinementMenu = true;

            Configuration = ModContent.GetInstance<ChemistryClassConfig>();

            if (!Main.dedServ) {

                refinementMenu = new RefinementMenuState {
                    Left = 0f.ToStyleDimension(),
                    Top = 0f.ToStyleDimension(),
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.Fill
                };
                refinementMenu.Activate();

                _refinementMenu = new UserInterface();

                decayMeter = new DecayMeterState {
                    Left = 0f.ToStyleDimension(),
                    Top = 0f.ToStyleDimension(),
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.Fill
                };
                decayMeter.Activate();

                decayMeter.decayMeter.length = Configuration.DecayMeterLength;

                _decayMeter = new UserInterface();

            }

            _unpausedUpdateCount = 0;

        }

        //Unload information
        public override void Unload() {

            if (_refinementMenu != null)
                _refinementMenu.SetState(null);

            if (_decayMeter != null)
                _decayMeter.SetState(null);

            refinementMenu = null;
            _refinementMenu = null;

            decayMeter = null;
            _decayMeter = null;

            _unpausedUpdateCount = null;

            InteractRefinementMenu = null;
            _allowRefinementMenu = null;

            Configuration = null;

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

                layers.Insert(

                    mouseTextIndex + 1,

                    new LegacyGameInterfaceLayer(
                        "ChemistryClass: Decay Meter",
                            delegate {
                                _decayMeter.Draw(Main.spriteBatch, new GameTime());
                                return true;
                            },
                        InterfaceScaleType.UI
                    )

                );
            }

        }

        //Update Input
        public override void PostUpdateInput() {

            if (InteractRefinementMenu == null || InteractRefinementMenu.GetAssignedKeys().Count < 1) return;

            if (InteractRefinementMenu.JustPressed && Main.playerInventory) {

                AllowRefinementMenu = !AllowRefinementMenu;

                if(AllowRefinementMenu) {
                    Main.PlaySound(SoundID.MenuOpen);
                } else {
                    Main.PlaySound(SoundID.MenuClose);
                }

            }

        }

        //Update UI layers
        public override void UpdateUI(GameTime gameTime) {

            if (Main.dedServ) return;

            bool menuActive = Main.playerInventory && AllowRefinementMenu;

            if (menuActive && _refinementMenu.CurrentState == null) {

                refinementMenu.menu.autoRefineSlot.Item = Main.LocalPlayer.Chemistry().autoRefineItem;
                _refinementMenu.SetState(refinementMenu);

            } else if (!menuActive && _refinementMenu.CurrentState != null) {

                Main.LocalPlayer.Chemistry().autoRefineItem = refinementMenu.menu.autoRefineSlot.Item;
                _refinementMenu.SetState(null);

            }

            if (Configuration.DecayDisplay != DecayDisplayMode.Callout && _decayMeter.CurrentState == null) {

                _decayMeter.SetState(decayMeter);

            } else if (Configuration.DecayDisplay == DecayDisplayMode.Callout && _decayMeter.CurrentState != null) {

                _decayMeter.SetState(null);

            }

            _refinementMenu?.Update(gameTime);
            _decayMeter?.Update(gameTime);

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