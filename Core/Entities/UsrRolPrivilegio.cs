using Core.Interfaces;

namespace Core.Entities
{
    public class UsrRolPrivilegio : BaseEntity
    {
        public int IdRol { get; set; }
        public UsrRol UsrRol { get; set; }

        public int IdPrivilegio { get; set; }
        public UsrPrivilegio UsrPrivilegio { get; set; }

    }
}

