using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Usuario
    {
       
        public Nullable<int> Nro { get; set; }
        public int Code { get; set; }
        public string CodUsuario { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string E_Mail { get; set; }
        public Nullable<bool> U_FlagActivo { get; set; }
        public string Activo { get; set; }
        public Nullable<bool> SuperUsuario { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public string U_fechaRegistro { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public string U_fechaModificacion { get; set; }
        public int RoleId { get; set; }
        public string Rol { get; set; }
        public bool U_Web { get; set; }
        public bool U_APP { get; set; }
        public string U_ApellidoNombre { get; set; }
        public string Contraseña { get; set; }
        public string U_CodEmpresa { get; set; }
        public bool U_AutorizaIngreso { get; set; }
    }

    public class MenuTemp
    {
        public int MenuID { get; set; }
        public string DisplayName { get; set; }
        public int ParentMenuID { get; set; }
        public int OrderNumber { get; set; }
        public string MenuURL { get; set; }
        public string MenuIcon { get; set; }
    }

    public class Permisos
    {
        public int MenuID { get; set; }
        public string DisplayName { get; set; }
        public int ParentMenuID { get; set; }
        public int PermisoType { get; set; }
        public bool Permiso { get; set; }
    }
}