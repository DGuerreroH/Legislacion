namespace LegislacionAPP.Common.Enum
{
    public static class RolSistemaExtensions
    {
        public static string AsString(this RolSistema rol) => rol switch
        {
            RolSistema.Admin => Roles.Admin,
            RolSistema.Gerente => Roles.Gerente,
            RolSistema.Auditor => Roles.Auditor,
            RolSistema.Digitalizador => Roles.Digitalizador,
            _ => throw new ArgumentOutOfRangeException(nameof(rol), rol, null)
        };

        public static RolSistema ToEnum(this string roleName) => roleName switch
        {
            Roles.Admin => RolSistema.Admin,
            Roles.Gerente => RolSistema.Gerente,
            Roles.Auditor => RolSistema.Auditor,
            Roles.Digitalizador => RolSistema.Digitalizador,
            _ => throw new ArgumentOutOfRangeException(nameof(roleName), roleName, null)
        };
    }
}
