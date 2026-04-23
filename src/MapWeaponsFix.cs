using Microsoft.Extensions.Configuration;
using Sharp.Shared;
using Sharp.Shared.Enums;
using Sharp.Shared.GameEntities;
using Sharp.Shared.Listeners;
using Sharp.Shared.Objects;

namespace MS_MapWeaponsFix
{
    public class MapWeaponsFix : IModSharpModule, IEntityListener
    {
        public string DisplayName => "MapWeaponsFix";
        public string DisplayAuthor => "DarkerZ[RUS]";
        public MapWeaponsFix(ISharedSystem sharedSystem, string dllPath, string sharpPath, Version version, IConfiguration coreConfiguration, bool hotReload)
        {
            _sharedSystem = sharedSystem;
        }

        private readonly ISharedSystem _sharedSystem;

        private IConVar? g_cvar_Enable;
        bool g_bEnable = true;

        public bool Init()
        {
            g_cvar_Enable = _sharedSystem.GetConVarManager().CreateConVar("ms_mapweaponsfix", true, "Disabled/enabled [0/1]", ConVarFlags.Notify);
            if (g_cvar_Enable != null) _sharedSystem.GetConVarManager().InstallChangeHook(g_cvar_Enable, OnCvarEnableChanged);
            _sharedSystem.GetEntityManager().InstallEntityListener(this);
            return true;
        }

        public void Shutdown()
        {
            _sharedSystem.GetEntityManager().RemoveEntityListener(this);
        }

        private void OnCvarEnableChanged(IConVar conVar)
        {
            g_bEnable = conVar.GetBool();
        }

        public void OnEntityCreated(IBaseEntity entity)
        {
            if (g_bEnable && entity.Classname.StartsWith("w", StringComparison.OrdinalIgnoreCase))
            {
                Sharp.Shared.Types.Vector v = entity.GetAbsOrigin();
                v.Z += 40;
                entity.SetAbsOrigin(v);
            }
        }

        public void OnEntitySpawned(IBaseEntity entity)
        {
            if (g_bEnable && entity.Classname.StartsWith("w", StringComparison.OrdinalIgnoreCase))
            {
                Sharp.Shared.Types.Vector v = entity.GetAbsOrigin();
                v.Z -= 40;
                entity.SetAbsOrigin(v);
            }
        }

        int IEntityListener.ListenerVersion => IEntityListener.ApiVersion;
        int IEntityListener.ListenerPriority => 0;
    }
}
