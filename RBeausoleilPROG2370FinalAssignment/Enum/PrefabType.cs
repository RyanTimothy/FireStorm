/*  PrefabType.cs
 *  The various prefab types within the game
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public enum PrefabType
    {
        None,
        Tank,
        Bullet,
        TurretTower,
        TurretTowerDamaged,
        TreeSingle,
        TreeDouble,
        TowerMuzzleFlash,
        ConcreteExplodeDebrisParticles,
        ExplodingConcreteDustCloudParticles,
        BulletDebrisParticles,
        BulletConcreteDustParticles,
        CameraTarget,
        TreeDebrisSingle,
        TreeDebrisDouble,
        Base,
        LookoutTower,
        Hanger,
        Hospital,
        BuildingA,
        HealthBarUI,
        ExplosionFire,
        ExplosionFlash,
        ExplosionDirt,
        MenuUI,
        HelpMenuUI,
        CreditMenuUI,
        TankModelOnly,
        TurretTowerModelOnly,
        WallHorizontal,
        WallVertical,
        WallDamagedEast,
        WallDamagedFullHorizontal,
        WallDamagedWest,
        WallDamagedNorth,
        WallDamagedSouth,
        WallDamagedFullVertical,
        GameMessageUI,
        TargetBuildingsUI
    }
}