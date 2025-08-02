using JerarquiaEmpleadosMVC.Models;

namespace JerarquiaEmpleadosMVC.Models
{
    public class EmpleadoViewModel
    {
        public List<Empleado> Lista { get; set; } = new();
        public List<Empleado> Jerarquia { get; set; } = new();
    }
}
