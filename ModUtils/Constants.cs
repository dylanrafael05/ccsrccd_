﻿using System;
namespace ChemistryClass.ModUtils.Constants {

    /*public static class MathematicConstants {

        public const float PI = (float)Math.PI;
        public const float HALF_PI = PI / 2;
        public const float QUARTER_PI = PI / 4;
        public const float EIGHTH_PI = PI / 8;
        public const float TWO_PI = PI * 2;

        public const float ONE_RPS = PI / 30;

    }*/

    public static class TileSlope {

        public const byte None = 0;
        public const byte DownRight = 2;
        public const byte DownLeft = 3;
        public const byte UpRight = 4;
        public const byte UpLeft = 5;

    }

    public static class ProjectileAIStyle {

        public const int Custom = -1;
        public const int Bullet = 0;
        public const int Arrow = 1;
        public const int Thrown = 2;
        public const int Boomerang = 3;
        public const int Vilethorn = 4;
        public const int Starfury = 5;
        public const int Powder = 6;
        public const int Grapple = 7;
        public const int WaterBolt = 8;
        public const int MagicMissile = 9;
        public const int DirtRobBlock = 10;
        public const int ShadowOrbPet = 11;
        public const int WaterStream = 12;
        public const int Harpoon = 13;
        public const int SpikyBall = 14;
        public const int Flail = 15;
        public const int Bomb = 16;
        public const int Tombstone = 17;
        public const int DemonScythe = 18;
        public const int Spear = 19;
        public const int Chainsaw = 20;
        public const int MusicNote = 21;
        public const int IceRodBlock = 22;
        public const int Flames = 23;
        public const int CrystalStorm = 24;
        public const int Boulder = 25;
        public const int SwordBeam = 27;
        public const int IceBolt = 28;
        public const int GemBolt = 29;
        public const int MushroomSpore = 30;
        public const int Solution = 31;
        public const int BeachBall = 32;
        public const int Flare = 33;
        public const int Rocket = 34;
        public const int RopeCoil = 35;
        public const int FriendlyBee = 36;
        public const int SpearB = 37;
        public const int Flamethrower = 38;
        public const int MechanicalPiranha = 39;
        public const int Leaf = 40;
        public const int FlowerPetal = 41;
        public const int CrystalLeafA = 42;
        public const int CrystalLeafB = 43;
        public const int ChlorophyteOrb = 44;
        public const int RainCloud = 45;
        public const int RainbowGun = 46;
        public const int MagnetSphere = 47;
        public const int HeatRay = 48;
        public const int ExplosiveBunnyCannon = 49;
        public const int Inferno = 50;
        public const int LostSoul = 51;
        public const int Spiritheal = 52;
        public const int FrostHydra = 53;
        public const int Raven = 54;
        public const int HorsemansBlade = 55;
        public const int FlamingScythe = 56;
        public const int NorthPole = 57;
        public const int Present = 58;
        public const int SpectreWrath = 59;
        public const int WaterGun = 60;
        public const int Bobber = 61;
        public const int Hornet = 62;
        public const int BabySpiderPet = 63;
        public const int SharknadoA = 64;
        public const int SharknadoB = 65;
        public const int TwinsSummon = 66;
        public const int Pirate = 67;
        public const int MolotovCocktail = 68;
        public const int FlaironFlail = 69;
        public const int FlaironBubble = 70;
        public const int Typhoon = 71;
        public const int Bubble = 72;
        public const int FireworkFountain = 73;
        public const int ScutlixLaser = 74;
        public const int LaserMachinegun = 75;
        public const int ScutlixCrosshair = 76;
        public const int Electrosphere = 77;
        public const int Xenopopper = 78;
        public const int MartianDeathray = 79;
        public const int MartianRocket = 80;
        public const int InfluxWaver = 81;
        public const int PhantasmalEye = 82;
        public const int PhantasmalSphere = 83;
        public const int PhantasmalDeathray = 84;
        public const int MoonLeech = 85;
        public const int IceMist = 86;
        public const int LightningOrb = 87;
        public const int CursedFlames = 88;
        public const int LightningRitual = 89;
        public const int MagicLantern = 90;
        public const int Shadowflame = 91;
        public const int ToxicCloud = 92;
        public const int Nail = 93;
        public const int CoinPortal = 94;
        public const int ToxicBubble = 95;
        public const int IchorSplash = 96;
        public const int FlyingPiggyBank = 97;
        public const int Energy = 98;
        public const int Yoyo = 99;
        public const int MedusaRay = 100;
        public const int MechanicalMinecartRay = 101;
        public const int FlowInvader = 102;
        public const int Starmark = 103;
        public const int BrainOfConfusion = 104;
        public const int SporeA = 105;
        public const int SporeB = 106;
        public const int NebulaSphere = 107;
        public const int Vortex = 108;
        public const int MechanicsWrench = 109;
        public const int Syringe = 110;
        public const int DryadsWard = 111;
        public const int TruffleSpore = 112;
        public const int BoneJavelin = 113;
        public const int PortalGate = 114;
        public const int TerrarianProjectile = 115;
        public const int SolarFlare = 116;
        public const int SolarEruption = 117;
        public const int NebulaArcanumA = 118;
        public const int NebulaArcanumB = 119;
        public const int StardustGuardian = 120;
        public const int StardustDragon = 121;
        public const int Phantasm = 122;
        public const int LunarPortal = 123;
        public const int LunarPortalLaser = 124;
        public const int SuspiciousLookingTentacle = 125;

    }

    public static class NpcAIStyle {

        public const int Custom = -1;
        public const int None = 0;
        public const int Slime = 1;
        public const int DemonEye = 2;
        public const int Zombie = 3;
        public const int EyeOfCthulu = 4;
        public const int EaterOfSouls = 5;
        public const int Worm = 6;
        public const int Passive = 7;
        public const int Caster = 8;
        public const int Spell = 9;
        public const int CuusedSkull = 10;
        public const int Skeletron = 11;
        public const int SkeletronHand = 12;
        public const int AngryTrapper = 13;
        public const int Bat = 14;
        public const int KingSlime = 15;
        public const int Swimming = 16;
        public const int Vulture = 17;
        public const int Jellyfish = 18;
        public const int Antlion = 19;
        public const int DungeonSpikeBall = 20;
        public const int BlazingWheel = 21;
        public const int Pixie = 22;
        public const int FlyingWeapon = 23;
        public const int Bird = 24;
        public const int Mimic = 25;
        public const int Unicorn = 26;
        public const int WallOfFleshMouth = 27;
        public const int WallOfFleshEye = 28;
        public const int TheHungry = 29;
        public const int Retinazer = 30;
        public const int Spazmatism = 31;
        public const int SkeletronPrime = 32;
        public const int PrimeSaw = 33;
        public const int PrimeVice = 34;
        public const int PrimeCannon = 35;
        public const int PrimeLaser = 36;
        public const int Destroyer = 37;
        public const int Snowman = 38;
        public const int GiantTortoise = 39;
        public const int Spider = 40;
        public const int Herpling = 41;
        public const int LostGirl = 42;
        public const int QueenBee = 43;
        public const int FlyingFish = 44;
        public const int GolemBody = 45;
        public const int BoundGolemHead = 46;
        public const int GolemFist = 47;
        public const int FreeGolemHead = 48;
        public const int AngryNimbus = 49;
        public const int Spore = 50;
        public const int Plantera = 51;
        public const int PlanterasHook = 52;
        public const int PlanterasTentacle = 53;
        public const int BrainOfCthulu = 54;
        public const int BrainOfCthuluCreeper = 55;
        public const int DungeonSpirit = 56;
        public const int MourningWood = 57;
        public const int Pumpking = 58;
        public const int PumpkingScythe = 59;
        public const int IceQueen = 60;
        public const int SantaNK1 = 61;
        public const int ElfCopter = 62;
        public const int Flocko = 63;
        public const int Firefly = 64;
        public const int Butterfly = 65;
        public const int PassiveWorm = 66;
        public const int Snail = 67;
        public const int Duck = 68;
        public const int DukeFishron = 69;
        public const int DetonatingBubble = 70;
        public const int Sharkron = 71;
        public const int BubbleShield = 72;
        public const int TeslaTurret = 73;
        public const int Corite = 74;
        public const int Rider = 75;
        public const int MartianSaucer = 76;
        public const int MoonLordCore = 77;
        public const int MoonLordHand = 78;
        public const int MoonLordHead = 79;
        public const int MartianProbe = 80;
        public const int TrueEyeOfCthulu = 81;
        public const int MoonLeech = 82;
        public const int LunaticDevote = 83;
        public const int LunaticCultist = 84;
        public const int StarCell = 85;
        public const int AncientVision = 86;
        public const int BiomeMimic = 87;
        public const int Mothron = 88;
        public const int MothronEgg = 89;
        public const int BabyMothron = 90;
        public const int GraniteElemental = 91;
        public const int TargetDummy = 92;
        public const int FlyingDutchman = 93;
        public const int CelestialPillar = 94;
        public const int SmallStarCell = 95;
        public const int FlowInvader = 96;
        public const int NebulaFloater = 97;
        //public const int FireShooter = 98;
        public const int SolatFragment = 99;
        public const int AncientLight = 100;
        public const int AncientDoom = 101;
        public const int SandElemental = 102;
        public const int SandCrystal = 103;
        public const int EterniaCrystal = 105;
        public const int MysteriousPortal = 106;
        public const int EterniaAttacker = 107;
        public const int FlyingEterniaAttacker = 108;
        public const int DarkMage = 109;
        public const int Betsy = 110;
        public const int EtherianLightningBug = 111;

    }

}